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
    public class TagService : Service<Tag>, IService<TagRequest, TagResponse>
    {
        public TagService(DbContext db) : base(db)
        {
        }

        public List<TagResponse> List()
        {
            var query = Query().Select(t => new TagResponse
            {
                Id = t.Id,
                Guid = t.Guid,
                Name = t.Name
            });
            return query.ToList();
        }

        public TagResponse Item(int id)
        {
            var entity = Query().SingleOrDefault(t => t.Id == id);
            if (entity == null)
                return null;
            return new TagResponse()
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Name = entity.Name
            };
        }

        public CommandResponse Create(TagRequest request)
        {
            if (Query().Any(t => t.Name.ToLower() == request.Name.ToLower().Trim()))
                return Error("Tag with the same name already exists!");
            
            var entity = new Tag
            {
                Name = request.Name.Trim()
            };
            Create(entity);
            return Success("Tag created successfully.", entity.Id);
        }

        public CommandResponse Update(TagRequest request)
        {
            if (Query().Any(t => t.Id != request.Id && t.Name.ToLower() == request.Name.ToLower().Trim()))
                return Error("Tag with the same name already exists!");
            
            var entity = Query(false).SingleOrDefault(t => t.Id == request.Id);
            if (entity == null)
                return Error("Tag not found!");
            
            entity.Name = request.Name.Trim();
            Update(entity);
            return Success("Tag updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false).Include(t => t.BlogTags).SingleOrDefault(t => t.Id == id);
            if (entity == null)
                return Error("Tag not found!");
            
            // İlişkili blog'lar varsa silinemez
            if (entity.BlogTags != null && entity.BlogTags.Any())
                return Error("Tag cannot be deleted because it has related blogs!");
            
            Delete(entity);
            return Success("Tag deleted successfully.", entity.Id);
        }

        public TagRequest Edit(int id)
        {
            var entity = Query().SingleOrDefault(t => t.Id == id);
            if (entity == null)
                return null;
            return new TagRequest()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }
    }
}