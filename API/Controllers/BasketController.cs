using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
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
    private readonly IMapper _mapper;

    public BasketController(IBasketRepository basketRepository, ILogger<BasketController> logger, IMapper mapper)
    {
      _basketRepository = basketRepository;
      _logger = logger;
      _mapper = mapper;
    }
    [HttpGet]
    public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
    {
      var basket = await _basketRepository.GetBasketAsync(id);
      return Ok(basket ?? new CustomerBasket(id));
    }
    [HttpPost]
    public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto basket)
    {
      var updatedBasket = await _basketRepository.UpdateBasketAsync(_mapper.Map<CustomerBasketDto, CustomerBasket>(basket));
      return Ok(updatedBasket);
    }
    [HttpDelete]
    public async Task DeleteBasket(string id)
    {
      await _basketRepository.DeleteBasketAsync(id);
    }
  }
}