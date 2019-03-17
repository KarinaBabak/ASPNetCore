using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data.DTOModels;
using NorthwindSystem.Persistence.Interface;
using ProductDAOEntity = NorthwindSystem.Data.Entities.Product;


namespace NorthwindSystem.BLL.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILocalConfiguration _config;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, ILocalConfiguration config, IMapper mapper)
        {
            _productRepository = repository;
            _config = config;
            _mapper = mapper;
        }

        public async Task<int> Add(ProductDto entity)
        {
            var product = _mapper.Map<ProductDAOEntity>(entity);
            return await _productRepository.Add(product);
        }

        public async Task Delete(ProductDto entity)
        {
            var product = _mapper.Map<ProductDAOEntity>(entity);
            await _productRepository.Delete(product);
        }

        public async Task<IEnumerable<ProductDto>> GetAll()
        {
            IEnumerable<ProductDAOEntity> items;
            int maxProductNumber = _config.GetMaxProductsNumber();
            if (maxProductNumber > 0)
            {
                items = await _productRepository.GetNumberItems(maxProductNumber);
            }
            else
            {
                items = await _productRepository.GetAll();
            }

            return _mapper.Map<IEnumerable<ProductDto>>(items);
        }

        public async Task<ProductDto> GetById(int entityId)
        {
            var product = await _productRepository.GetById(entityId);
            return _mapper.Map<ProductDto>(product);
        }

        public async Task Update(ProductDto entity)
        {
            var product = _mapper.Map<ProductDAOEntity>(entity);
            await _productRepository.Update(product);
        }
    }
}
