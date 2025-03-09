using PizzaShop.Domain.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task AddCategoryAsync(Category category);

        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int categoryId);
        Task SoftDeleteCategoryAsync(int id);

        Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId);

        Task<List<string>> GetItemTypesAsync();
        Task<List<Unit>> GetUnitsAsync();

        Task AddAsync(Menuitem menuItem);

        // Task<Menuitem> GetMenuItemByIdAsync(int id);
        // Task UpdateMenuItemAsync(Menuitem menuItem);

        Task UpdateMenuItemAsync(int id, Menuitem menuItem);

        Task UpdateMenuItemAsync(MenuItemViewModel model);

    }
}