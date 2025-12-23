using APP.Models;
using System.Collections.Generic;

namespace APP.Services
{
    /// <summary>
    /// Interface for managing recently viewed blogs in session
    /// </summary>
    public interface IRecentBlogsService
    {
        /// <summary>
        /// Adds a blog to recently viewed list
        /// </summary>
        void AddRecentBlog(int id, string title);
        
        /// <summary>
        /// Gets list of recently viewed blogs (max 5)
        /// </summary>
        List<RecentlyViewedBlog> GetRecentBlogs();
        
        /// <summary>
        /// Clears all recently viewed blogs
        /// </summary>
        void ClearRecentBlogs();
    }
}