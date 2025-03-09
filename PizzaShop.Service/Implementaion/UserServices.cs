

using Microsoft.EntityFrameworkCore;
using PizzaShop.Domain.DataModels;
using PizzaShop.Domain.ViewModels;
using PizzaShop.Repository.Interface;
using PizzaShop.Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace PizzaShop.Service.Implementation
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;

        public UserServices(IUserRepository userRepository, IEmailSender emailSender)
        {
            _userRepository = userRepository;
            _emailSender = emailSender;
        }


        public async Task<UpdateProfileModel> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return null;

            var model = new UpdateProfileModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                UserName = user.Username,
                Phone = user.Phone,
                CountryID = user.CountryId,
                StateID = user.StateId,
                CityID = user.CityId,
                Address = user.Adress,
                Zipcode = user.Zipcode,
                ProfileImageUrl = user.ProfileImage,

                // Populate dropdowns
                Countries = await _userRepository.GetCountries().ToListAsync(),
                States = await _userRepository.GetStates(user.CountryId).ToListAsync(),
                Cities = await _userRepository.GetCities(user.StateId).ToListAsync()
            };

            return model;
        }

        public async Task<bool> UpdateUserProfileAsync(UpdateProfileModel model, string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            if (user == null) return false;

            // Updating user details
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Username = model.UserName;
            user.Phone = model.Phone;
            user.CountryId = model.CountryID;
            user.StateId = model.StateID;
            user.CityId = model.CityID;
            user.Adress = model.Address;
            user.Zipcode = model.Zipcode;
            user.ProfileImage = model.ProfileImageUrl;


            if (model.ProfileImage != null)
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                Directory.CreateDirectory(folderPath);
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ProfileImage.CopyToAsync(stream);
                }

                user.ProfileImage = "/uploads/" + fileName; // Save relative path in DB
            }


            return await _userRepository.UpdateUserAsync(user);
        }

        // public async Task<bool> UpdateUserProfileAsync(UpdateProfileModel model, string email)
        // {
        //     var user = await _userRepository.GetUserByEmail(email);
        //     if (user == null) return false;

        //     user.FirstName = model.FirstName;
        //     user.LastName = model.LastName;
        //     user.Username = model.UserName;
        //     user.Phone = model.Phone;

        //     if (model.ProfileImage != null)
        //     {
        //         var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        //         Directory.CreateDirectory(folderPath);
        //         string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ProfileImage.FileName);
        //         string filePath = Path.Combine(folderPath, fileName);

        //         using (var stream = new FileStream(filePath, FileMode.Create))
        //         {
        //             await model.ProfileImage.CopyToAsync(stream);
        //         }

        //         user.ProfileImage = "/uploads/" + fileName; // Saving URL
        //     }

        //     return await _userRepository.UpdateUserAsync(user);
        // }


        public async Task<List<StateViewModel>> GetStatesAsync(int countryId)
        {
            var states = await _userRepository.GetStates(countryId).ToListAsync();
            return states.Select(s => new StateViewModel
            {
                StateId = s.StateId,
                StateName = s.StateName
            }).ToList();
        }

        public async Task<List<CityViewModel>> GetCitiesAsync(int stateId)
        {
            var cities = await _userRepository.GetCities(stateId).ToListAsync();
            return cities.Select(c => new CityViewModel
            {
                CityId = c.CityId,
                CityName = c.CityName
            }).ToList();
        }

        public async Task<(List<UserListModel> Users, int TotalCount)> GetUserListAsync(string searchQuery, int pageNumber, int pageSize, string sortBy, string sortOrder)
        {
            Expression<Func<User, bool>> predicate = string.IsNullOrEmpty(searchQuery)
                ? (u => !u.IsDeleted)
                : (u => !u.IsDeleted && (u.FirstName.Contains(searchQuery) || u.LastName.Contains(searchQuery) || u.Email.Contains(searchQuery) || u.Phone.Contains(searchQuery)));

            var users = await _userRepository.GetUsersAsync(predicate, pageNumber, pageSize, sortBy, sortOrder);
            var totalCount = await _userRepository.GetTotalUsersAsync(predicate);

            return (users, totalCount);
        }
        public async Task<bool> SoftDeleteUserAsync(int userId)
        {
            return await _userRepository.SoftDeleteUserAsync(userId);
        }


        public async Task<EditUserModel> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }



        public async Task<List<Country>> GetCountriesAsync()
        {
            return await _userRepository.GetCountriesAsync();
        }



        public async Task<bool> UpdateUserAsync(EditUserModel model)
        {
            var user = await _userRepository.GetUserByIdAsync(model.Id);
            if (user == null) return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Phone = model.Phone;
            user.RoleId = model.RoleId;
            user.Password = model.Password;
            user.CountryID = model.CountryID;
            user.StateID = model.StateID;
            user.CityID = model.CityID;
            user.CityID = model.CityID;
            user.Zipcode = model.Zipcode;
            // user.IsDeleted = model.IsDeleted;

            return await _userRepository.UpdateUserAsync(user);
        }

        public void AddUser(UserModel model)
        {
            var newUser = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Username = model.UserName,
                Email = model.Email,
                Password = model.Password,
                RoleId = model.RoleId,
                CountryId = model.CountryID,
                StateId = model.StateID,
                CityId = model.CityID,
                Phone = model.PhoneNumber,
                Adress = model.Address,
                Zipcode = model.Zipcode
            };

            _userRepository.AddUser(newUser);
        }

        public async Task<bool> SendWelcomeEmailAsync(string email, string password)
        {
            var subject = "Welcome to PizzaShop!";
            var htmlMessage = $"<p>Dear User,</p><p>Your account has been created successfully.</p><p>Email: {email}</p><p>Password: {password}</p><p>Thank you for joining us!</p>";
            return await _emailSender.SendEmailAsync(email, subject, htmlMessage);
        }

        public async Task<User> GetCurrentUserAsync(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

    }
}