using Microsoft.AspNetCore.Mvc;

namespace testAPi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private static List<Customer> _CUSTOMERS = new List<Customer>();

    /// <summary>
    /// Obtiene la lista de todo los clientes.
    /// </summary>
    /// <returns>La lista de clientes.</returns>
    [HttpGet]
    public ActionResult<IEnumerable<Customer>> GetCustomers()
    {
        return _CUSTOMERS;
    }

    [HttpGet("{id}")]
    public ActionResult<Customer> GetCustomer(int id)
    {
        var customer = _CUSTOMERS.FirstOrDefault(p => p.Id == id);
        if (customer == null)
        {
            return NotFound();
        }
        return customer;
    }

    /// <summary>
    /// Crea un nuevo cliente.
    /// </summary>
    [HttpPost]
    public ActionResult<Customer> PostCustomer(Customer customer)
    {
        customer.Id = _CUSTOMERS.Count + 1;
        _CUSTOMERS.Add(customer);
        return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
    }

    /// <summary>
    /// Edita un cliente.
    /// </summary>
    /// /// <remarks>
    /// Puedes editar un cliente. 
    /// Solo debes pasar el id del cliente que deseas editar.
    ///  
    /// </remarks>
    [HttpPut("{id}")]
    public IActionResult PutCustomer(int id, Customer customer)
    {
        var existingCustomer = _CUSTOMERS.FirstOrDefault(p => p.Id == id);
        if (existingCustomer == null)
        {
            return NotFound();
        }

        existingCustomer.Name = customer.Name;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.City = customer.City;

        return NoContent();
    }


    /// <summary>
    /// Elima un cliente.
    /// </summary>
    /// /// <remarks>
    /// Puedes eliminar un cliente. 
    /// Solo debes pasar el id del cliente que deseas eliminar.
    ///  
    /// </remarks>
    [HttpDelete("{id}")]
    public IActionResult DeleteCustomer(int id)
    {
        var customer = _CUSTOMERS.FirstOrDefault(p => p.Id == id);
        if (customer == null)
        {
            return NotFound();
        }

        _CUSTOMERS.Remove(customer);
        return NoContent();
    }

    /// <summary>
    /// Busca un cliente.
    /// </summary>
    /// /// <remarks>
    /// Puedes Buscar un cliente. 
    /// Solo debes pasar el id del cliente o el nombre, del cliente que deseas buscar.
    ///  
    /// </remarks>
    [HttpGet("Buscar")]
    public ActionResult<IEnumerable<Customer>> BuscarCliente([FromQuery] int? id, [FromQuery] string? name)
    {
        var result = _CUSTOMERS.AsEnumerable();

        if (id.HasValue)
        {
            result = result.Where(customer => customer.Id == id.Value);
        }

        if (!string.IsNullOrEmpty(name))
        {
            result = result.Where(customer => customer.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!result.Any())
        {
            return NotFound("No se encontraron clientes con los criterios proporcionados.");
        }

        return Ok(result);
    }

}