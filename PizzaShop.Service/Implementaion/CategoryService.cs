using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interfaces;
using PizzaShop.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PizzaShop.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;



        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

        }

        public async Task<List<Modifiergroup>> GetModifierAsync()
        {
            return await _categoryRepository.GetModifierAsync();
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (category == null || string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Category data is invalid.");

            await _categoryRepository.AddCategoryAsync(category);
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await _categoryRepository.UpdateCategoryAsync(category);
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            await _categoryRepository.DeleteCategoryAsync(categoryId);
        }

        public async Task SoftDeleteCategoryAsync(int id)
        {
            await _categoryRepository.SoftDeleteCategoryAsync(id);
        }

        public async Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId)
        {
            return await _categoryRepository.GetMenuItemsByCategoryAsync(categoryId);
        }

        public async Task<List<string>> GetItemTypesAsync()
        {
            return await _categoryRepository.GetItemTypesAsync();
        }

        public async Task<List<Unit>> GetUnitsAsync()
        {
            return await _categoryRepository.GetUnitsAsync();
        }

        public async Task<Menuitem> AddMenuItemAsync(MenuItemViewModel model)
        {
            var menuItem = new Menuitem
            {
                CategoryId = model.CategoryId,
                ItemName = model.ItemName,
                ItemType = model.ItemType,
                Rate = model.Rate,
                Quantity = model.Quantity,
                UnitId = int.Parse(model.Unit), // Ensure this is set correctly but it's not now
                IsAvailable = model.IsAvailable,
                DefaultTax = model.IsTaxable,
                TaxPercentage = (decimal)model.TaxPercentage,
                Shortcode = model.ShortCode,
                Description = model.Description,
                // ImagePath = model.Image != null ? model.Image.ToString() : null // Handle file upload 
            };

            await _categoryRepository.AddAsync(menuItem);
            return menuItem;
        }
        //   public async Task<MenuItemViewModel> GetMenuItemByIdAsync(int id)
        // {
        //     var menuItem = await _categoryRepository.GetMenuItemByIdAsync(id);
        //     return new MenuItemViewModel
        //     {
        //         Id = menuItem.Id,
        //         CategoryId = menuItem.CategoryId,
        //         ItemName = menuItem.ItemName,
        //         ItemType = menuItem.ItemType,
        //         Rate = menuItem.Rate,
        //         Quantity = menuItem.Quantity,
        //         Unit = menuItem.Unit.ToString(),
        //         IsAvailable = (bool)menuItem.IsAvailable,
        //         IsTaxable = menuItem.DefaultTax,
        //         TaxPercentage = menuItem.TaxPercentage,
        //         ShortCode = menuItem.Shortcode,
        //         Description = menuItem.Description
        //     };
        // }

        // public async Task UpdateMenuItemAsync(MenuItemViewModel model)
        // {
        //     var menuItem = await _categoryRepository.GetMenuItemByIdAsync(model.Id);
        //     if (menuItem != null)
        //     {
        //         menuItem.CategoryId = model.CategoryId;
        //         menuItem.ItemName = model.ItemName;
        //         menuItem.ItemType = model.ItemType;
        //         menuItem.Rate = model.Rate;
        //         menuItem.Quantity = model.Quantity;
        //         menuItem.UnitId = int.Parse(model.Unit);
        //         menuItem.IsAvailable = model.IsAvailable;
        //         menuItem.DefaultTax = model.IsTaxable;
        //         menuItem.TaxPercentage = (decimal)model.TaxPercentage;
        //         menuItem.Shortcode = model.ShortCode;
        //         menuItem.Description = model.Description;
        //         menuItem.ImagePath = model.Image != null ? model.Image.FileName : menuItem.ImagePath; // Handle file upload

        //         await _categoryRepository.UpdateMenuItemAsync(menuItem);
        //     }
        // }

        public async Task UpdateMenuItemAsync(int id, Menuitem menuItem)
        {
            await _categoryRepository.UpdateMenuItemAsync(id, menuItem);
        }

        public async Task UpdateMenuItemAsync(MenuItemViewModel model)
        {
            await _categoryRepository.UpdateMenuItemAsync(model);
        }
    }
}