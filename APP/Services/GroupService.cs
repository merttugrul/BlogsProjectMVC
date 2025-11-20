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
    public class GroupService : Service<Group>, IService<GroupRequest, GroupResponse>
    {
        public GroupService(DbContext db) : base(db)
        {
        }

        public List<GroupResponse> List()
        {
            var query = Query().Select(g => new GroupResponse
            {
                Id = g.Id,
                Guid = g.Guid,
                Title = g.Title
            });
            return query.ToList();
        }

        public GroupResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity == null)
                return null;
            return new GroupResponse()
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Title = entity.Title
            };
        }

        public CommandResponse Create(GroupRequest request)
        {
            if (Query().Any(g => g.Title.ToLower() == request.Title.ToLower().Trim()))
                return Error("Group with the same title already exists!");
            
            var entity = new Group
            {
                Title = request.Title.Trim()
            };
            Create(entity);
            return Success("Group created successfully.", entity.Id);
        }

        public CommandResponse Update(GroupRequest request)
        {
            if (Query().Any(g => g.Id != request.Id && g.Title.ToLower() == request.Title.ToLower().Trim()))
                return Error("Group with the same title already exists!");
            
            var entity = Query(false).SingleOrDefault(g => g.Id == request.Id);
            if (entity == null)
                return Error("Group not found!");
            
            entity.Title = request.Title.Trim();
            Update(entity);
            return Success("Group updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).Include(g => g.Users).SingleOrDefault(g => g.Id == id);
            if (entity == null)
                return Error("Group not found!");
            
            // İlişkili user'lar varsa silinemez
            if (entity.Users != null && entity.Users.Any())
                return Error("Group cannot be deleted because it has related users!");
            
            Delete(entity);
            return Success("Group deleted successfully.", entity.Id);
        }

        public GroupRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(g => g.Id == id);
            if (entity == null)
                return null;
            return new GroupRequest()
            {
                Id = entity.Id,
                Title = entity.Title
            };
        }
    }
}