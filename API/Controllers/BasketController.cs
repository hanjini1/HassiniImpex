using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Entites;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
  [Route("api/[controller]")]
  public class BasketController : BaseApiController
  {
    private readonly IBasketRepository _basketRepository;
    private readonly ILogger<BasketController> _logger;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger)
    {
      _basketRepository = basketRepository;
      _logger = logger;
    }
    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
      var basket = await _basketRepository.GetBasketAsync(id);
      return Ok(basket ?? new CustomerBasket(id));
    }
    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
    {
      var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);
      return Ok(updatedBasket);
    }
    [HttpDelete]
    public async Task DeleteBasket(string id)
    {
      await _basketRepository.DeleteBasketAsync(id);
    }
  }
}