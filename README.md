# ASP.NET Core MVC Blog Projesi - [Do�u�-Teknoloji]

Bu proje, Do�u� Teknoloji kapsam�nda geli�tirilmi�, ASP.NET Core MVC teknolojileri kullan�larak olu�turulmu� temel �zelliklere sahip bir blog sitesidir.

## Proje Hakk�nda

Bu blog sitesi, kullan�c�lar�n kay�t olup giri� yapabildi�i, blog yaz�lar� olu�turup y�netebildi�i ve di�er kullan�c�lar�n yaz�lar�n� okuyup yorum yapabildi�i bir platform sunar. Proje, MVC mimarisi, Entity Framework Core, ASP.NET Core Identity ve Repository Pattern gibi modern .NET teknolojileri ve tasar�m desenleri kullan�larak geli�tirilmi�tir.

## �zellikler

*   **Kullan�c� Y�netimi:**
    *   Kullan�c� Kay�t, Giri� ve ��k�� i�lemleri (ASP.NET Core Identity)
    *   �ifre s�f�rlama/y�netimi (Identity'nin varsay�lan �zellikleri)
*   **Yetkilendirme:**
    *   Sadece giri� yapm�� kullan�c�lar yeni yaz� ekleyebilir, kendi yaz�lar�n� d�zenleyebilir ve silebilir.
    *   Sadece giri� yapm�� kullan�c�lar yorum yapabilir.
    *   Misafir kullan�c�lar yaz�lar� ve yorumlar� okuyabilir.
*   **Blog Y�netimi:**
    *   Blog yaz�lar� i�in CRUD (Olu�turma, Okuma, G�ncelleme, Silme) i�lemleri.
    *   Yaz� Alanlar�: Ba�l�k, ��erik, Yay�nlanma Tarihi, Yazar Bilgisi, Kategori.
*   **Kategoriler:**
    *   Yaz�lar i�in kategori sistemi.
    *   Anasayfada kategoriye g�re filtreleme.
*   **Yorumlar:**
    *   Kullan�c�lar�n blog yaz�lar�na yorum b�rakabilmesi.
    *   Yorumlar�n yaz� detay sayfas�nda g�sterilmesi.
*   **G�rsel Y�kleme (Opsiyonel):**
    *   Blog yaz�lar�na g�rsel y�kleme ve g�sterme �zelli�i.
*   **UI/UX:**
    *   Bootstrap kullan�larak olu�turulmu� responsive tasar�m.
    *   Temiz ve kullan�c� dostu aray�z.

## Kullan�lan Teknolojiler

*   **Backend:**
    *   ASP.NET Core MVC ([.NET S�r�m�n�z, �rn. .NET 6/7/8])
    *   Entity Framework Core ([S�r�m]) - ORM
    *   ASP.NET Core Identity - Authentication & Authorization
    *   Repository Pattern - Veri Eri�im Katman� Soyutlamas�
    *   Dependency Injection - Ba��ml�l�k Y�netimi
*   **Veritaban�:**
    *   SQL Server [veya kulland���n�z di�er DB: PostgreSQL, SQLite]
    *   EF Core Migrations - Veritaban� �emas� Y�netimi
*   **Frontend:**
    *   Razor Views
    *   HTML5 / CSS3
    *   Bootstrap ([S�r�m]) - CSS Framework
    *   JavaScript / jQuery (Varsay�lan template ile gelir)
*   **Di�er:**
    *   LibMan (�stemci K�t�phane Y�netimi)
    *   SOLID Prensipleri (Tasar�m hedefi)

## Kurulum ve �al��t�rma

Projeyi yerel makinenizde �al��t�rmak i�in a�a��daki ad�mlar� takip edebilirsiniz:

1.  **�n Ko�ullar:**
    *   [.NET SDK](https://dotnet.microsoft.com/download) ([Kulland���n�z S�r�m])
    *   [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (veya kulland���n�z veritaban� motoru)
    *   Bir IDE (Visual Studio, VS Code, Rider vb.) veya .NET CLI

2.  **Repository'yi Klonlama:**
    ```bash
    git clone https://github.com/[GitHubKullan�c�Ad�n�z]/[RepoAd�n�z].git
    cd [RepoAd�n�z]
    ```

3.  **Veritaban� Ba�lant�s�:**
    *   `appsettings.json` (veya gizli tutuluyorsa `appsettings.Development.json` / User Secrets) dosyas�n� a��n.
    *   `ConnectionStrings` b�l�m�ndeki `"DefaultConnection"` de�erini kendi yerel veritaban� ba�lant� c�mlenizle g�ncelleyin. �rnek:
        ```json
        "ConnectionStrings": {
          "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BlogDbStudyCase;Trusted_Connection=True;MultipleActiveResultSets=true"
        }
        ```

4.  **Veritaban� Migrations:**
    *   Veritaban�n� ve tablolar� olu�turmak/g�ncellemek i�in a�a��daki komutu Package Manager Console (Visual Studio'da `Tools > NuGet Package Manager > Package Manager Console`) veya projenin k�k dizininde bir terminalde �al��t�r�n:
        ```powershell
        dotnet ef database update
        ```
        *(E�er `Migrations` klas�r� reponuzda yoksa veya ilk kez siz olu�turacaksan�z, �nce `dotnet ef migrations add InitialCreate` gibi bir komutla migration olu�turman�z gerekebilir.)*
    *   Bu i�lem, `Comment` tablosu dahil t�m tablolar� ve ili�kileri olu�turacak ve varsa Seed Data'y� (�rn. Kategoriler) ekleyecektir.

5.  **Uygulamay� �al��t�rma:**
    *   **Visual Studio:** `F5` tu�una bas�n veya Debug men�s�nden `Start Debugging`'i se�in.
    *   **.NET CLI:** Projenin k�k dizininde a�a��daki komutu �al��t�r�n:
        ```bash
        dotnet run
        ```
    *   Uygulama varsay�lan olarak belirtilen portta (�rn. `https://localhost:5001` veya `http://localhost:5000`) �al��maya ba�layacakt�r. Taray�c�n�zda bu adresi a��n.
