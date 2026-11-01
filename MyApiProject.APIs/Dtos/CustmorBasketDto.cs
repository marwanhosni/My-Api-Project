using MyApiProject.APIs.Dtos;
using System.ComponentModel.DataAnnotations;

namespace MyApiProject.APIs.Dtos
{
    public class CustmorBasketDto
    {
        [Required]
        public string Id { get; set; } = null!;
        [Required]
        public List<BasketItemDto> Items { get; set; } = null!;
    }
}
