using System.ComponentModel.DataAnnotations;

namespace Identity1.Models.ViewModel
{
    public class RoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
