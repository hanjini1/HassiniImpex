using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entites;

namespace Core.Specifications
{
  public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
  {
    public ProductsWithTypesAndBrandsSpecification(ProductSpecParams specParams)
    : base(x =>
    (string.IsNullOrEmpty(specParams.Search) || x.Name.ToLower().Contains(specParams.Search)) &&
    (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId)
    && (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId)
    )
    {
      AddInclude(x => x.ProductBrand);
      AddInclude(x => x.ProductType);
      ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
      switch (specParams.Sort)
      {
        case "priceAsc":
          AddOrderBy(p => p.Price);
          break;
        case "priceDesc":
          AddOrderByDescending(p => p.Price);
          break;
        default:
          AddOrderBy(n => n.Name);
          break;
      }

    }

    public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
    {
      AddInclude(x => x.ProductBrand);
      AddInclude(x => x.ProductType);
    }
  }
}