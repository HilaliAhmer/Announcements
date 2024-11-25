﻿using Novell.Directory.Ldap;
using Microsoft.Extensions.Options;
using MCC.Korsini.Announcements.WebUI.Models;
using System.Linq;

public class LdapAuthenticationService
{
    private readonly LdapSettings _ldapSettings;

    public LdapAuthenticationService(IOptions<LdapSettings> ldapSettings)
    {
        _ldapSettings = ldapSettings.Value;
    }

    private LdapConnection CreateConnection()
    {
        try
        {
            Console.WriteLine($"Connecting to LDAP server: {_ldapSettings.Server}:{_ldapSettings.Port}");
            var connection = new LdapConnection { SecureSocketLayer = false };
            connection.Connect(_ldapSettings.Server, _ldapSettings.Port);

            Console.WriteLine($"Binding with service account: {_ldapSettings.UserName}");
            connection.Bind(_ldapSettings.UserName, _ldapSettings.Password);

            Console.WriteLine("LDAP connection established with service account.");
            return connection;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP connection error: {ex.Message}");
            throw new Exception("Unable to connect to LDAP server. Check the configuration and credentials.");
        }
    }

    public bool Authenticate(string email, string password)
    {
        try
        {
            using var connection = CreateConnection();

            // Kullanıcıyı email ile arayın
            var searchFilter = $"(mail={email})";
            Console.WriteLine($"Searching for user with filter: {searchFilter}");
            var searchResults = connection.Search(
                _ldapSettings.BaseDN,
                LdapConnection.ScopeSub,
                searchFilter,
                new[] { "distinguishedName" },
                false
            );

            if (!searchResults.HasMore())
            {
                Console.WriteLine("User not found in LDAP.");
                return false;
            }

            var userEntry = searchResults.Next();
            var userDn = userEntry.Dn;
            Console.WriteLine($"Found user DN: {userDn}");

            // Kullanıcı doğrulaması için bind işlemi
            using var userConnection = new LdapConnection { SecureSocketLayer = false };
            userConnection.Connect(_ldapSettings.Server, _ldapSettings.Port);

            Console.WriteLine($"Binding with user DN: {userDn}");
            userConnection.Bind(userDn, password);

            Console.WriteLine("User authenticated successfully.");
            return true;
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP authentication error: {ex.Message}");
            return false;
        }
    }

    public bool IsUserAuthorized(string email)
    {
        try
        {
            using var connection = CreateConnection();

            // Kullanıcıyı email ile arayın
            var searchFilter = $"(mail={email})";
            Console.WriteLine($"Searching for user with filter: {searchFilter}");
            var searchResults = connection.Search(
                _ldapSettings.BaseDN,
                LdapConnection.ScopeSub,
                searchFilter,
                new[] { "memberOf" },
                false
            );

            if (!searchResults.HasMore())
            {
                Console.WriteLine("User not found in LDAP.");
                return false;
            }

            var userEntry = searchResults.Next();
            var groups = userEntry.GetAttribute("memberOf")?.StringValueArray ?? Array.Empty<string>();

            // Kullanıcının belirtilen grupta olup olmadığını kontrol et
            Console.WriteLine($"Checking if user belongs to group: {_ldapSettings.DistinguishedGroupName}");
            return groups.Any(g => string.Equals(g, _ldapSettings.DistinguishedGroupName, StringComparison.OrdinalIgnoreCase));
        }
        catch (LdapException ex)
        {
            Console.WriteLine($"LDAP group membership check error: {ex.Message}");
            return false;
        }
    }
}