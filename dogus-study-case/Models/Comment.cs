using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity; 
namespace dogus_study_case.Models
{
    public class Comment
    {
        public int Id { get; set; } 

        [Required(ErrorMessage = "Yorum içeriği boş bırakılamaz.")]
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Yorum en az 3, en fazla 1000 karakter olmalıdır.")]
        [Display(Name = "Yorumunuz")]
        public string Content { get; set; } = string.Empty;

        [DataType(DataType.DateTime)]
        [Display(Name = "Yorum Tarihi")]
        public DateTime CommentDate { get; set; }

        // Foreign Key for BlogPost
        [Required]
        public int BlogPostId { get; set; }

        // [Required]
        public string UserId { get; set; } = string.Empty;

        // --- Navigation Properties ---
        [ForeignKey("BlogPostId")]
        public virtual BlogPost? BlogPost { get; set; } 

        [ForeignKey("UserId")]
        public virtual IdentityUser? User { get; set; } 
    }
}