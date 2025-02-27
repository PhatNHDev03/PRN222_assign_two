using Azure;
using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.BusinessObject.Pagination;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.SystemAccounts
{
    [BindProperties]
    public class IndexModel : PageModel
    {
        public  List<SystemAccount> List { get; set; }
        public Pager Pager { get; set; }
        private readonly ISystemAccountService _systemAccountService;
        public IndexModel(ISystemAccountService systemAccountService)
        {
            _systemAccountService = systemAccountService;
        }
        public void OnGet(int pg=1)
        {
            int pageSize = 10;
         var (listAll,totalItems) = _systemAccountService.findALlWithPagination(pg,pageSize);
            Pager = new Pager(totalItems,pg, pageSize);
            List = listAll;
        }
    }
}
