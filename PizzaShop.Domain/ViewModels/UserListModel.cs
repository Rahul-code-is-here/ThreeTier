using System.ComponentModel.DataAnnotations;
using PizzaShop.Domain.DataModels;

namespace PizzaShop.Domain.ViewModels;

public class UserListModel
{
     public int Id { get; set; }

    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Role selection is required")]
    public int RoleId { get; set; }

    public int CountryID { get; set; }
    public int StateID { get; set; }
    public int CityID { get; set; }

    public bool IsDeleted { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [Phone(ErrorMessage = "Invalid Phone Number")]
    public string Phone { get; set; }

    [Required(ErrorMessage = "Zipcode is required")]
    public string Zipcode { get; set; }

     public IFormFile ImageFile { get; set; }
     public List<Country> Countries { get; set; } = new List<Country>(); 
        public List<State> States { get; set; } = new List<State>();
        public List<City> Cities { get; set; } = new List<City>();
}

// public class UserListModel
// {
//     public int Id { get; set; }
//     public string FirstName { get; set; }
//     public string LastName { get; set; }
//     public string Email { get; set; }
//     public string Phone { get; set; }
//     public int RoleId { get; set; }
//     public bool IsDeleted { get; set; }

//     [Required(ErrorMessage = "Password is required")]
//     [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
//     public string Password { get; set; }

//      public string Address { get; set; }

//         public List<Country> Countries { get; set; }
//         public List<State> States { get; set; }
//         public List<City> Cities { get; set; }

// }