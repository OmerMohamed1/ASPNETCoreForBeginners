using ASPNETCoreForBeginners.Data;
using ASPNETCoreForBeginners.Entities;
using ASPNETCoreForBeginners.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreForBeginners.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext dbContext,
            ILogger<ProductsController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = "AgeGreaterThan25")]
        // [CheckPermissionAttirbute(Permission.ReadProducts)]
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            // using Basic Authentication
            //var userName = User.Identity.Name;
            //var userId = ((ClaimsPrincipal)User.Identity).FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var records = _dbContext.Set<Product>().ToList();
            return Ok(records);
        }

        [HttpGet]
        [Route("{id}")]
        //[CheckPermissionAttirbute(Permission.ReadProducts)]
        [LogSensitiveAction]
        public ActionResult<Product> GetById(int id)
        {
            _logger.LogDebug("Getting Product #{id}", id);
            var recordById = _dbContext.Set<Product>().Find(id);
            if (recordById == null)
            {
                _logger.LogWarning("Product #{id} was not Found", id);
            }
            return recordById == null ? NotFound() : Ok(recordById);

        }


        [HttpPost]
        [Route("")]
        public ActionResult<int> CreateProduct(Product product)
        {
            //product.Id = 0;

            _dbContext.Set<Product>().Add(product);
            _dbContext.SaveChanges();

            return Ok(product.Id);
        }

        [HttpPut]
        [Route("")]
        public ActionResult UpdateProduct(Product product)
        {

            var existingProduct = _dbContext.Set<Product>().Find(product.Id);
            if (existingProduct is null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;

            _dbContext.Set<Product>().Update(existingProduct);
            _dbContext.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteProduct(int id)
        {

            var existingProduct = _dbContext.Set<Product>().Find(id);
            if (existingProduct is null)
            {
                return NotFound();
            }
            _dbContext.Set<Product>().Remove(existingProduct);
            _dbContext.SaveChanges();

            return Ok();
        }
    }
}
