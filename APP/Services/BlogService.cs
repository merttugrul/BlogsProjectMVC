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
    public class BlogService : Service<Blog>, IService<BlogRequest, BlogResponse>
    {
        public BlogService(DbContext db) : base(db)
        {
        }

        public List<BlogResponse> List()
        {
            var query = Query()
                .Include(b => b.User)
                .Include(b => b.BlogTags)
                .ThenInclude(bt => bt.Tag)
                .Select(b => new BlogResponse
                {
                    Id = b.Id,
                    Guid = b.Guid,
                    Title = b.Title,
                    Content = b.Content,
                    Rating = b.Rating,
                    PublishDate = b.PublishDate,
                    UserId = b.UserId,
                    
                    // Formatted fields
                    PublishDateF = b.PublishDate.ToString("MM/dd/yyyy"),
                    RatingF = b.Rating.HasValue ? b.Rating.Value + " / 5" : "No Rating",
                    UserName = b.User != null ? b.User.UserName : "",
                    TagNames = string.Join(", ", b.BlogTags.Select(bt => bt.Tag.Name))
                });
            return query.ToList();
        }

        public BlogResponse Item(int id)
        {
            var entity = Query()
                .Include(b => b.User)
                .Include(b => b.BlogTags)
                .ThenInclude(bt => bt.Tag)
                .SingleOrDefault(b => b.Id == id);
            
            if (entity == null)
                return null;
            
            return new BlogResponse()
            {
                Id = entity.Id,
                Guid = entity.Guid,
                Title = entity.Title,
                Content = entity.Content,
                Rating = entity.Rating,
                PublishDate = entity.PublishDate,
                UserId = entity.UserId,
                
                // Formatted fields
                PublishDateF = entity.PublishDate.ToString("MM/dd/yyyy"),
                RatingF = entity.Rating.HasValue ? entity.Rating.Value + " / 5" : "No Rating",
                UserName = entity.User != null ? entity.User.UserName : "",
                TagNames = string.Join(", ", entity.BlogTags.Select(bt => bt.Tag.Name))
            };
        }

        public CommandResponse Create(BlogRequest request)
        {
            // Title kontrolü
            if (Query().Any(b => b.Title.ToLower() == request.Title.ToLower().Trim()))
                return Error("Blog with the same title already exists!");
            
            var entity = new Blog
            {
                Title = request.Title.Trim(),
                Content = request.Content.Trim(),
                Rating = request.Rating,
                PublishDate = request.PublishDate,
                UserId = request.UserId
            };
            
            // Many-to-Many: BlogTag kayıtlarını oluştur
            if (request.TagIds != null && request.TagIds.Any())
            {
                entity.BlogTags = request.TagIds.Select(tagId => new BlogTag
                {
                    TagId = tagId
                }).ToList();
            }
            
            Create(entity);
            return Success("Blog created successfully.", entity.Id);
        }

        public CommandResponse Update(BlogRequest request)
        {
            // Title kontrolü (kendisi hariç)
            if (Query().Any(b => b.Id != request.Id && b.Title.ToLower() == request.Title.ToLower().Trim()))
                return Error("Blog with the same title already exists!");
            
            var entity = Query(false)
                .Include(b => b.BlogTags)
                .SingleOrDefault(b => b.Id == request.Id);
            
            if (entity == null)
                return Error("Blog not found!");
            
            entity.Title = request.Title.Trim();
            entity.Content = request.Content.Trim();
            entity.Rating = request.Rating;
            entity.PublishDate = request.PublishDate;
            entity.UserId = request.UserId;
            
            // Many-to-Many: Eski BlogTag kayıtlarını sil, yenilerini ekle
            entity.BlogTags.Clear();
            if (request.TagIds != null && request.TagIds.Any())
            {
                entity.BlogTags = request.TagIds.Select(tagId => new BlogTag
                {
                    BlogId = entity.Id,
                    TagId = tagId
                }).ToList();
            }
            
            Update(entity);
            return Success("Blog updated successfully.", entity.Id);
        }

        public CommandResponse Delete(int id)
        {
            var entity = Query(false)
                .Include(b => b.BlogTags)
                .SingleOrDefault(b => b.Id == id);
            
            if (entity == null)
                return Error("Blog not found!");
            
            Delete(entity);
            return Success("Blog deleted successfully.", entity.Id);
        }

        public BlogRequest Edit(int id)
        {
            var entity = Query()
                .Include(b => b.BlogTags)
                .SingleOrDefault(b => b.Id == id);
            
            if (entity == null)
                return null;
            
            return new BlogRequest()
            {
                Id = entity.Id,
                Title = entity.Title,
                Content = entity.Content,
                Rating = entity.Rating,
                PublishDate = entity.PublishDate,
                UserId = entity.UserId,
                TagIds = entity.BlogTags.Select(bt => bt.TagId).ToList()
            };
        }
    }
}