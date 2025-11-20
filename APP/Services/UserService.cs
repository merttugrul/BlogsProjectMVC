using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APP.Services
{
    public class UserService : Service<User>, IService<UserRequest, UserResponse>
    {
        public UserService(DbContext db) : base(db)
        {
        }

        public List<UserResponse> List()
        {
            var query = Query()
                .Include(u => u.Group)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
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
                .Include(u => u.Group)
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
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
                RegistrationDate = request.RegistrationDate,
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
                .Include(u => u.UserRoles)
                .SingleOrDefault(u => u.Id == request.Id);
            
            if (entity == null)
                return Error("User not found!");
            
            entity.UserName = request.UserName.Trim();
            entity.Password = request.Password.Trim();
            entity.FirstName = request.FirstName?.Trim();
            entity.LastName = request.LastName?.Trim();
            entity.Gender = request.Gender;
            entity.BirthDate = request.BirthDate;
            entity.RegistrationDate = request.RegistrationDate;
            entity.Score = request.Score;
            entity.IsActive = request.IsActive;
            entity.Address = request.Address?.Trim();
            entity.CountryId = request.CountryId;
            entity.CityId = request.CityId;
            entity.GroupId = request.GroupId;
            
            // Many-to-Many: Eski UserRole kayıtlarını sil, yenilerini ekle
            entity.UserRoles.Clear();
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
                .Include(u => u.UserRoles)
                .SingleOrDefault(u => u.Id == id);
            
            if (entity == null)
                return Error("User not found!");
            
            Delete(entity);
            return Success("User deleted successfully.", entity.Id);
        }

        public UserRequest Edit(int id)
        {
            var entity = Query()
                .Include(u => u.UserRoles)
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
    }
}