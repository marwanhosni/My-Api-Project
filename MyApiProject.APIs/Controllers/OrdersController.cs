using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.APIs.Dtos;
using MyApiProject.APIs.Errors;
using MyApiProject.Core.Entities.Identity;
using MyApiProject.Core.Entities.Orders;
using MyApiProject.Repository.Interfaces;
using OrderAddress = MyApiProject.Core.Entities.Orders.Address;
using System.Security.Claims;

namespace MyApiProject.APIs.Controllers;

public class OrdersController : BaseApiController
{
    private readonly IOrderService _orderService;
    private readonly IMapper _mapper;

    public OrdersController(IOrderService orderService, IMapper mapper)
    {
        _orderService = orderService;
        _mapper = mapper;
    }
    [Authorize]
    [HttpPost()]
    public async Task <ActionResult<Order>> GreateOrder(OrderDto model)
    {
        var userBuyerEmail =  User.FindFirstValue(ClaimTypes.Email);
        var orderAddress = _mapper.Map<AddressDto,OrderAddress>(model.ShippingAddress);
        var order = await _orderService.GreateOrderAsync(userBuyerEmail, model.BasketId, model.DeleviryMethodId, orderAddress);
        if (order == null) return BadRequest(new ApiRequest(400, "There Is A proble in your Order"));
        return Ok(order);
    }
}
