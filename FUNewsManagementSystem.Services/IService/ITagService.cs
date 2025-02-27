using FUNewsManagementSystem.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services.IService
{
    public interface ITagService
    {
        List<Tag> GetAllTags();
        Tag GetTagById(int id);
        void AddTag(Tag tag);
        void UpdateTag(Tag tag);
        void DeleteTag(int id);
        int getLastId();
        List<Tag> GetTagsByIds(List<int> tagIds);
        (List<Tag>, int totalItems) findALlWithPagination(int pg, int pageSize);


    }
}
