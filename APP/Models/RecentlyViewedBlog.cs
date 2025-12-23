namespace APP.Models
{
    /// <summary>
    /// Model for storing recently viewed blog information in session
    /// </summary>
    public class RecentlyViewedBlog
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ViewedAt { get; set; }
    }
}