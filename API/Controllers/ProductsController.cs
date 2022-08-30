using Infrastructure.Data;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Contracts;
using Core.Specifications;
using AutoMapper;
using API.Dtos;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{

  [Route("api/[controller]")]
  public class ProductsController : BaseApiController
  {
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductBrand> _productBrandRepo;
    private readonly IGenericRepository<ProductType> _productTypeRepo;
    private readonly IMapper _mapper;

    public ProductsController(IGenericRepository<Product> productRepo,
    IGenericRepository<ProductBrand> productBrandRepo,
    IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
    {
      this._productBrandRepo = productBrandRepo ?? throw new ArgumentNullException(nameof(productBrandRepo));
      this._productTypeRepo = productTypeRepo ?? throw new ArgumentNullException(nameof(productTypeRepo));
      this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      this._productRepo = productRepo ?? throw new ArgumentNullException(nameof(productRepo));

    }

    [HttpGet]
    public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProductsAsync([FromQuery] ProductSpecParams specParams)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(specParams);
      var countSpec = new ProductWithFiltersForCountSpecification(specParams);
      var totalItems = await _productRepo.CountAsync(countSpec);
      var products = await _productRepo.ListAsync(spec);
      var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);
      return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, totalItems, data));
    }
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
    {
      var spec = new ProductsWithTypesAndBrandsSpecification(id);
      var product = await _productRepo.GetEntityWithSpec(spec);
      if (product == null) return NotFound(new ApiResponse(404));
      return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
    }
    [HttpGet("brands")]
    public async Task<ActionResult<List<ProductBrand>>> GetProductBrandsAsync()
    {
      var productBrands = await _productBrandRepo.ListAllAsync();
      return Ok(productBrands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<List<ProductType>>> GetProductTypesAsync()
    {
      var productTypes = await _productTypeRepo.ListAllAsync();
      return Ok(productTypes);
    }
  }
}