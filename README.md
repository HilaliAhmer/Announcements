# Announcements - ASP.NET Core MVC Project

## 📖 Project Overview

**Announcements**, kurum içi duyuruların yönetimi için geliştirilmiş bir ASP.NET Core MVC projesidir. Bu platform, IT ekipleri ve diğer kullanıcılar arasındaki iletişimi kolaylaştırmak için kesintiler, planlı çalışmalar, prosedürlere ve kullanıcı kılavuzlarına hızlı erişim sağlayacak bir duyuru sistemi sunar. Kullanıcılar duyuruları e-posta yoluyla alabilir ve duyurular belirli günlerde planlanabilir.

## 🛠️ Architecture and Technology Stack

### Architecture

Proje, aşağıdaki katmanlı mimariyi kullanarak geliştirilmiştir:

- **Business Layer:** İş mantığı katmanı.
- **Core Layer:** Ortak bileşenler ve yapıların bulunduğu katman.
- **DataAccess Layer:** Veri erişim işlemlerini yöneten katman.
- **Entities Layer:** Varlık sınıflarının tanımlandığı katman.
- **Web UI Layer:** Kullanıcı arayüzünü sağlayan katman.

### Technology Stack

- **Backend:** ASP.NET Core MVC 8.0
- **Frontend:**
  - Bootstrap 5.3.x
  - jQuery 3.6.x
  - FontAwesome 6.x
  - SCSS 1.62.x
  - CSS (Bootstrap ile entegre)
- **Database:** SQL Server
- **ORM:** Entity Framework Core 8.0.0

## 🖥️ Installation and Setup

### Prerequisites

Projeyi çalıştırmadan önce aşağıdaki bileşenlerin yüklü olduğundan emin olun:

- **.NET SDK 8.0** veya daha üstü
- **Visual Studio 2022** (ya da ASP.NET Core MVC destekleyen başka bir IDE)
- **SQL Server**

### Installation Steps

1. **Depoyu Klonlayın**  
Depoyu yerel makinenize klonlayın:
```bash
git clone https://github.com/HilaliAhmer/Announcements.git
```

2. **Projeyi Açın**  
Visual Studio 2022 veya Visual Studio Code kullanarak projeyi açın.

3. **Gerekli Bağımlılıkları Yükleyin**  
NuGet bağımlılıklarını yükleyin:
   - Visual Studio'da `Tools > NuGet Package Manager > Manage NuGet Packages for Solution` yolunu izleyin.
   - Tüm bağımlılıkların yüklendiğinden emin olun.

4. **Veritabanını Yapılandırın**  
   - `appsettings.json` dosyasını açarak veritabanı bağlantı ayarlarını yapılandırın.
   - Aşağıdaki komutu kullanarak migration işlemini çalıştırın:
```bash
dotnet ef database update
```

5. **Uygulamayı Çalıştırın**  
Projeyi çalıştırmak için şu komutu kullanın:
```bash
dotnet run
```

6. **Web Tarayıcınızdan Erişim Sağlayın**  
Aşağıdaki URL'yi ziyaret ederek uygulamayı çalıştırabilirsiniz:
```bash
http://localhost:5000
```

## 📄 License

Bu proje, [MIT Lisansı](LICENSE) altında lisanslanmıştır.
