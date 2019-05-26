using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using NorthwindSystem.Areas.Identity.Data;
using System.Threading.Tasks;

namespace NorthwindSystem.Controllers
{
    [Authorize(Roles = Role.Admin)]
    [Route("admin")]
    public class AdministratorController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AdministratorController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();

            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
}