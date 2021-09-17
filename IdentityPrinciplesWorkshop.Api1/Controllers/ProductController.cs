using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using IdentityPrinciplesWorkshop.Api1.Entities;
using Microsoft.AspNetCore.Authorization;

namespace IdentityPrinciplesWorkshop.Api1.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [Authorize(Policy = "ReadProduct")]
        [HttpGet]
        public IActionResult GetProducts()
        {
            return Ok(new List<Product>()
            {
                new Product(){Id = 1,Name = "Defter",Price = 3,Stock = 500},
                new Product(){Id = 2,Name = "Uçlu Kalem",Price = 20,Stock = 500},
                new Product(){Id = 3,Name = "Tükenmez Kalem",Price = 5,Stock = 500},
                new Product(){Id = 4,Name = "Cetvel",Price = 3,Stock = 500},
                new Product(){Id = 5,Name = "Matara",Price = 30,Stock = 500},
                new Product(){Id = 6,Name = "Silgi",Price = 3,Stock = 500}
            });
        }

        [Authorize(Policy = "CreateOrUpdate")]
        [HttpGet("{id}")]
        public IActionResult Update(int id)
        {
            return Ok($"Id'si {id} olan ürün güncellenmiştir.");
        }

        [Authorize(Policy = "CreateOrUpdate")]
        [HttpPost]
        public IActionResult Create(Product product)
        {
            return Ok(product);
        }
    }
}
