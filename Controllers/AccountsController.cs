using Identity1.Data;
using Identity1.Models;
using Identity1.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity1.Controllers
{
    [Authorize("Admin, SuperAdmin")]
    public class AccountsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountsController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model) {
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                PhoneNumber = model.Phone,
                UserName = model.Email,
                City = model.City,
                Gender = model.Gender,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded) {
                await userManager.AddToRoleAsync(user, "User");
                return RedirectToAction("Login");
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            return View(model);
        }
        public IActionResult GetUsers()
        {
            var users = userManager.Users.ToList();
            List<UsersViewModel> UsersVm = new List<UsersViewModel>();
            foreach (var user in users)
            {
                UsersViewModel userModel = new UsersViewModel() {
                    Id = user.Id,
                    Email = user.Email,
                    Phone = user.PhoneNumber,
                    UserName = user.UserName,
                    City = user.City,
                    Roles = userManager.GetRolesAsync(user).Result,
                };
                UsersVm.Add(userModel);
            }
            return View(UsersVm);
        }
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel model)
        {
            IdentityRole role = new IdentityRole() {
                Name = model.RoleName
            };
            var result = await roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(RolesList));
            }
            return View(model);
        }
        public IActionResult RolesList()
        {
            var roles = roleManager.Roles.ToList();
            var usersViewModel = roles.Select(role=> new RoleViewModel{
                RoleName = role.Name
            }).ToList();
            return View(usersViewModel);
        }
        public IActionResult EditUserRole(string id)
        {
            var viewModel = new EditUserRoleViewModel {
                Id = id,
                RoleList = roleManager.Roles.Select(
                    role => new SelectListItem {
                        Value = role.Id,
                        Text = role.Name
                    }).ToList()
            };
            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserRole(EditUserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            var currentRole = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user,currentRole);
            var role = await roleManager.FindByIdAsync(model.SelectedRoles);
            await userManager.AddToRoleAsync(user, role.Name);
            return RedirectToAction(nameof(GetUsers));
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
