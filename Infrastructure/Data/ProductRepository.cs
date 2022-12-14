using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Entites;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
  public class ProductRepository : IProductRepository
  {

    private readonly StoreContext _context;
    public ProductRepository(StoreContext context)
    {
      this._context = context;
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {

      return await _context.ProductBrands.ToListAsync();
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
      return await _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand).SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
      return await _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand).ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
      return await _context.ProductTypes.ToListAsync();
    }
  }
}