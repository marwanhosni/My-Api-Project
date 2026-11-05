using MyApiProject.Core.Entities;
using MyApiProject.Core.Entities.Orders;
using MyApiProject.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApiProject.Repository.Implementations;
public class OrderService : IOrderService
{
    private readonly IBasketRepository _basketRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OrderService(IBasketRepository basketRepository,
        IUnitOfWork unitOfWork)
    {
        _basketRepository = basketRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<Order?> GreateOrderAsync(string BuyerEmail, string BasketId, int DeleviryMethodId, Address ShippingAddress)
    {
        //1. Get Basket from BasketRepo
        var basket = await _basketRepository.GetBasketAsync(BasketId);

        //2. Get Items from Product Repo
        var OrderItems = new List<OrderItem>();
        if (basket?.Items.Count > 0) 
        {
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);
                var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                OrderItems.Add(orderItem);
            }
        }

        //3. Calculate Subtotal
        var subtotal = OrderItems.Sum(sub => sub.Price * sub.Quantity);

        //4. Delevery Method
        var deleveryMethod = await _unitOfWork.Repository<DeliveryMethods>().GetByIdAsync(DeleviryMethodId);

        //5. Create Order
        var order = new Order(BuyerEmail, ShippingAddress, deleveryMethod,OrderItems,subtotal);

        //6. Add Order Locally
        await _unitOfWork.Repository<Order>().Add(order);

        //7. Add Order TO DataBase 
        var result = await _unitOfWork.CompleteAsync();

        return result > 0 ? order : null;
    }

    public Task<IReadOnlyList<Order>?> GetOrederForSpecAsync(string Email)
    {
        throw new NotImplementedException();

    }
    public Task<Order?> GetOrederByIdForSpecAsync(int Id, string Email)
    {
        throw new NotImplementedException();
    }

    

    
}
