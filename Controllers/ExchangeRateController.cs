using Microsoft.AspNetCore.Mvc;

namespace testAPi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class ExchangeRateController : ControllerBase 
{
    private static List<ExchangeRate> _EXCHANGES = new List<ExchangeRate>();

     /// <summary>
    /// Crea una nueva Tasa de cambio.
    /// </summary>
    [HttpPost]
    public ActionResult<ExchangeRate> PostExhangeRate(ExchangeRate exchange)
    {
        exchange.Id = _EXCHANGES.Count + 1;
        _EXCHANGES.Add(exchange);
        return CreatedAtAction(nameof(PostExhangeRate), new { id = exchange.Id }, exchange);
    }

     /// <summary>
    /// Lista todas Tasa de cambio por mes.
    /// </summary>
    [HttpGet("list/{month}")]
    public ActionResult<IEnumerable<ExchangeRate>> GetExchangeRatesByMonth(int month)
    {
        try
        {   
            var exchangeRatesByMonth = _EXCHANGES.Where(e => e.Date.Month == month).ToList();

            if (exchangeRatesByMonth.Count == 0)
            {
                return NotFound($"No hay tasas de cambio para el mes {month}");
            }

            return Ok(exchangeRatesByMonth);
        }
        catch (Exception ex)
        {
            return BadRequest($"Error al obtener tasas de cambio por mes: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtine la Tasa de cambio de hoy.
    /// </summary>
    [HttpGet("today")]
public ActionResult<ExchangeRate> GetExchangeRateToday()
{
    try
    {   

        var today = DateTime.Now.Date;
        var exchangeRateToday = _EXCHANGES.FirstOrDefault(e => e.Date.Date == today);

        if (exchangeRateToday == null)
        {
            return NotFound($"No hay tasa de cambio para hoy ({today})");
        }

        return Ok(exchangeRateToday);
    }
    catch (Exception ex)
    {
        return BadRequest($"Error al obtener tasa de cambio para hoy: {ex.Message}");
    }
}

/// <summary>
    /// Edita la tasa de cambio.
    /// </summary>
    /// /// <remarks>
    /// Puedes editar La tasa de cambio. 
    /// Solo debes pasar el id de la tasa de cambio que deseas editar.
    ///  
    /// </remarks>
    [HttpPut("{id}")]
    public IActionResult PutExchange(int id, ExchangeRate exchange)
    {
        var existingExchange = _EXCHANGES.FirstOrDefault(p => p.Id == id);
        if (existingExchange == null)
        {
            return NotFound();
        }

        existingExchange.BaseCoin = exchange.BaseCoin;
        existingExchange.TargetCurrency = exchange.TargetCurrency;
        existingExchange.Rate = exchange.Rate;
       

        return NoContent();
    }

    /// <summary>
    /// Elima una tasa de cambio.
    /// </summary>
    /// /// <remarks>
    /// Puedes eliminar una tasa de cambio. 
    /// Solo debes pasar el id de la tasa de cambio que deseas eliminar.
    ///  
    /// </remarks>
    [HttpDelete("{id}")]
    public IActionResult DeleteExchange(int id)
    {
        var exchange = _EXCHANGES.FirstOrDefault(p => p.Id == id);
        if (exchange == null)
        {
            return NotFound();
        }

        _EXCHANGES.Remove(exchange);
        return NoContent();
    }

}