using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.BusinessObject.Pagination;
using FUNewsManagementSystem.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FUNewsManagementSystem.WebRazorPage.Pages.Tags
{
    public class IndexModel : PageModel
    {
        public List<Tag> Tags { get; set; }
        public Pager Pager { get; set; }

        private readonly ITagService _tagService;


        public IndexModel(ITagService tagService)
        {
            _tagService = tagService;

        }

        public void OnGet(int pg=1)
        {
            int pageSize = 5;
            var (TagsAll, TotalItem) = _tagService.findALlWithPagination(pg, pageSize);
            Pager = new Pager(TotalItem,pg,pageSize);
            Tags = TagsAll;
        }
    }
}
