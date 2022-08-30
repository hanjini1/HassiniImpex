using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Entites;

namespace Core.Specifications
{
  public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
  {
    public ProductWithFiltersForCountSpecification(ProductSpecParams specParams)
    : base(x =>
    (!specParams.BrandId.HasValue || x.ProductBrandId == specParams.BrandId)
    && (!specParams.TypeId.HasValue || x.ProductTypeId == specParams.TypeId)
    )
    {

    }
  }
}
