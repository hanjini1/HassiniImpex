using Infrastructure.Data;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Contracts;

namespace API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ProductsController : ControllerBase
  {
    private readonly IProductRepository _repository;

    public ProductsController(IProductRepository repository)
    {
      this._repository = repository;
    }
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProductsAsync()
    {
      var products = await _repository.GetProductsAsync();
      return Ok(products);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
      return await _repository.GetProductByIdAsync(id);
    }
    [HttpGet("brands")]
    public async Task<ActionResult<List<ProductBrand>>> GetProductBrandsAsync()
    {
      var productBrands = await _repository.GetProductBrandsAsync();
      return Ok(productBrands);
    }
    [HttpGet("types")]
    public async Task<ActionResult<List<ProductType>>> GetProductTypesAsync()
    {
      var productTypes = await _repository.GetProductTypesAsync();
      return Ok(productTypes);
    }
  }
}