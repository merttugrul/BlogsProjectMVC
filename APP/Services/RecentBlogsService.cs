using APP.Models;
using CORE.APP.Services.Session.MVC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace APP.Services
{
    /// <summary>
    /// Service for managing recently viewed blogs using session storage
    /// </summary>
    public class RecentBlogsService : IRecentBlogsService
    {
        private readonly SessionServiceBase _sessionService;
        private const string SessionKey = "RecentlyViewedBlogs";
        private const int MaxRecentBlogs = 5;

        public RecentBlogsService(SessionServiceBase sessionService)
        {
            _sessionService = sessionService;
        }

        public void AddRecentBlog(int id, string title)
        {
            var recentBlogs = GetRecentBlogs();
            
            // Remove if already exists
            recentBlogs.RemoveAll(b => b.Id == id);
            
            // Add to beginning
            recentBlogs.Insert(0, new RecentlyViewedBlog
            {
                Id = id,
                Title = title,
                ViewedAt = DateTime.Now.ToString("MM/dd/yyyy HH:mm")
            });
            
            // Keep only last 5
            if (recentBlogs.Count > MaxRecentBlogs)
            {
                recentBlogs = recentBlogs.Take(MaxRecentBlogs).ToList();
            }
            
            _sessionService.SetSession(SessionKey, recentBlogs);
        }

        public List<RecentlyViewedBlog> GetRecentBlogs()
        {
            return _sessionService.GetSession<List<RecentlyViewedBlog>>(SessionKey) 
                   ?? new List<RecentlyViewedBlog>();
        }

        public void ClearRecentBlogs()
        {
            _sessionService.RemoveSession(SessionKey);
        }
    }
}