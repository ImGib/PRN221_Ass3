using System.ComponentModel.DataAnnotations;

namespace _26_TranGiaBao_Ass3.Models
{
    public class PostCategories
    {
        [Key]
        public int CategoryID {  get; set; }
        public string CategoryName {  get; set; }
        public string? Description { get; set; }
        public ICollection<Posts>? Posts { get; set; }
    }
}
