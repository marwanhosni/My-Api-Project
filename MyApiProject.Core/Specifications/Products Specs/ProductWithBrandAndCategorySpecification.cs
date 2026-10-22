using MyApiProject.Core.Entities;
using MyApiProject.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyApiProject.Core.Products_Specs
{
    public class ProductWithBrandAndCategorySpecification : BaseSpecification<Product>
    {
        //This Constractor Will Be Used For Creating Object For GetAll Products
        public ProductWithBrandAndCategorySpecification(string? sorting) : base()
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);

            if (!string.IsNullOrEmpty(sorting))
            {
                switch (sorting) 
                { 
                    case "priceAsc":
                        AddOrderBy(P => P.Price);
                        break;  
                    case "pricedesc":
                        AddOrderByDesc(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(P => P.Name);
            }
        }

        public ProductWithBrandAndCategorySpecification(int id) : base(P => P.Id == id)
        {
            Include.Add(P => P.Brand);
            Include.Add(P => P.Category);
        }
    }
}
