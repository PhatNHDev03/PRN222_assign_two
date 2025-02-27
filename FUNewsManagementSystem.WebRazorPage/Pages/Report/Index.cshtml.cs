using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.BusinessObject;
namespace FUNewsManagementSystem.WebRazorPage.Pages.Report
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;
        private readonly INewsArticleService _newsArticleService;
        public DateTime? firstCreateDate { get; set; }
        public IndexModel(ISystemAccountService systemAccountService, INewsArticleService newsArticleService)
        {
            _systemAccountService = systemAccountService;
            _newsArticleService = newsArticleService;
        }

        public List<SystemAccount> AllAccounts { get; set; }
        public short? SelectedId { get; set; }
        public string Notify { get; set; }
        public string UrlApi { get; set; }

        public IActionResult OnGet(short? id)
        {
            AllAccounts = _systemAccountService.findAllSystem();
            SelectedId = id;
            firstCreateDate = _newsArticleService.FirstCreateDate();
            if (id == null)
            {
                Notify = "All";
                UrlApi = "/Report?handler=ChartByEachAuthor";
            }
            else
            {
                Notify = "Personal";
                UrlApi = $"/Report?handler=DataForPersonal&id={id}";
               // TempData["SelectedId"] = id; // Lưu trữ selectedId trong TempData
            }

            return Page();
        }


        public JsonResult OnGetChartByEachAuthor()
        {
            var data = new List<object>();

            var listCountArticleByEachAuthor = _systemAccountService.findAllWithArticles();

            foreach (var item in listCountArticleByEachAuthor.OrderByDescending(x=>(x.NewsArticles.Count)))
            {
                data.Add(new { label = item.AccountEmail, value = item.NewsArticles.Count });
            }

            return new JsonResult(data);
        }
        public JsonResult OnGetChartByEachAuthorWithDate(DateTime? startDate, DateTime? endDate)
        {


            if (startDate == null || endDate == null)
            {
                return new JsonResult(new { error = "Missing start or end date" });
            }

            var data = new List<object>();

            var listCountArticleByEachAuthor = _systemAccountService.FindAllWithArticlesWithDate((DateTime)startDate,(DateTime) endDate);
      
            foreach (var item in listCountArticleByEachAuthor.OrderByDescending(x => (x.NewsArticles.Count)))
            {
                data.Add(new { label = item.AccountEmail, value = item.NewsArticles.Count });
            }

            return new JsonResult(data);
        }

        public JsonResult OnGetDataForPersonal(int? id)
        {
            if (id == null)
            {
                return new JsonResult(new { error = "ID is null" });
            }
            var idShort = short.Parse(id.ToString());
            var data = new List<object>();
            var listCountArticleByEachAuthor = _systemAccountService.findAllWithArticlesById(idShort);
            if (listCountArticleByEachAuthor != null)
            {
                data.Add(new { label = listCountArticleByEachAuthor.AccountEmail, value = listCountArticleByEachAuthor.NewsArticles.Count });
            }
            return new JsonResult(data);
        }
        public JsonResult OnGetDataForPersonalWithDate(int? id, DateTime? startDate, DateTime? endDate)
        {
            if (id == null)
            {
                return new JsonResult(new { error = "ID is null" });
            }
            var idShort = short.Parse(id.ToString());
            var data = new List<object>();
            var listCountArticleByEachAuthor = _systemAccountService.FindAllWithArticlesByIdWithDate(idShort,(DateTime)startDate,(DateTime)endDate);
            if (listCountArticleByEachAuthor != null)
            {
                data.Add(new { label = listCountArticleByEachAuthor.AccountEmail, value = listCountArticleByEachAuthor.NewsArticles.Count });
            }
            return new JsonResult(data);
        }
        public IActionResult OnGetViewAll(short? id)
        {
            firstCreateDate = _newsArticleService.FirstCreateDate();
            if (id == null)
            {
                return RedirectToPage("Index"); // Không có id, về trang mặc định
            }
            return RedirectToPage("Index", new { id }); // Có id, giữ nguyên id
        }



    }
}
