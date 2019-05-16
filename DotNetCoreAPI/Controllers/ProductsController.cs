using System.Collections.Generic;
using DotNetCoreAPI.Dtos;
using DotNetCoreAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        public ProductsController(DALContext context)
        {
            _productService = new ProductService(context);
        }

        // GET api/products
        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get()
        {
            return _productService.GetAllProducts();
        }

        // GET api/products/category/products
        [HttpGet("{category}/products")]
        public ActionResult<IEnumerable<ProductDto>> Get([FromBody] string category)
        {
            return _productService.GetAllProductsByCategory(category);
        }

        // POST api/products
        [HttpPost]
        public void Post([FromBody] ProductDto productDto)
        {
            _productService.CreateProduct(productDto);
        }

        // DELETE api/products/{productName}
        [HttpDelete("{productName}")]
        public void Delete([FromBody] string name)
        {
            _productService.DeleteProduct(name);
        }
    }
}
