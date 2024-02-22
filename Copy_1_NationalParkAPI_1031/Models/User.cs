using System.ComponentModel.DataAnnotations.Schema;

namespace Copy_1_NationalParkAPI_1031.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        [NotMapped]
        public string Token { get; set; }   
    }
}
