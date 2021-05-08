using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Serilog.Elastic.Kibana.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clientId = Faker.RandomNumber.Next(1, 1000);
            _logger.LogInformation("Entering get method... {@clientId}", clientId);
            try
            {
                for (int i = 3; i >= 0; i--)
                {
                    _logger.LogDebug("Custom object in log {@CustomerObj}", new Customer(clientId, Faker.Name.FullName(), Faker.Name.FullName()));
                    DoSomething(i);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Get... {@clientId}", clientId);
            }
            return StatusCode(StatusCodes.Status200OK, "Working");
        }

        private static int DoSomething(int i)
        {
            return i / i;
        }
    }

    public class Customer
    {
        public int ClientId { get; set; }
        public string UserName { get; set; }
        public string BusinessName { get; set; }
        public Customer(int clientId, string userName, string businessName)
        {
            ClientId = clientId;
            UserName = userName;
            BusinessName = businessName;
        }
    }
}
