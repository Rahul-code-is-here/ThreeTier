// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using PizzaShop.Domain.DataModels;
// using PizzaShop.Domain.ViewModels;
// using PizzaShop.Repository.Interfaces;
// using PizzaShop.Service.Interface;
// using PizzaShop.Services.Interfaces;
// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Threading.Tasks;

// namespace PizzaShop.Web.Controllers
// {


//     public class MenuController : Controller
//     {
//         private readonly ICategoryService _categoryService;

//         private readonly IUserServices _userServices;

//         public MenuController(ICategoryService categoryService, IUserServices userServices)
//         {
//             _categoryService = categoryService;
//             _userServices = userServices;
//         }


//         public async Task<IActionResult> Menu()
//         {
//             var categories = await _categoryService.GetCategoriesAsync();
//             return View(categories);
//         }



//         [HttpPost]
//         public async Task<IActionResult> AddCategory([FromBody] Category category)
//         {
//             try
//             {
//                 if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
//                     return BadRequest(new { message = "Invalid category data" });

//                 await _categoryService.AddCategoryAsync(category);

//                 return Json(new { message = "Category added successfully", name = category.CategoryName });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { message = "Error adding category", details = ex.Message });
//             }
//         }

//         [HttpPut]
//         public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
//         {
//             if (category == null || id != category.Id)
//                 return BadRequest(new { message = "Invalid category data" });

//             try
//             {
//                 await _categoryService.UpdateCategoryAsync(category);
//                 return Ok(new { message = "Category updated successfully" });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { message = "Error updating category", details = ex.Message });
//             }
//         }

//         [HttpDelete]
//         public async Task<IActionResult> DeleteCategory(int id)
//         {
//             try
//             {
//                 await _categoryService.DeleteCategoryAsync(id);
//                 return Ok(new { message = "Category deleted successfully" });
//             }
//             catch (Exception ex)
//             {
//                 return StatusCode(500, new { message = "Error deleting category", details = ex.Message });
//             }
//         }

//         [HttpPut]
//         public async Task<IActionResult> SoftDeleteCategory(int id)
//         {
//             await _categoryService.SoftDeleteCategoryAsync(id);
//             return NoContent();
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetMenuItemsByCategory(int categoryId)
//         {
//             var menuItems = await _categoryService.GetMenuItemsByCategoryAsync(categoryId);
//             return Json(menuItems);
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetDropdownData()
//         {
//             var categories = await _categoryService.GetCategoriesAsync();
//             var itemTypes = await _categoryService.GetItemTypesAsync();
//             var units = await _categoryService.GetUnitsAsync();

//             return Json(new { categories, itemTypes, units });
//         }

//         [HttpPost]
//         public async Task<IActionResult> AddMenuItem([FromForm] MenuItemViewModel model)
//         {
//             // if (!ModelState.IsValid)
//             // {
//             //     return BadRequest(ModelState);
//             // }


//             var result = await _categoryService.AddMenuItemAsync(model);

//             if (result != null)
//             {
//                 return Ok(new { message = "Menu item added successfully!" });
//             }
//             else
//             {
//                 return StatusCode(500, "Internal server error");
//             }
//         }



//         [HttpPut]
//         public async Task<IActionResult> UpdateMenuItem([FromForm] MenuItemViewModel model)
//         {
//             if (model == null)
//             {
//                 return BadRequest("Menu item is null.");
//             }

//             await _categoryService.UpdateMenuItemAsync(model);
//             return Ok(new { message = "Menu item updated successfully" });
//         }

//         private string ExtractEmailFromToken(string token)
//         {
//             var handler = new JwtSecurityTokenHandler();
//             var jwtToken = handler.ReadJwtToken(token);
//             var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

//             return emailClaim?.Value;
//         }

//     }
// }

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Service.Interface;
using PizzaShop.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PizzaShop.Web.Controllers
{
    public class MenuController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUserServices _userServices;

        public MenuController(ICategoryService categoryService, IUserServices userServices)
        {
            _categoryService = categoryService;
            _userServices = userServices;
        }

        private async Task SetUserProfileInViewBag()
        {
            var token = Request.Cookies["AuthToken"];
            var email = ExtractEmailFromToken(token);

            if (!string.IsNullOrEmpty(email))
            {
                var userProfile = await _userServices.GetUserProfileAsync(email);
                if (userProfile != null)
                {
                    ViewBag.UserName = userProfile.UserName;
                    ViewBag.UserImage = userProfile.ProfileImageUrl;
                }
            }
        }

        public async Task<IActionResult> Menu()
        {
            await SetUserProfileInViewBag();

            await SetUserProfileInViewBag();

            var model = new MenuViewModel
            {
                Categories = await _categoryService.GetCategoriesAsync(),
                ModifierGroups = await _categoryService.GetModifierAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] Category category)
        {
            try
            {
                if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
                    return BadRequest(new { message = "Invalid category data" });

                await _categoryService.AddCategoryAsync(category);

                return Json(new { message = "Category added successfully", name = category.CategoryName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding category", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category category)
        {
            if (category == null || id != category.Id)
                return BadRequest(new { message = "Invalid category data" });

            try
            {
                await _categoryService.UpdateCategoryAsync(category);
                return Ok(new { message = "Category updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating category", details = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new { message = "Category deleted successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting category", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> SoftDeleteCategory(int id)
        {
            await _categoryService.SoftDeleteCategoryAsync(id);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuItemsByCategory(int categoryId)
        {
            var menuItems = await _categoryService.GetMenuItemsByCategoryAsync(categoryId);
            return Json(menuItems);
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownData()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            var itemTypes = await _categoryService.GetItemTypesAsync();
            var units = await _categoryService.GetUnitsAsync();

            return Json(new { categories, itemTypes, units });
        }

        [HttpPost]
        public async Task<IActionResult> AddMenuItem([FromForm] MenuItemViewModel model)
        {
            var result = await _categoryService.AddMenuItemAsync(model);

            if (result != null)
            {
                return Ok(new { message = "Menu item added successfully!" });
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMenuItem([FromForm] MenuItemViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Menu item is null.");
            }

            await _categoryService.UpdateMenuItemAsync(model);
            return Ok(new { message = "Menu item updated successfully" });
        }

        private string ExtractEmailFromToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);

            return emailClaim?.Value;
        }
    }
}