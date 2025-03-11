using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<List<Modifiergroup>> GetModifierAsync();

        Task AddCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);

        Task SoftDeleteCategoryAsync(int id);

        Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId);

        Task<List<string>> GetItemTypesAsync();
        Task<List<Unit>> GetUnitsAsync();

        Task<Menuitem> AddMenuItemAsync(MenuItemViewModel model);

        // Task<MenuItemViewModel> GetMenuItemByIdAsync(int id);
        // Task UpdateMenuItemAsync(MenuItemViewModel model);

        Task UpdateMenuItemAsync(int id, Menuitem menuItem);

        Task UpdateMenuItemAsync(MenuItemViewModel model);
    }
}