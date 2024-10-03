using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity1.Models.ViewModel
{
    public class EditUserRoleViewModel
    {
        public string Id { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
        public string? SelectedRoles { get; set; }
    }
}
