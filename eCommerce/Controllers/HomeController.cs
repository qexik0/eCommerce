using eCommerce.Models;
using eCommerce.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eCommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserService _userService;
        private readonly ItemService _itemService;

        public HomeController(ILogger<HomeController> logger, UserService service, ItemService itemService)
        {
            _logger = logger;
            _userService = service;
            _itemService = itemService;
        }

        public async Task<IActionResult> Index(BrowseViewModel viewModel, string CategoryId)
        {
            viewModel.Items = await _itemService.GetAll();
            viewModel.Categories = await _itemService.GetCategories();
            if (CategoryId != null)
            {
                viewModel.Items = viewModel.Items.Where(x => x.CategoryId == CategoryId).ToList();
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string itemId, DetailsViewModel viewModel)
        {
            viewModel.Item = await _itemService.GetById(itemId);
            viewModel.Item.CategoryId = await _itemService.GetCategoryName(viewModel.Item.CategoryId);
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
            {
                var user = await _userService.GetByUsername(User.Identity.Name);
                if (user.UserRatings != null)
                {
                    var q = user.UserRatings.Find(x => x.ItemId == itemId);
                    if (q != null)
                    {
                        viewModel.UserRating = q.Rating;
                    }
                }
            }
            if (viewModel.Item.ItemReviews != null)
            {
                viewModel.ReviewUsernames = new List<string>();
                foreach (var review in viewModel.Item.ItemReviews)
                {
                    viewModel.ReviewUsernames.Add((await _userService.GetById(review.UserId)).Username);
                }
            }
            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated && User.Identity.Name != null)
            {
                var user = await _userService.GetByUsername(User.Identity.Name);
                model.user = user;
                if (user.UserRatings != null && user.UserRatings.Count > 0)
                {
                    model.AvgRating = (double)user.UserRatings.Sum(x => x.Rating) / user.UserRatings.Count;
                }
            }
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> Rate(DetailsViewModel viewModel)
        {
            var username = User.Identity.Name;
            var user = await _userService.GetByUsername(username);
            var userId = user.Id;
            var item = await _itemService.GetById(viewModel.ItemId);
            if (item.ItemRatings != null && item.ItemRatings.Any(x => x.UserId == userId))
            {
                item.ItemRatings.Find(x => x.UserId == userId).Rating = (int)viewModel.Rate;
                user.UserRatings.Find(x => x.ItemId == item.Id).Rating = (int)viewModel.Rate;
                _itemService.UpdateRatings(item);
                _userService.UpdateRatings(user);
            }
            else
            {
                if (item.ItemRatings == null)
                {
                    item.ItemRatings = new();
                }
                item.ItemRatings.Add(new ItemRating { UserId = userId, Rating = (int)viewModel.Rate });
                _itemService.UpdateRatings(item);

                if (user.UserRatings == null)
                {
                    user.UserRatings = new();
                }
                user.UserRatings.Add(new UserRating { ItemId = viewModel.ItemId, Rating = (int)viewModel.Rate });
                _userService.UpdateRatings(user);
            }
            return RedirectToAction("Details", new { itemId = item.Id });
        }

        [Authorize]
        public async Task<IActionResult> Review(DetailsViewModel viewModel)
        {
            var username = User.Identity.Name;
            var user = await _userService.GetByUsername(username);
            var item = await _itemService.GetById(viewModel.ItemId);
            if (item.ItemReviews != null && item.ItemReviews.Any(x => x.UserId == user.Id))
            {
                item.ItemReviews.Find(x => x.UserId == user.Id).ReviewText = viewModel.ReviewText;
                user.UserReviews.Find(x => x.ItemId == item.Id).ReviewText = viewModel.ReviewText;
                _itemService.UpdateReviews(item);
                _userService.UpdateReviews(user);
            }
            else
            {
                if (item.ItemReviews == null)
                {
                    item.ItemReviews = new();
                }
                if (user.UserReviews == null)
                {
                    user.UserReviews = new();
                }
                item.ItemReviews.Add(new() { ReviewText= viewModel.ReviewText, UserId = user.Id });
                user.UserReviews.Add(new() { ItemId = item.Id, ReviewText = viewModel.ReviewText });
                _itemService.UpdateReviews(item);
                _userService.UpdateReviews(user);
            }
            return RedirectToAction("Details", new { itemId = item.Id });
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteItem(string itemId)
        {
            var item = await _itemService.GetById(itemId);
            if (item.ItemRatings != null && item.ItemRatings.Count > 0)
            {
                foreach (var rating in item.ItemRatings)
                {
                    var user = await _userService.GetById(rating.UserId);
                    user.UserRatings.RemoveAll(x => x.ItemId == item.Id);
                    _userService.UpdateRatings(user);
                }
            }
            if (item.ItemReviews != null && item.ItemReviews.Count > 0)
            {
                foreach (var review in item.ItemReviews)
                {
                    var user = await _userService.GetById(review.UserId);
                    user.UserReviews.RemoveAll(x => x.ItemId == item.Id);
                    _userService.UpdateReviews(user);
                }
            }
            _itemService.RemoveItem(item);
            return RedirectToAction("Index");
        }
    }
}