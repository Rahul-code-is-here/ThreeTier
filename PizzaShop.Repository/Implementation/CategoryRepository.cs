using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataContext;
using PizzaShop.Domain.DataModels;
using PizzaShop.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaShop.Repository.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PizzaShemaContext _context;

        public CategoryRepository(PizzaShemaContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
        }

        public async Task<List<Modifiergroup>> GetModifierAsync()
        {
            return await _context.Modifiergroups.Where(m => !m.IsDeleted).ToListAsync();
        }

        public async Task AddCategoryAsync(Category category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            try
            {
                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding category to database.", ex);
            }
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _context.Categories.FindAsync(category.Id);
            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Description = category.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteCategoryAsync(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SoftDeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                _context.Entry(category).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Menuitem>> GetMenuItemsByCategoryAsync(int categoryId)
        {
            return await _context.Menuitems
                .Where(mi => mi.CategoryId == categoryId && !mi.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<string>> GetItemTypesAsync()
        {
            return await _context.Menuitems.Select(mi => mi.ItemType).Distinct().ToListAsync();
        }

        // public async Task<List<string>> GetUnitsAsync()
        // {
        //     return await _context.Units.Select(u => u.Name).ToListAsync();
        // }
        public async Task<List<Unit>> GetUnitsAsync()
        {
            return await _context.Units.Where(u => !u.IsDeleted).ToListAsync();
        }

        public async Task AddAsync(Menuitem menuItem)
        {
            // Fetch the list of valid unit IDs
            var validUnitIds = await _context.Units.Select(u => u.Id).ToListAsync();

            await _context.Menuitems.AddAsync(menuItem);
            await _context.SaveChangesAsync();
        }
        // public async Task<Menuitem> GetMenuItemByIdAsync(int id)
        // {
        //     return await _context.Menuitems.FindAsync(id);
        // }

        // public async Task UpdateMenuItemAsync(Menuitem menuItem)
        // {
        //     _context.Menuitems.Update(menuItem);
        //     await _context.SaveChangesAsync();
        // }

        public async Task UpdateMenuItemAsync(int id, Menuitem menuItem)
        {
            var existingMenuItem = await _context.Menuitems.FindAsync(id);
            if (existingMenuItem != null)
            {
                existingMenuItem.CategoryId = menuItem.CategoryId;
                existingMenuItem.ItemName = menuItem.ItemName;
                existingMenuItem.ItemType = menuItem.ItemType;
                existingMenuItem.Rate = menuItem.Rate;
                existingMenuItem.Quantity = menuItem.Quantity;
                existingMenuItem.Unit = menuItem.Unit;
                existingMenuItem.IsAvailable = menuItem.IsAvailable;
                existingMenuItem.DefaultTax = menuItem.DefaultTax;
                existingMenuItem.TaxPercentage = menuItem.TaxPercentage;
                existingMenuItem.Shortcode = menuItem.Shortcode;
                existingMenuItem.Description = menuItem.Description;
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateMenuItemAsync(MenuItemViewModel model)
        {
            var menuItem = await _context.Menuitems.FindAsync(model.Id);
            if (menuItem != null)
            {
                menuItem.ItemName = model.ItemName;
                menuItem.ItemType = model.ItemType;
                menuItem.Rate = model.Rate;
                menuItem.Quantity = model.Quantity;
                menuItem.IsAvailable = model.IsAvailable;
                menuItem.CategoryId = model.CategoryId;
                menuItem.UnitId = int.Parse(model.Unit);
                menuItem.DefaultTax = model.IsTaxable;
                menuItem.TaxPercentage = (decimal)model.TaxPercentage;
                menuItem.Shortcode = model.ShortCode;
                menuItem.Description = model.Description;

                await _context.SaveChangesAsync();
            }
        }


    }
}