namespace FUNewsManagementSystem.WebRazorPage.ViewModels
{
    public class NewsArticleViewModel
    {
        public string NewsArticleID { get; set; }
        public string NewsTitle { get; set; }
        public string Headline { get; set; }
        public string NewsContent { get; set; }
        public string NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public bool NewsStatus { get; set; }

        public List<int> SelectedTags { get; set; } = new List<int>(); // Danh sách TagId được chọn
    }
}
