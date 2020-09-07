using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop_7194.Data;
using Shop_7194.Models;

namespace Shop_7194.Controllers
{
    [Route("products")]
    public class ProductController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<Product>>> Get(
            [FromServices] DataContext context
        )
        {
            var products = await context
                .Products
                .Include(x => x.Category)
                .AsNoTracking()
                .ToListAsync();

            return Ok(products);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> GetById(
            int id,
            [FromServices] DataContext context
        )
        {
            var product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return Ok(product);
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Product>> Post(
            [FromServices] DataContext context,
            [FromBody] Product model
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                await context.Products.AddAsync(model);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "Erro ao cadastrar produto: " + e.Message });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<ActionResult<Product>> Put(
            int id,
            [FromServices] DataContext context,
            [FromBody] Product model
        )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != model.Id)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
                context.Entry<Product>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(model);
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "Erro ao atualizar produto: " + e.Message });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult<List<Category>>> Delete(
            int id,
            [FromServices] DataContext context
            )
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);

            if (product == null)
                return NotFound(new { message = "Produto não encontrado" });

            try
            {
                context.Remove(product);
                await context.SaveChangesAsync();

                return Ok(new { message = "Produto removido" });
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "Erro ao deletar produto: " + e.Message });
            }
        }
    }
}
