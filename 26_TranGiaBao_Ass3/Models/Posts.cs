using System.ComponentModel.DataAnnotations;

namespace _26_TranGiaBao_Ass3.Models
{
    public class Posts
    {
        [Key]
        public int PostID { get; set; } 
        public int AuthorID { get; set; }
        public DateTime CreatedDate {  get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Title { get; set; }
        public string? Content { get; set; }
        public string? PublishStatus { get; set; }
        public int CategoryID { get; set; }
        public PostCategories? Category { get; set; }
        public AppUsers? User { get; set; }
    }
}
