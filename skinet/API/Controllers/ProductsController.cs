using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IProductRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        return Ok(await repository.GetProductsAsync(brand, type, sort));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)//[FromBody]
    {
        repository.AddProduct(product);
        if(await repository.SaveChangesAsync()) 
        { 
            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }
        return BadRequest("Problem creating product");
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id || !ProductExists(id))
        {
            return BadRequest();
        }

        repository.UpdateProduct(product);
        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem updating product");
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await repository.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        repository.DeleteProduct(product);

        if (await repository.SaveChangesAsync())
        {
            return NoContent();
        }
        return BadRequest("Problem deleting product");
    }
  
    private bool ProductExists(int id)
    {
        return repository.ProductExists(id);
    }
    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        return Ok(await repository.GetBrandsAsync());
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        return Ok(await repository.GetTypesAsync());
    }
}