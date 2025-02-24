using FUNewsManagementSystem.Services;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FUNewsManagementSystem.BusinessObject;
namespace FUNewsManagementSystem.WebRazorPage.Pages.Report
{
    public class IndexModel : PageModel
    {
        private readonly ISystemAccountService _systemAccountService;

        public IndexModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }

        public List<SystemAccount> AllAccounts { get; set; }
        public short? SelectedId { get; set; }
        public string Notify { get; set; }
        public string UrlApi { get; set; }

        public IActionResult OnGet(short? id)
        {
            AllAccounts = _systemAccountService.findAllSystem();
            SelectedId = id;

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

            foreach (var item in listCountArticleByEachAuthor)
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

        public IActionResult OnGetViewAll(short? id)
        {
            if (id == null)
            {
                return RedirectToPage("Index"); // Không có id, về trang mặc định
            }
            return RedirectToPage("Index", new { id }); // Có id, giữ nguyên id
        }



    }
}
