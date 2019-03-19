using AutoMapper;
using NorthwindSystem.BLL.Interface;
using NorthwindSystem.Data.DTOModels;
using NorthwindSystem.Data.Entities;
using NorthwindSystem.Persistence.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NorthwindSystem.BLL.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public SupplierService(ISupplierRepository repository, IMapper mapper)
        {
            _supplierRepository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SupplierDto>> GetAll()
        {
            var suppliers = await _supplierRepository.GetAll();
            return _mapper.Map<IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> GetById(int modelId)
        {
            var supplier = await _supplierRepository.GetById(modelId);
            return _mapper.Map<SupplierDto>(supplier);
        }
    }
}
