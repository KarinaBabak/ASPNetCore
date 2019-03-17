using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data.DTOModels;
using NorthwindSystem.Persistence.Interface;
using CategoryDAOEntity = NorthwindSystem.Data.Entities.Category;

namespace NorthwindSystem.BLL.Implementation
{
    public class Configuration : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public Configuration(ICategoryRepository repository, IMapper mapper)
        {
            _categoryRepository = repository;
            _mapper = mapper;
        }

        public async Task<int> Add(CategoryDto entity)
        {
            var category = _mapper.Map<CategoryDAOEntity>(entity);
            return await _categoryRepository.Add(category);
        }

        public async Task Delete(CategoryDto entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CategoryDto>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }

        public async Task<CategoryDto> GetById(int entityId)
        {
            var category = await _categoryRepository.GetById(entityId);
            return _mapper.Map<CategoryDto>(category);
        }

        public async Task Update(CategoryDto entity)
        {
            var category = _mapper.Map<CategoryDAOEntity>(entity);
            await _categoryRepository.Update(category);
        }
    }
}
