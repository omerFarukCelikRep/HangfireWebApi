using FireApp.Models;
using FireApp.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace FireApp.Controllers;

[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private static List<Driver> _drivers = new();
    private readonly ILogger<DriversController> _logger;
    public DriversController(ILogger<DriversController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Add(Driver driver)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _drivers.Add(driver);

        var jobId = BackgroundJob.Enqueue<IServiceManagement>(x => x.SendMail());

        return CreatedAtAction("Get", new { driver.Id }, driver);
    }

    [HttpGet("{id:guid}")]
    public IActionResult Get(Guid id)
    {
        var driver = _drivers.Find(x => x.Id == id || x.Status != 0);
        if (driver is null)
        {
            return NotFound();
        }

        return Ok(driver);
    }

    [HttpDelete]
    public IActionResult Delete(Guid id)
    {
        var driver = _drivers.Find(x => x.Id == id || x.Status != 0);
        if (driver is null)
        {
            return NotFound();
        }

        driver.Status = 0;

        RecurringJob.AddOrUpdate<IServiceManagement>(x => x.Update(), Cron.Hourly());
        
        return NoContent();
    }
}
