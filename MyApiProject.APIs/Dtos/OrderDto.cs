using MyApiProject.Core.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace MyApiProject.APIs.Dtos;

public class OrderDto
{
    [Required]
   public string BasketId { get; set; }

    [Required]
    public int DeleviryMethodId { get; set; }

    [Required]
    public AddressDto ShippingAddress { get; set; }
}
