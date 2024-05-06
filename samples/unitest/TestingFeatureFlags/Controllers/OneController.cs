using Microsoft.AspNetCore.Mvc;
using TestingFeatureFlags.Models;
using TestingFeatureFlags.Services;

namespace TestingFeatureFlags.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OneController : ControllerBase
    {
        private readonly IDataService _dataService;

        public OneController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet("{id}")]
        public async Task<OneModel> Get(string id)
        {
            return await _dataService.ReadDataOneAsync(id);
        }
    }
}
