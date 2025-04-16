using System.Collections.Generic;    
using System.ComponentModel.DataAnnotations; 

namespace dogus_study_case.Models
{
    public class Category
    {
        public int Id { get; set; }  
        [Required(ErrorMessage = "Kategori adı boş bırakılamaz.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        public string Name { get; set; }

        // Navigation Property: 
        public virtual ICollection<BlogPost> BlogPosts { get; set; } = new List<BlogPost>();
    }
}
