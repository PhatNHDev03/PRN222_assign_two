using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.BusinessObject.Pagination;
using FUNewsManagementSystem.DataAccess.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.DataAccess
{
    public class TagRepository : ITagRepository
    {
        private readonly FUNewsManagementContext _context;

        public TagRepository(FUNewsManagementContext context)
        {
            _context = context;
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.OrderByDescending(o => o.TagId).ToList();
        }

        public Tag GetTagById(int id)
        {
            return _context.Tags.Find(id);
        }

        public void AddTag(Tag tag)
        {
            _context.Tags.Add(tag);
            _context.SaveChanges();
        }

        public void UpdateTag(Tag tag)
        {
            _context.Tags.Update(tag);
            _context.SaveChanges();
        }

        public void DeleteTag(int id)
        {
            var tag = _context.Tags.Find(id);
            if (tag != null)
            {
                // Xóa tất cả bản ghi trong bảng NewsTag có chứa TagId
                var relatedNewsTags = _context.Set<Dictionary<string, object>>("NewsTag")
                    .Where(nt => EF.Property<int>(nt, "TagId") == id)
                    .ToList();

                _context.RemoveRange(relatedNewsTags);
                _context.SaveChanges();

                // Xóa tag
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }

        public int GetLastId()
        {
            return _context.Tags.OrderByDescending(o => o.TagId).FirstOrDefault()?.TagId ?? 0;
        }

        public List<Tag> GetTagsByIds(List<int> tagIds)
        {
            return _context.Tags.Where(t => tagIds.Contains(t.TagId)).ToList();
        }

        public (List<Tag>, int totalItems) findALlWithPagination(int pg, int pageSize)
        {
            var list = _context.Tags.OrderByDescending(x=>x.TagId).ToList();

            if (pg == 1)
            {
                pg = 1;
            }
            int resCount = list.Count();
            var pager = new Pager(resCount, pg, pageSize);
            int recSkip = (pg - 1) * pageSize;
            var data = list.Skip(recSkip).Take(pager.Pagesize).ToList();
            return (data, resCount);
        }
    }

}
