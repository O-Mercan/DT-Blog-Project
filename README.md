# ASP.NET Core MVC Blog Projesi - [Doðuþ-Teknoloji]

Bu proje, Doðuþ Teknoloji kapsamýnda geliþtirilmiþ, ASP.NET Core MVC teknolojileri kullanýlarak oluþturulmuþ temel özelliklere sahip bir blog sitesidir.

## Proje Hakkýnda

Bu blog sitesi, kullanýcýlarýn kayýt olup giriþ yapabildiði, blog yazýlarý oluþturup yönetebildiði ve diðer kullanýcýlarýn yazýlarýný okuyup yorum yapabildiði bir platform sunar. Proje, MVC mimarisi, Entity Framework Core, ASP.NET Core Identity ve Repository Pattern gibi modern .NET teknolojileri ve tasarým desenleri kullanýlarak geliþtirilmiþtir.

## Özellikler

*   **Kullanýcý Yönetimi:**
    *   Kullanýcý Kayýt, Giriþ ve Çýkýþ iþlemleri (ASP.NET Core Identity)
    *   Þifre sýfýrlama/yönetimi (Identity'nin varsayýlan özellikleri)
*   **Yetkilendirme:**
    *   Sadece giriþ yapmýþ kullanýcýlar yeni yazý ekleyebilir, kendi yazýlarýný düzenleyebilir ve silebilir.
    *   Sadece giriþ yapmýþ kullanýcýlar yorum yapabilir.
    *   Misafir kullanýcýlar yazýlarý ve yorumlarý okuyabilir.
*   **Blog Yönetimi:**
    *   Blog yazýlarý için CRUD (Oluþturma, Okuma, Güncelleme, Silme) iþlemleri.
    *   Yazý Alanlarý: Baþlýk, Ýçerik, Yayýnlanma Tarihi, Yazar Bilgisi, Kategori.
*   **Kategoriler:**
    *   Yazýlar için kategori sistemi.
    *   Anasayfada kategoriye göre filtreleme.
*   **Yorumlar:**
    *   Kullanýcýlarýn blog yazýlarýna yorum býrakabilmesi.
    *   Yorumlarýn yazý detay sayfasýnda gösterilmesi.
*   **Görsel Yükleme (Opsiyonel):**
    *   Blog yazýlarýna görsel yükleme ve gösterme özelliði.
*   **UI/UX:**
    *   Bootstrap kullanýlarak oluþturulmuþ responsive tasarým.
    *   Temiz ve kullanýcý dostu arayüz.

## Kullanýlan Teknolojiler

*   **Backend:**
    *   ASP.NET Core MVC ([.NET Sürümünüz, örn. .NET 6/7/8])
    *   Entity Framework Core ([Sürüm]) - ORM
    *   ASP.NET Core Identity - Authentication & Authorization
    *   Repository Pattern - Veri Eriþim Katmaný Soyutlamasý
    *   Dependency Injection - Baðýmlýlýk Yönetimi
*   **Veritabaný:**
    *   SQL Server [veya kullandýðýnýz diðer DB: PostgreSQL, SQLite]
    *   EF Core Migrations - Veritabaný Þemasý Yönetimi
*   **Frontend:**
    *   Razor Views
    *   HTML5 / CSS3
    *   Bootstrap ([Sürüm]) - CSS Framework
    *   JavaScript / jQuery (Varsayýlan template ile gelir)
*   **Diðer:**
    *   LibMan (Ýstemci Kütüphane Yönetimi)
    *   SOLID Prensipleri (Tasarým hedefi)

## Kurulum ve Çalýþtýrma

Projeyi yerel makinenizde çalýþtýrmak için aþaðýdaki adýmlarý takip edebilirsiniz:

1.  **Ön Koþullar:**
    *   [.NET SDK](https://dotnet.microsoft.com/download) ([Kullandýðýnýz Sürüm])
    *   [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (veya kullandýðýnýz veritabaný motoru)
    *   Bir IDE (Visual Studio, VS Code, Rider vb.) veya .NET CLI

2.  **Repository'yi Klonlama:**
    ```bash
    git clone https://github.com/[GitHubKullanýcýAdýnýz]/[RepoAdýnýz].git
    cd [RepoAdýnýz]
    ```

3.  **Veritabaný Baðlantýsý:**
    *   `appsettings.json` (veya gizli tutuluyorsa `appsettings.Development.json` / User Secrets) dosyasýný açýn.
    *   `ConnectionStrings` bölümündeki `"DefaultConnection"` deðerini kendi yerel veritabaný baðlantý cümlenizle güncelleyin. Örnek:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogDbStudyCase;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Veritabaný Migrations:**
    *   Veritabanýný ve tablolarý oluþturmak/güncellemek için aþaðýdaki komutu Package Manager Console (Visual Studio'da `Tools > NuGet Package Manager > Package Manager Console`) veya projenin kök dizininde bir terminalde çalýþtýrýn:
        ```powershell
        dotnet ef database update
        ```
        *(Eðer `Migrations` klasörü reponuzda yoksa veya ilk kez siz oluþturacaksanýz, önce `dotnet ef migrations add InitialCreate` gibi bir komutla migration oluþturmanýz gerekebilir.)*
    *   Bu iþlem, `Comment` tablosu dahil tüm tablolarý ve iliþkileri oluþturacak ve varsa Seed Data'yý (örn. Kategoriler) ekleyecektir.

5.  **Uygulamayý Çalýþtýrma:**
    *   **Visual Studio:** `F5` tuþuna basýn veya Debug menüsünden `Start Debugging`'i seçin.
    *   **.NET CLI:** Projenin kök dizininde aþaðýdaki komutu çalýþtýrýn:
        ```bash
        dotnet run
        ```
    *   Uygulama varsayýlan olarak belirtilen portta (örn. `https://localhost:5001` veya `http://localhost:5000`) çalýþmaya baþlayacaktýr. Tarayýcýnýzda bu adresi açýn.
