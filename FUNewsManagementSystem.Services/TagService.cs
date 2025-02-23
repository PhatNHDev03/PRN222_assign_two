using FUNewsManagementSystem.BusinessObject;
using FUNewsManagementSystem.DataAccess;
using FUNewsManagementSystem.DataAccess.IRepository;
using FUNewsManagementSystem.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsManagementSystem.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public List<Tag> GetAllTags()
        {
            return _tagRepository.GetAllTags();
        }

        public Tag GetTagById(int id)
        {
            return _tagRepository.GetTagById(id);
        }

        public void AddTag(Tag tag)
        {
            _tagRepository.AddTag(tag);
        }

        public void UpdateTag(Tag tag)
        {
            _tagRepository.UpdateTag(tag);
        }

        public void DeleteTag(int id)
        {
            _tagRepository.DeleteTag(id);
        }
        public int getLastId() => _tagRepository.GetLastId();

        public List<Tag> GetTagsByIds(List<int> tagIds)
       => _tagRepository.GetTagsByIds(tagIds);
    }
}
