namespace PizzaShop.Domain.ViewModels;

public class MenuViewModel
{
    public List<PizzaShop.Domain.DataModels.Category> Categories { get; set; }
    public List<PizzaShop.Domain.DataModels.Modifiergroup> ModifierGroups { get; set; }
}