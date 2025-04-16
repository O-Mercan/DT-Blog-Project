using dogus_study_case.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using dogus_study_case.Models;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using dogus_study_case.Data; 
using Microsoft.AspNetCore.Hosting; 
using dogus_study_case.ViewModels; 
using System.IO; 

namespace dogus_study_case.Controllers
{
    [Authorize]
    public class PostsController : Controller
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<PostsController> _logger;
        private readonly ApplicationDbContext _context; 
        private readonly IWebHostEnvironment _webHostEnvironment; 

        public PostsController(
            IBlogPostRepository blogPostRepository,
            ICategoryRepository categoryRepository,
            UserManager<IdentityUser> userManager,
            ILogger<PostsController> logger,
            ApplicationDbContext context,
            IWebHostEnvironment webHostEnvironment) 
        {
            _blogPostRepository = blogPostRepository ?? throw new ArgumentNullException(nameof(blogPostRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment)); 
        }

        // --- Index ---
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? categoryId)
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.ErrorMessage = TempData["ErrorMessage"];
            ViewBag.Categories = Enumerable.Empty<Category>();
            ViewBag.SelectedCategoryId = null;

            try
            {
                try
                {
                    var categories = await _categoryRepository.GetAllAsync();
                    ViewBag.Categories = categories.OrderBy(c => c.Name);
                }
                catch (Exception catEx)
                {
                    _logger.LogError(catEx, "Kategori listesi getirilirken hata oluştu (Index Action).");
                    if (string.IsNullOrEmpty(ViewBag.ErrorMessage))
                        ViewBag.ErrorMessage = "Kategoriler yüklenirken bir sorun oluştu.";
                }

                IEnumerable<BlogPost> posts;
                if (categoryId.HasValue && categoryId.Value > 0)
                {
                    _logger.LogInformation("Blog yazıları kategoriye göre filtreleniyor. KategoriID: {CategoryId}", categoryId.Value);
                    var allPosts = await _blogPostRepository.GetAllWithDetailsAsync();
                    posts = allPosts.Where(p => p.CategoryId == categoryId.Value);
                    ViewBag.SelectedCategoryId = categoryId.Value;
                    var selectedCategory = (ViewBag.Categories as IEnumerable<Category>)?.FirstOrDefault(c => c.Id == categoryId.Value);
                    ViewBag.SelectedCategoryName = selectedCategory?.Name ?? $"Kategori ID: {categoryId.Value}";
                }
                else
                {
                    _logger.LogInformation("Tüm blog yazıları listeleniyor.");
                    posts = await _blogPostRepository.GetAllWithDetailsAsync();
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog yazıları listelenirken bir hata oluştu (Genel Catch). KategoriID: {CategoryId}", categoryId);
                if (string.IsNullOrEmpty(ViewBag.ErrorMessage))
                { ViewBag.ErrorMessage = "Yazılar yüklenirken bir sorun oluştu. Lütfen daha sonra tekrar deneyin."; }
                if (ViewBag.Categories == null) ViewBag.Categories = Enumerable.Empty<Category>();
                return View(new List<BlogPost>());
            }
        }

        // --- Details ---
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {

            if (!id.HasValue) { /*...*/ return NotFound(); }
            try
            {
                var blogPost = await _blogPostRepository.GetByIdWithDetailsAsync(id.Value); 
                if (blogPost == null) { /*...*/ return NotFound(); }
                return View(blogPost);
            }
            catch (Exception ex) { /*...*/ return RedirectToAction(nameof(Index)); }
        }

        // --- Create (GET) ---
        public async Task<IActionResult> Create()
        {
            try
            {
                await PopulateCategoriesDropDownList();
                return View(new BlogPostCreateViewModel());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yeni yazı oluşturma formu yüklenirken hata oluştu (Kategoriler).");
                TempData["ErrorMessage"] = "Form yüklenirken bir hata oluştu. Kategoriler getirilemedi.";
                return RedirectToAction(nameof(Index));
            }
        }

        // --- Create (POST) ---  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostCreateViewModel viewModel) 
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            if (viewModel.CategoryId <= 0)
            {
                ModelState.AddModelError(nameof(viewModel.CategoryId), "Lütfen geçerli bir kategori seçin.");
            }

            string? relativeImagePath = null; 
            string? uploadedFilePathServer = null; 


            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                try
                {
                    relativeImagePath = await SaveUploadedFile(viewModel.ImageFile); 
                    uploadedFilePathServer = Path.Combine(_webHostEnvironment.WebRootPath, relativeImagePath.TrimStart('/', '\\'));
                }
                catch (IOException ioEx)
                {
                    _logger.LogError(ioEx, "Görsel kaydedilirken G/Ç hatası oluştu. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", "Görsel yüklenirken bir dosya hatası oluştu. Lütfen tekrar deneyin.");
                }
                catch (ArgumentException argEx)
                { 
                    _logger.LogWarning(argEx, "Geçersiz görsel dosyası denemesi. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", argEx.Message); 
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Görsel kaydedilirken beklenmedik hata oluştu. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("", "Görsel yüklenirken beklenmedik bir hata oluştu.");
                }
            }

            if (ModelState.IsValid)
            {

                BlogPost blogPost = new BlogPost
                {
                    Title = viewModel.Title,
                    Content = viewModel.Content,
                    CategoryId = viewModel.CategoryId,
                    UserId = userId,
                    PublicationDate = DateTime.UtcNow,
                    ImagePath = relativeImagePath 
                };

                try
                {
                    await _blogPostRepository.AddAsync(blogPost);
                    await _blogPostRepository.SaveChangesAsync(); 
                    _logger.LogInformation("Yeni blog yazısı oluşturuldu. ID: {PostId}, YazarID: {UserId}, Görsel: {ImagePath}", blogPost.Id, userId, blogPost.ImagePath);
                    TempData["SuccessMessage"] = "Blog yazısı başarıyla oluşturuldu.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogError(ex, "Blog yazısı DB'ye kaydedilirken hata oluştu.");
                    ModelState.AddModelError("", "Kayıt sırasında bir veritabanı hatası oluştu.");

                    DeleteUploadedFileFromServer(uploadedFilePathServer);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Blog yazısı kaydedilirken beklenmedik hata oluştu.");
                    ModelState.AddModelError("", $"Beklenmedik bir hata oluştu.");

                    DeleteUploadedFileFromServer(uploadedFilePathServer);
                }
            }

            _logger.LogWarning("Blog yazısı oluşturma işlemi ModelState validasyonu geçemedi veya DB hatası oluştu. YazarID: {UserId}", userId);
            LogModelStateErrors();

            DeleteUploadedFileFromServer(uploadedFilePathServer); 

            try { await PopulateCategoriesDropDownList(viewModel.CategoryId); }
            catch (Exception catEx) { _logger.LogError(catEx, "Hata sonrası kategori listesi doldurulamadı."); ViewBag.Categories = new SelectList(Enumerable.Empty<SelectListItem>()); ModelState.AddModelError("", "Kategoriler yüklenemedi."); }
            return View(viewModel); 
        }


        // --- Edit (GET) ---  
        public async Task<IActionResult> Edit(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound("Düzenlenecek yazı için bir ID belirtilmedi.");
            }

            try
            {
                var blogPost = await _blogPostRepository.GetByIdAsync(id.Value);
                if (blogPost == null)
                {
                    return NotFound($"'{id.Value}' ID'li blog yazısı bulunamadı.");
                }

                var currentUserId = _userManager.GetUserId(User);
                if (blogPost.UserId != currentUserId)
                {
                    return Forbid();
                }

                var viewModel = new BlogPostEditViewModel
                {
                    Id = blogPost.Id,
                    Title = blogPost.Title,
                    Content = blogPost.Content,
                    CategoryId = blogPost.CategoryId,
                    ExistingImagePath = blogPost.ImagePath 
                };

                await PopulateCategoriesDropDownList(blogPost.CategoryId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazı düzenleme formu yüklenirken hata oluştu. ID: {PostId}", id.Value);
                TempData["ErrorMessage"] = "Düzenleme formu yüklenirken bir sorun oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }

        // --- Edit (POST) --- ViewModel ve Dosya Yükleme ile GÜNCELLENDİ ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostEditViewModel viewModel) 
        {
            if (id != viewModel.Id)
            {
                return BadRequest("İstek geçersiz, ID uyuşmazlığı var.");
            }

            // Orijinal postu veritabanından ÇEK
            var originalPost = await _blogPostRepository.GetByIdAsync(id);
            if (originalPost == null)
            {
                return NotFound($"'{id}' ID'li blog yazısı bulunamadı.");
            }

            // Yetkilendirme Kontrolü
            var currentUserId = _userManager.GetUserId(User);
            if (originalPost.UserId != currentUserId)
            {
                return Forbid();
            }

            // Kategori validasyonu
            if (viewModel.CategoryId <= 0)
            {
                ModelState.AddModelError(nameof(viewModel.CategoryId), "Lütfen geçerli bir kategori seçin.");
            }

            string? newRelativeImagePath = null; 
            string? newUploadedFilePathServer = null; 
            string? oldImagePathToDelete = null; 

            if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
            {
                try
                {
                    newRelativeImagePath = await SaveUploadedFile(viewModel.ImageFile);
                    newUploadedFilePathServer = Path.Combine(_webHostEnvironment.WebRootPath, newRelativeImagePath.TrimStart('/', '\\'));
                    oldImagePathToDelete = originalPost.ImagePath; 
                }
                catch (IOException ioEx)
                {
                    _logger.LogError(ioEx, "Yeni görsel kaydedilirken G/Ç hatası. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", "Yeni görsel yüklenirken bir dosya hatası oluştu.");
                }
                catch (ArgumentException argEx)
                {
                    _logger.LogWarning(argEx, "Geçersiz yeni görsel dosyası denemesi. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("ImageFile", argEx.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Yeni görsel kaydedilirken beklenmedik hata. Dosya: {FileName}", viewModel.ImageFile.FileName);
                    ModelState.AddModelError("", "Yeni görsel yüklenirken beklenmedik bir hata oluştu.");
                }
            }

            if (ModelState.IsValid) 
            {
                try
                {

                    originalPost.Title = viewModel.Title;
                    originalPost.Content = viewModel.Content;
                    originalPost.CategoryId = viewModel.CategoryId;

                    if (newRelativeImagePath != null || oldImagePathToDelete != null)
                    {
                        originalPost.ImagePath = newRelativeImagePath; 
                    }

                    await _blogPostRepository.SaveChangesAsync(); 

                    if (!string.IsNullOrEmpty(oldImagePathToDelete))
                    {
                        DeleteUploadedFileFromServer(Path.Combine(_webHostEnvironment.WebRootPath, oldImagePathToDelete.TrimStart('/', '\\')));
                    }

                    _logger.LogInformation("Blog yazısı güncellendi. ID: {PostId}, YazarID: {UserId}, Görsel: {ImagePath}", originalPost.Id, currentUserId, originalPost.ImagePath);
                    TempData["SuccessMessage"] = "Blog yazısı başarıyla güncellendi.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException ex) { /*...*/ ModelState.AddModelError(string.Empty, "..."); }
                catch (DbUpdateException ex) { /*...*/ ModelState.AddModelError("", "..."); DeleteUploadedFileFromServer(newUploadedFilePathServer); /* Yeni yüklenen dosyayı sil */ }
                catch (Exception ex) { /*...*/ ModelState.AddModelError("", "..."); DeleteUploadedFileFromServer(newUploadedFilePathServer); /* Yeni yüklenen dosyayı sil */ }
            }


            _logger.LogWarning("Blog yazısı güncelleme işlemi ViewModel validasyonu geçemedi veya DB hatası oluştu. PostID: {PostId}", id);
            LogModelStateErrors();

            DeleteUploadedFileFromServer(newUploadedFilePathServer);

            try { await PopulateCategoriesDropDownList(viewModel.CategoryId); } catch { /*...*/ }

            viewModel.ExistingImagePath = originalPost.ImagePath;
            return View(viewModel);
        }


        // --- Delete (GET) ---
        public async Task<IActionResult> Delete(int? id)
        {
            if (!id.HasValue) { /*...*/ return NotFound(); }
            try
            {
                var blogPost = await _blogPostRepository.GetByIdWithDetailsAsync(id.Value);
                if (blogPost == null) { /*...*/ return NotFound(); }
                var currentUserId = _userManager.GetUserId(User); if (blogPost.UserId != currentUserId) { /*...*/ return Forbid(); }
                return View(blogPost);
            }
            catch (Exception ex) { /*...*/ return RedirectToAction(nameof(Index)); }
        }

        // --- Delete (POST) --- GÜNCELLENDİ  ---
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            string? imagePathToDeleteServer = null;
            try
            {
                var blogPost = await _blogPostRepository.GetByIdAsync(id);
                if (blogPost == null)
                {
                    return RedirectToAction(nameof(Index)); 
                }

                var currentUserId = _userManager.GetUserId(User);
                if (blogPost.UserId != currentUserId)
                {
                    return Forbid(); 
                }
                if (!string.IsNullOrEmpty(blogPost.ImagePath))
                {
                    imagePathToDeleteServer = Path.Combine(_webHostEnvironment.WebRootPath, blogPost.ImagePath.TrimStart('/', '\\'));
                }

                await _blogPostRepository.DeleteAsync(blogPost);
                await _blogPostRepository.SaveChangesAsync();

                DeleteUploadedFileFromServer(imagePathToDeleteServer);

                _logger.LogInformation("Blog yazısı ve ilişkili görsel (varsa) silindi. ID: {PostId}, YazarID: {UserId}", id, currentUserId);
                TempData["SuccessMessage"] = "Blog yazısı başarıyla silindi.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Blog yazısı silinirken veritabanı hatası (ilişkili veri olabilir). PostID: {PostId}", id);
                TempData["ErrorMessage"] = "Yazı silinirken bir veritabanı hatası oluştu. İlişkili yorumlar olabilir.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Blog yazısı silinirken beklenmedik hata. PostID: {PostId}", id);
                TempData["ErrorMessage"] = "Yazı silinirken beklenmedik bir hata oluştu.";
                return RedirectToAction(nameof(Index));
            }
        }


        // --- AddComment (POST) ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddComment([Bind("BlogPostId,Content")] Comment comment)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId)) { return Unauthorized(); }
            comment.UserId = userId;
            comment.CommentDate = DateTime.UtcNow;
            if (ModelState.IsValid) { try { _context.Comments.Add(comment); await _context.SaveChangesAsync(); /*...*/ TempData["CommentSuccess"] = "..."; } catch (DbUpdateException ex) { /*...*/ TempData["CommentError"] = "..."; } catch (Exception ex) { /*...*/ TempData["CommentError"] = "..."; } }
            else { /*...*/ TempData["CommentError"] = "..."; }
            return RedirectToAction("Details", new { id = comment.BlogPostId });
        }

        private async Task PopulateCategoriesDropDownList(object? selectedCategory = null)
        {
            IEnumerable<Category> categories;
            try { categories = await _categoryRepository.GetAllAsync(); } catch (Exception ex) { /*...*/ categories = Enumerable.Empty<Category>(); ViewBag.CategoryErrorMessage = "Kategoriler yüklenemedi."; }
            ViewBag.Categories = new SelectList(categories.OrderBy(c => c.Name), "Id", "Name", selectedCategory);
        }

        private void LogModelStateErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            if (errors.Any()) { _logger.LogWarning("ModelState Hataları: {Errors}", string.Join(" | ", errors)); }
        }


        private async Task<string> SaveUploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("Yüklenecek dosya boş olamaz.", nameof(file));
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/gif" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var mimeType = file.ContentType.ToLowerInvariant();

            if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension) || !allowedMimeTypes.Contains(mimeType))
            {
                throw new ArgumentException($"Geçersiz dosya türü: {extension} ({mimeType}). İzin verilenler: {string.Join(", ", allowedExtensions)}", nameof(file));
            }

            string relativeFolderPath = Path.Combine("images", "posts");
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, relativeFolderPath);
            Directory.CreateDirectory(uploadsFolder); 

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(relativeFolderPath, uniqueFileName).Replace("\\", "/");
        }

        private void DeleteUploadedFileFromServer(string? serverPath)
        {
            if (string.IsNullOrEmpty(serverPath)) return;

            try
            {
                if (System.IO.File.Exists(serverPath))
                {
                    System.IO.File.Delete(serverPath);
                    _logger.LogInformation("Dosya sunucudan silindi: {FilePath}", serverPath);
                }
                else
                {
                    _logger.LogWarning("Silinmeye çalışılan dosya bulunamadı: {FilePath}", serverPath);
                }
            }
            catch (IOException ioEx)
            {
                _logger.LogError(ioEx, "Dosya silinirken G/Ç hatası oluştu: {FilePath}", serverPath);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Dosya silinirken beklenmedik hata oluştu: {FilePath}", serverPath);
            }
        }

    }
}