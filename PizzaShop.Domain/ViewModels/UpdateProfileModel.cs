using PizzaShop.Domain.DataModels;

namespace PizzaShop.Domain.ViewModels;

    public class UpdateProfileModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int CountryID { get; set; }  
        public int StateID { get; set; }  
        public int CityID { get; set; } 
        public string Address { get; set; }
        public string Zipcode { get; set; }

    // public IFormFile ProfileImageUrl { get; set; }
    public IFormFile ProfileImage { get; set; } // This will be used for file upload
    public string ProfileImageUrl { get; set; } // Stores image URL



    // Dropdown lists
    public List<Country> Countries { get; set; } = new List<Country>(); 
        public List<State> States { get; set; } = new List<State>();
        public List<City> Cities { get; set; } = new List<City>();
    }
