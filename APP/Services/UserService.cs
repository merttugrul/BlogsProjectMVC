using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using CORE.APP.Services.Authentication.MVC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        private readonly ICookieAuthService _cookieAuthService;

        public UserService(DbContext db, ICookieAuthService cookieAuthService) : base(db)
        {
            _cookieAuthService = cookieAuthService;
        }

        protected override IQueryable<User> Query(bool isNoTracking = true)
        {
            return base.Query(isNoTracking).Include(u => u.Group)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role);

        }

        public List<UserResponse> List()
        {
            var query = Query()
                
                .Select(u => new UserResponse
                {
                    Id = u.Id,
                    Guid = u.Guid,
                    UserName = u.UserName,
                    Password = u.Password,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Gender = u.Gender,
                    BirthDate = u.BirthDate,
                    RegistrationDate = u.RegistrationDate,
                    Score = u.Score,
                    IsActive = u.IsActive,
                    Address = u.Address,
                    CountryId = u.CountryId,
                    CityId = u.CityId,
                    GroupId = u.GroupId,
                    
                    // Formatted fields
                    FullName = (u.FirstName ?? "") + " " + (u.LastName ?? ""),
                    BirthDateF = u.BirthDate.HasValue ? u.BirthDate.Value.ToString("MM/dd/yyyy") : "",
                    GenderF = u.Gender.ToString(),
                    RegistrationDateF = u.RegistrationDate.ToString("MM/dd/yyyy"),
                    ScoreF = u.Score.ToString("N1"),
                    IsActiveF = u.IsActive ? "Active" : "Inactive",
                    GroupTitle = u.Group != null ? u.Group.Title : "",
                    RoleNames = string.Join(", ", u.UserRoles.Select(ur => ur.Role.Name))
                });
            return query.ToList();
        }

        public UserResponse Item(int id)
        {
            var entity = Query()
                
                .SingleOrDefault(u => u.Id == id);
            
            if (entity == null)
                return null;
            
            return new UserResponse()
            {
                Id = entity.Id,
                Guid = entity.Guid,
                UserName = entity.UserName,
                Password = entity.Password,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                RegistrationDate = entity.RegistrationDate,
                Score = entity.Score,
                IsActive = entity.IsActive,
                Address = entity.Address,
                CountryId = entity.CountryId,
                CityId = entity.CityId,
                GroupId = entity.GroupId,
                
                // Formatted fields
                FullName = (entity.FirstName ?? "") + " " + (entity.LastName ?? ""),
                BirthDateF = entity.BirthDate.HasValue ? entity.BirthDate.Value.ToString("MM/dd/yyyy") : "",
                GenderF = entity.Gender.ToString(),
                RegistrationDateF = entity.RegistrationDate.ToString("MM/dd/yyyy"),
                ScoreF = entity.Score.ToString("N1"),
                IsActiveF = entity.IsActive ? "Active" : "Inactive",
                GroupTitle = entity.Group != null ? entity.Group.Title : "",
                RoleNames = string.Join(", ", entity.UserRoles.Select(ur => ur.Role.Name))
            };
        }

        public CommandResponse Create(UserRequest request)
        {
            if (Query().Any(u => u.UserName.ToLower() == request.UserName.ToLower().Trim()))
                return Error("User with the same username already exists!");
            
            var entity = new User
            {
                UserName = request.UserName.Trim(),
                Password = request.Password.Trim(),
                FirstName = request.FirstName?.Trim(),
                LastName = request.LastName?.Trim(),
                Gender = request.Gender,
                BirthDate = request.BirthDate,
                RegistrationDate = DateTime.Now,
                Score = request.Score,
                IsActive = request.IsActive,
                Address = request.Address?.Trim(),
                CountryId = request.CountryId,
                CityId = request.CityId,
                GroupId = request.GroupId
            };
            
            // Many-to-Many: UserRole kayıtlarını oluştur
            if (request.RoleIds != null && request.RoleIds.Any())
            {
                entity.UserRoles = request.RoleIds.Select(roleId => new UserRole
                {
                    RoleId = roleId
                }).ToList();
            }
            
            Create(entity);
            return Success("User created successfully.", entity.Id);
        }

        public CommandResponse Update(UserRequest request)
        {
            if (Query().Any(u => u.Id != request.Id && u.UserName.ToLower() == request.UserName.ToLower().Trim()))
                return Error("User with the same username already exists!");
            
            var entity = Query(false)
                
                .SingleOrDefault(u => u.Id == request.Id);
            
            if (entity == null)
                return Error("User not found!");
            
            entity.UserName = request.UserName.Trim();
            entity.Password = request.Password.Trim();
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            
            entity.Score = request.Score;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();
            entity.CountryId = request.CountryId;
            entity.CityId = request.CityId;
            entity.GroupId = request.GroupId;
            
            // Many-to-Many: Eski UserRole kayıtlarını sil, yenilerini ekle
            Delete(entity.UserRoles);
            if (request.RoleIds != null && request.RoleIds.Any())
            {
                entity.UserRoles = request.RoleIds.Select(roleId => new UserRole
                {
                    UserId = entity.Id,
                    RoleId = roleId
                }).ToList();
            }
            
            Update(entity);
            return Success("User updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                
                .SingleOrDefault(u => u.Id == id);
            
            if (entity == null)
                return Error("User not found!");
            Delete(entity.UserRoles);
            Delete(entity);
            return Success("User deleted successfully.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = Query()
                
                .SingleOrDefault(u => u.Id == id);
            
            if (entity == null)
                return null;
            
            return new UserRequest()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Password = entity.Password,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Gender = entity.Gender,
                BirthDate = entity.BirthDate,
                RegistrationDate = entity.RegistrationDate,
                Score = entity.Score,
                IsActive = entity.IsActive,
                Address = entity.Address,
                CountryId = entity.CountryId,
                CityId = entity.CityId,
                GroupId = entity.GroupId,
                RoleIds = entity.UserRoles.Select(ur => ur.RoleId).ToList()
            };
        }

        // Authentication:
        /// <summary>
        /// Authenticates a user using the provided login credentials and initiates a cookie-based sign-in.
        /// </summary>
        public async Task<CommandResponse> Login(UserLoginRequest request)
        {
            var entity = Query().SingleOrDefault(
                u => u.UserName == request.UserName
                  && u.Password == request.Password
                  && u.IsActive);

            if (entity is null)
                return Error("Invalid user name or password!");

            await _cookieAuthService.SignIn(
                entity.Id,
                entity.UserName,
                entity.UserRoles.Select(ur => ur.Role.Name).ToArray());

            return Success("User logged in successfully.", entity.Id);
        }

        /// <summary>
        /// Signs out the currently authenticated user by removing the authentication cookie.
        /// </summary>
        public async Task Logout()
        {
            await _cookieAuthService.SignOut();
        }

        /// <summary>
        /// Registers a new user with the default "User" role and "Active" status.
        /// </summary>
        public CommandResponse Register(UserRegisterRequest request)
        {
            var roleEntity = Query<Role>().SingleOrDefault(r => r.Name == "User");
            if (roleEntity is null)
                return Error("\"User\" role not found!");

            return Create(new UserRequest
            {
                UserName = request.UserName,
                Password = request.Password,
                IsActive = true,
                RoleIds = new List<int> { roleEntity.Id }
            });
        }
    }
}