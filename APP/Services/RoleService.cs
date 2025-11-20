using APP.Domain;
using APP.Models;
using CORE.APP.Models;
using CORE.APP.Services;
using CORE.APP.Services.MVC;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace APP.Services
{
    public class RoleService : Service<Role>, IService<RoleRequest, RoleResponse>
    {
        public RoleService(DbContext db) : base(db)
        {
        }

        public List<RoleResponse> List()
        {
            var query = Query().Select(r => new RoleResponse
            {
                Id = r.Id,
                Guid = r.Guid,
                Name = r.Name
            });
            return query.ToList();
        }

        public RoleResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity == null)
                return null;
            return new RoleResponse()
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name
            };
        }

        public CommandResponse Create(RoleRequest request)
        {
            if (Query().Any(r => r.Name.ToLower() == request.Name.ToLower().Trim()))
                return Error("Role with the same name already exists!");
            
            var entity = new Role
            {
                Name = request.Name.Trim()
            };
            Create(entity);
            return Success("Role created successfully.", entity.Id);
        }

        public CommandResponse Update(RoleRequest request)
        {
            if (Query().Any(r => r.Id != request.Id && r.Name.ToLower() == request.Name.ToLower().Trim()))
                return Error("Role with the same name already exists!");
            
            var entity = Query(false).SingleOrDefault(r => r.Id == request.Id);
            if (entity == null)
                return Error("Role not found!");
            
            entity.Name = request.Name.Trim();
            Update(entity);
            return Success("Role updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).Include(r => r.UserRoles).SingleOrDefault(r => r.Id == id);
            if (entity == null)
                return Error("Role not found!");
            
            // İlişkili user'lar varsa silinemez
            if (entity.UserRoles != null && entity.UserRoles.Any())
                return Error("Role cannot be deleted because it has related users!");
            
            Delete(entity);
            return Success("Role deleted successfully.", entity.Id);
        }

        public RoleRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(r => r.Id == id);
            if (entity == null)
                return null;
            return new RoleRequest()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}