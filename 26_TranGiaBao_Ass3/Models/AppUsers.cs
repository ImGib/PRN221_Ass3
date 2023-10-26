using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _26_TranGiaBao_Ass3.Models
{
    public class AppUsers
    {
        [Key]
        public int UserID { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        [Unique]
        public string Email { get; set; }
        public string Password { get; set; }
        public ICollection<Posts>? Posts { get; set; }
    }
}
