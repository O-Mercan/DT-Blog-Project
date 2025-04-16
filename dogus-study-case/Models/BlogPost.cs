using System;
using System.Collections.Generic; 
using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;  
using Microsoft.AspNetCore.Identity;
using dogus_study_case.Models; 

namespace dogus_study_case.Models
{
    public class BlogPost
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Başlık en az 5, en fazla 200 karakter olmalıdır.")]
        [Display(Name = "Başlık")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "İçerik alanı zorunludur.")]
        [Display(Name = "İçerik")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; } = string.Empty;

        [Display(Name = "Yayınlanma Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime PublicationDate { get; set; }

        [Display(Name = "Görsel URL (Opsiyonel)")]
        [StringLength(500, ErrorMessage = "Görsel URL'si en fazla 500 karakter olabilir.")]
        [DataType(DataType.ImageUrl)]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Lütfen bir kategori seçin.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori seçmelisiniz.")]
        [Display(Name = "Kategori")]
        public int CategoryId { get; set; }

        // [Required] 
        public string UserId { get; set; } = string.Empty;

        // --- Navigation Properties ---
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }

        public BlogPost()
        {
            Comments = new HashSet<Comment>(); 
        }
    }
}