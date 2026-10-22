using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApiProject.APIs.Dtos;
using MyApiProject.APIs.Errors;
using MyApiProject.Core.Entities;
using MyApiProject.Core.Products_Specs;
using MyApiProject.Repository.Interfaces;

namespace MyApiProject.APIs.Controllers;

public class ProductController : BaseApiController
{
    private readonly IGenaricRepository<Product> _productRepo;
    private readonly IMapper _mapper;
    private readonly IGenaricRepository<ProductBrand> _brandsRepo;
    private readonly IGenaricRepository<ProductCategory> _categoriesRepo;

    public ProductController(IGenaricRepository<Product> productRepo , IMapper mapper,IGenaricRepository<ProductBrand> brandsRepo , IGenaricRepository<ProductCategory> categoriesRepo)
    {
        _productRepo = productRepo;
        _mapper = mapper;
        _brandsRepo = brandsRepo;
        _categoriesRepo = categoriesRepo;
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetAll([FromQuery]string? sorting)
    {
        //var products = await _productRepo.GetAllAsync();
        var Spec = new ProductWithBrandAndCategorySpecification(sorting);
        var products = await _productRepo.GetAllWithSpecAsync(Spec);
        var result = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products);
        return Ok(result);
    }
    [HttpGet(template: "{id}")]
    public async Task<ActionResult<ProductToReturnDto>> GetById(int Id)
    {
        //var product = await _productRepo.GetByIdAsync(Id);
        var spec = new ProductWithBrandAndCategorySpecification(Id);
        var product = await _productRepo.GetByIdWithSpecAsync(spec);
        if (product == null)
        {
            return NotFound(new ApiRequest(404));
        }
        var result = _mapper.Map<Product, ProductToReturnDto>(product);
        return Ok(result);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
    {
      var brands =  await _brandsRepo.GetAllAsync();
        return Ok(brands);
    }    

    [HttpGet("categories")]
    public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
    {
      var categries =  await _categoriesRepo.GetAllAsync();
        return Ok(categries);
    }    
    
    
}
