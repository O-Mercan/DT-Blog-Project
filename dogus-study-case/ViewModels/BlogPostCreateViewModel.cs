using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http; 

namespace dogus_study_case.ViewModels 
{
    public class BlogPostCreateViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık en az 5, en fazla 200 karakter olmalıdır.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [Display(Name = "İçerik")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Lütfen bir kategori seçin.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçmelisiniz.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        [Display(Name = "Görsel (Opsiyonel)")]
        public IFormFile? ImageFile { get; set; } 
    }
}