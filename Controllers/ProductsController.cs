using Microsoft.AspNetCore.Mvc;

namespace testAPi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ProductsController : ControllerBase
{
    private static List<Products> _PRODUCTS = new List<Products>();

     /// <summary>
    /// Obtiene la lista de todo los Productos.
    /// </summary>
    /// <returns>Lista de Productos.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Products>> GetProducts()
    {
        return _PRODUCTS;
    }

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    [HttpPost]
    public ActionResult<Products> PostCustomer(Products product)
    {
        product.Id = _PRODUCTS.Count + 1;
        _PRODUCTS.Add(product);
        return CreatedAtAction(nameof(GetProducts), new { id = product.Id }, product);
    }

    /// <summary>
    /// Edita un Producto.
    /// </summary>
    /// /// <remarks>
    /// Puedes editar un Producto. 
    /// Solo debes pasar el id del Producto que deseas editar.
    ///  
    /// </remarks>
    [HttpPut("{id}")]
    public IActionResult PutProduc(int id, Products product)
    {
        var existingProduct = _PRODUCTS.FirstOrDefault(p => p.Id == id);
        if (existingProduct == null)
        {
            return NotFound();
        }

        existingProduct.Name = product.Name;
        existingProduct.Activate = product.Activate;
        existingProduct.Description = product.Description;
        existingProduct.PriceUSD = product.PriceUSD;
        existingProduct.PriceCor = product.PriceCor;

        return NoContent();
    }

    /// <summary>
    /// Elimina un producto.
    /// </summary>
    /// /// <remarks>
    /// Puedes eliminar un producto. 
    /// Solo debes pasar el codigo del producto que deseas eliminar.
    ///  
    /// </remarks>
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int code)
    {
        var product = _PRODUCTS.FirstOrDefault(p => p.CodeSKU == code);
        if (product == null)
        {
            return NotFound();
        }

        _PRODUCTS.Remove(product);
        return NoContent();
    }

     /// <summary>
    /// Busca un Producto.
    /// </summary>
    /// /// <remarks>
    /// Puedes Buscar un Producto. 
    /// Solo debes pasar el codigo del producto o la descripcion, 
    /// del producto que deseas buscar.
    ///  
    /// </remarks>
    [HttpGet("Buscar")]
    public ActionResult<IEnumerable<Customer>> BuscarCliente([FromQuery] int? id, [FromQuery] string? description)
    {
        var result = _PRODUCTS.AsEnumerable();

        if (id.HasValue)
        {
            result = result.Where(product => product.CodeSKU == product.CodeSKU);
        }

        if (!string.IsNullOrEmpty(description))
        {
            result = result.Where(product => product.Description.Contains(description, StringComparison.OrdinalIgnoreCase));
        }

        if (!result.Any())
        {
            return NotFound("No se encontraron Productos con los criterios proporcionados.");
        }

        return Ok(result);
    }

}