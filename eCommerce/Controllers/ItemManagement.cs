using eCommerce.Models;
using eCommerce.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Controllers
{
    [Authorize(Roles = "admin")]
    public class ItemManagement : Controller
    {
        private readonly ItemService _itemService;
        public ItemManagement(ItemService itemService)
        {
            _itemService = itemService;
        }
        public async Task<IActionResult> Index(ItemManagementViewModel viewModel)
        {
            viewModel.Categories = await _itemService.GetCategories();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(ItemManagementViewModel viewModel)
        {
            await _itemService.CreateCategory(viewModel.CategoryName);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(string name)
        {
            var categories = await _itemService.GetCategories();
            var category = categories.Find(x => x.Name == name);
            var items = await _itemService.GetAll();
            if (items.Any(x => x.CategoryId == category.Id))
            {
                TempData["CategoryDeletionError"] = "Not possible to remove a category associated with some items";
            }
            else
            {
                _itemService.RemoveCategory(name);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(ItemManagementViewModel viewModel)
        {
            if (viewModel.ItemName == null)
            {
                TempData["ItemCreationError"] = "Missing item name";
            }
            else if (viewModel.Price == null)
            {
                TempData["ItemCreationError"] = "Missing or wrong price";
            }
            else if (viewModel.Seller == null)
            {
                TempData["ItemCreationError"] = "Missing seller";
            }
            else
            {
                var item = new Item()
                {
                    Name = viewModel.ItemName,
                    CategoryId = viewModel.ItemCategory,
                    Description = viewModel.Description,
                    Price = viewModel.Price,
                    Seller = viewModel.Seller,
                    ImageLink = viewModel.ImageLink,
                    Size = viewModel.Size,
                    Color = viewModel.Color,
                    Spec = viewModel.Spec,
                };
                await _itemService.Create(item);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditItem(string itemId, EditItemViewModel viewModel)
        {
            viewModel.Categories = await _itemService.GetCategories();
            viewModel.Item = await _itemService.GetById(itemId);
            return View("Edit", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditItem(EditItemViewModel viewModel)
        {
            if (viewModel.Item.Name == null)
            {
                TempData["ItemEditError"] = "Missing item name";
                return RedirectToAction("EditItem", new { itemId = viewModel.ItemId });
            }
            else if (viewModel.Item.Price == null)
            {
                TempData["ItemEditError"] = "Missing or wrong price";
                return RedirectToAction("EditItem", new { itemId = viewModel.ItemId });
            }
            else if (viewModel.Item.Seller == null)
            {
                TempData["ItemEditError"] = "Missing seller";
                return RedirectToAction("EditItem", new { itemId = viewModel.ItemId });
            }
            else
            {
                var item = await _itemService.GetById(viewModel.ItemId);
                item.Name = viewModel.Item.Name;
                item.Price = viewModel.Item.Price;
                item.CategoryId = viewModel.Item.CategoryId;
                item.Spec = viewModel.Item.Spec;
                item.ImageLink = viewModel.Item.ImageLink;
                item.Seller = viewModel.Item.Seller;
                item.Color = viewModel.Item.Color;
                item.Size = viewModel.Item.Size;
                await _itemService.UpdateItem(item);
                return RedirectToAction("Details", "Home", new { itemId = item.Id });
            }
        }
    }
}
