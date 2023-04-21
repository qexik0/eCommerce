using eCommerce.Models;
using eCommerce.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserManagementController : Controller
    {
        private readonly UserService _userService;
        private readonly ItemService _itemService;

        public UserManagementController(UserService userService, ItemService itemService)
        {
            _userService = userService;
            _itemService = itemService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(UserManagementViewModel viewModel)
        {
            viewModel.Users = await _userService.GetAll();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(UserManagementViewModel viewModel)
        {
            await _userService.Create(viewModel.Username, viewModel.Password, viewModel.IsAdmin);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccount(string username)
        {
            var user = await _userService.GetByUsername(username);
            if (user.UserRatings != null && user.UserRatings.Count > 0)
            {
                foreach (var rating in user.UserRatings)
                {
                    var item = await _itemService.GetById(rating.ItemId);
                    item.ItemRatings.RemoveAll(x => x.UserId == user.Id);
                    _itemService.UpdateRatings(item);
                }
            }
            if (user.UserReviews != null && user.UserReviews.Count > 0)
            {
                foreach (var review in user.UserReviews)
                {
                    var item = await _itemService.GetById(review.ItemId);
                    item.ItemReviews.RemoveAll(x => x.UserId == user.Id);
                    _itemService.UpdateReviews(item);
                }
            }
            _userService.DeleteByUsername(username);
            if (username == User.Identity.Name)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Index");
        }
    }
}
