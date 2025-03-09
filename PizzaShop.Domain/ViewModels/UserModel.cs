using System.ComponentModel.DataAnnotations;
using PizzaShop.Domain.DataModels;

namespace PizzaShop.Domain.ViewModels;

public class UserModel
{
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [DataType(DataType.Password)]
    [RegularExpression("([a-z]|[A-Z]|[0-9]|[\\W]){4}[a-zA-Z0-9\\W]{3,11}", ErrorMessage = "Password Must contain Special Symbol, Number,Alphabet")]
    public string Password { get; set; }
    public int CountryID { get; set; }
    public int StateID { get; set; }
    public int CityID { get; set; }

    [Required(ErrorMessage = "You must provide a phone number")]
    [Display(Name = "Home Phone")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]

    public string PhoneNumber { get; set; }

    
   public int RoleId { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Zip is Required")]
    // [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid Zip")]

    public IFormFile ImageFile { get; set; }
    public string Zipcode { get; set; }
    public List<Country> Countries { get; set; }
    public List<State> States { get; set; }
    public List<City> Cities { get; set; }
}