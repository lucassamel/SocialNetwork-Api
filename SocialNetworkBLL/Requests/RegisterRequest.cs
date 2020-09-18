using System;
using System.ComponentModel.DataAnnotations;

namespace SocialNetworkBLL.Requests
{
    public class RegisterRequest
    {
        [Required]
        public string Nome { get; set; }
        
        [Required]
        public string Sobrenome { get; set; }
        
        public DateTime Aniversario { get; set; }
        
        public string Localidade { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}