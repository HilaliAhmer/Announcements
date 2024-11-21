using Microsoft.AspNetCore.Razor.TagHelpers;

namespace MCC.Korsini.Announcements.WebUI.TagHelpers
{
    [HtmlTargetElement("textarea", Attributes = "enable-tinymce")]
    public class TinyMCEInitializationTagHelper:TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var elementId = output.Attributes.ContainsName("id") ? output.Attributes["id"].Value.ToString() : "TinyMCETextArea";

            output.PostElement.AppendHtml($@"
                <script src=""/lib/tinymce/tinymce.min.js""></script>
                <script>
                    tinymce.init({{
                        selector: '#{elementId}',
                        menubar: false,
                        plugins: 'lists link image preview',
                        toolbar: 'formatselect | bold italic | alignleft aligncenter alignright | bullist numlist outdent indent | link image | preview',
                        file_picker_types: 'image',
                        file_picker_callback: function (callback, value, meta) {{
                            if (meta.filetype === 'image') {{
                                const input = document.createElement('input');
                                input.setAttribute('type', 'file');
                                input.setAttribute('accept', 'image/*');
                                input.onchange = function () {{
                                    const file = this.files[0];
                                    const reader = new FileReader();
                                    reader.onload = function () {{
                                        callback(reader.result, {{ alt: file.name }});
                                    }};
                                    reader.readAsDataURL(file);
                                }};
                                input.click();
                            }}
                        }}
                    }});
                </script>
            ");
        }
    }
}
