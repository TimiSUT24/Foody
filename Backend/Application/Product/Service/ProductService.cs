using Application.Exceptions;
using Application.Product.Dto.Request;
using Application.Product.Dto.Response;
using Application.Product.Interfaces;
using AutoMapper;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Product.Service
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper; 

        public ProductService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> AddAsync(CreateProductDto request, CancellationToken ct)
        {
            var exists = await _uow.Products.AnyAsync(p => p.Id == request.Id || p.Name == request.Name, ct);
            if (exists)
            {
                throw new ConflictException("Product already exists");
            }
            var product = _mapper.Map<Domain.Models.Product>(request);
            await _uow.Products.AddAsync(product, CancellationToken.None);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAsync(int offset, int limit, CancellationToken ct)
        {
            var products = await _uow.Products.GetAsync(offset, limit, ct);
            if(products == null || !products.Any())
            {
                throw new KeyNotFoundException("No products found");
            }
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products); 
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(id, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<bool> Update(UpdateProductDto request, CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(request.Id, ct);
            if(product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            
            _mapper.Map(request, product);
            _uow.Products.Update(product);
            await _uow.SaveChangesAsync(ct);
            return true; 
        }

        public async Task<bool> DeleteAsync(int id ,CancellationToken ct)
        {
            var product = await _uow.Products.GetByIdAsync<int>(id, ct);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }
            _uow.Products.Delete(product);
            await _uow.SaveChangesAsync(ct);
            return true;
        }

    }
}
