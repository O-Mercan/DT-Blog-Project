# ASP.NET Core MVC Blog Projesi - Doğuş-Teknoloji

Bu proje, Doğuş Teknoloji kapsamında geliştirilmiş, ASP.NET Core MVC teknolojileri kullanılarak oluşturulmuş temel özelliklere sahip bir blog sitesidir.

## Proje Hakkında

Bu blog sitesi, kullanıcıların kayıt olup giriş yapabildiği, blog yazıları oluşturup yönetebildiği ve diğer kullanıcıların yazılarını okuyup yorum yapabildiği bir platform sunar. Proje, MVC mimarisi, Entity Framework Core, ASP.NET Core Identity ve Repository Pattern gibi modern .NET teknolojileri ve tasarım desenleri kullanılarak geliştirilmiştir.

## Özellikler

*   **Kullanıcı Yönetimi:**
    *   Kullanıcı Kayıt, Giriş ve Çıkış işlemleri (ASP.NET Core Identity)
    *   Şifre sıfırlama/yönetimi (Identity'nin varsayılan özellikleri)
*   **Yetkilendirme:**
    *   Sadece giriş yapmış kullanıcılar yeni yazı ekleyebilir, kendi yazılarını düzenleyebilir ve silebilir.
    *   Sadece giriş yapmış kullanıcılar yorum yapabilir.
    *   Misafir kullanıcılar yazıları ve yorumları okuyabilir.
*   **Blog Yönetimi:**
    *   Blog yazıları için CRUD (Oluşturma, Okuma, Güncelleme, Silme) işlemleri.
    *   Yazı Alanları: Başlık, İçerik, Yayınlanma Tarihi, Yazar Bilgisi, Kategori.
*   **Kategoriler:**
    *   Yazılar için kategori sistemi.
    *   Anasayfada kategoriye göre filtreleme.
*   **Yorumlar:**
    *   Kullanıcıların blog yazılarına yorum bırakabilmesi.
    *   Yorumların yazı detay sayfasında gösterilmesi.
*   **Görsel Yükleme (Opsiyonel):**
    *   Blog yazılarına görsel yükleme ve gösterme özelliği.
*   **UI/UX:**
    *   Bootstrap kullanılarak oluşturulmuş responsive tasarım.
    *   Temiz ve kullanıcı dostu arayüz.

## Kullanılan Teknolojiler

*   **Backend:**
    *   ASP.NET Core MVC ([.NET 8])
    *   Entity Framework Core  - ORM
    *   ASP.NET Core Identity - Authentication & Authorization
    *   Repository Pattern - Veri Erişim Katmanı Soyutlaması
    *   Dependency Injection - Bağımlılık Yönetimi
*   **Veritabanı:**
    *   SQL Server [veya kullandığınız diğer DB: PostgreSQL, SQLite]
    *   EF Core Migrations - Veritabanı Şeması Yönetimi
*   **Frontend:**
    *   Razor Views
    *   HTML5 / CSS3
    *   Bootstrap ([Sürüm]) - CSS Framework
    *   JavaScript / jQuery (Varsayılan template ile gelir)
*   **Diğer:**
    *   LibMan (İstemci Kütüphane Yönetimi)
    *   SOLID Prensipleri (Tasarım hedefi)

## Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1.  **Ön Koşullar:**
    *   [.NET SDK](https://dotnet.microsoft.com/download) ([Kullandığınız Sürüm])
    *   [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (veya kullandığınız veritabanı motoru)
    *   Bir IDE (Visual Studio, VS Code, Rider vb.) veya .NET CLI

2.  **Repository'yi Klonlama:**
    ```bash
    git clone https://github.com/[GitHubKullanıcıAdınız]/[RepoAdınız].git
    cd [RepoAdınız]
    ```

3.  **Veritabanı Bağlantısı:**
    *   `appsettings.json` (veya gizli tutuluyorsa `appsettings.Development.json` / User Secrets) dosyasını açın.
    *   `ConnectionStrings` bölümündeki `"DefaultConnection"` değerini kendi yerel veritabanı bağlantı cümlenizle güncelleyin. Örnek:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogDbStudyCase;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Veritabanı Migrations:**
    *   Veritabanını ve tabloları oluşturmak/güncellemek için aşağıdaki komutu Package Manager Console (Visual Studio'da `Tools > NuGet Package Manager > Package Manager Console`) veya projenin kök dizininde bir terminalde çalıştırın:
        ```powershell
        dotnet ef database update
        ```
        *(Eğer `Migrations` klasörü reponuzda yoksa veya ilk kez siz oluşturacaksanız, önce `dotnet ef migrations add InitialCreate` gibi bir komutla migration oluşturmanız gerekebilir.)*
    *   Bu işlem, `Comment` tablosu dahil tüm tabloları ve ilişkileri oluşturacak ve varsa Seed Data'yı (örn. Kategoriler) ekleyecektir.

5.  **Uygulamayı Çalıştırma:**
    *   **Visual Studio:** `F5` tuşuna basın veya Debug menüsünden `Start Debugging`'i seçin.
    *   **.NET CLI:** Projenin kök dizininde aşağıdaki komutu çalıştırın:
        ```bash
        dotnet run
        ```
    *   Uygulama varsayılan olarak belirtilen portta (örn. `https://localhost:5001` veya `http://localhost:5000`) çalışmaya başlayacaktır. Tarayıcınızda bu adresi açın.
