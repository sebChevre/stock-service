using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockApi.Services;
using StockApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("/api/stock")]
    public class StockController : ControllerBase
    {

        private readonly StockService _StockService;

        private readonly ILogger<StockController> _logger;

        public StockController(StockService StockService,ILogger<StockController> logger)
        {
            _StockService = StockService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "reader")]
        public List<Stock> GetAllStocks()
        {
            _logger.LogInformation("GetAllStocks called");
            return _StockService.GetAllStocks();
        }


        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "reader")]
        public IActionResult GetStockByBiereId(string id)
        {
            _logger.LogInformation("GetStockByBiereId called, with biereId: " + id);
            Stock stock = _StockService.GetStockByBiereId(id);

            if(stock == null){
                return NotFound("/api/stock/" + id);
            }

            return Ok(stock);
        }

        [HttpPost]
        [Authorize(Policy = "contributor")]
        public IActionResult CreateStock(Stock stock){
            _logger.LogInformation("CreateStock called, with stock: {}",stock);
            _StockService.CreateStock(stock);
            return Created("/api/stock/" + stock.Id,"");
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = "contributor")]
        public IActionResult UpdateStock(Stock stock,string id){
            _logger.LogInformation("UpdateStock called for id: {}",id);
            _StockService.UpdateStock(stock,id);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "contributor")]
        public IActionResult  DeleteStock(String id){
            _logger.LogInformation("DeleteStock called, for id: {}",id);
            _StockService.DeleteStock(id);
            return Ok("Entity with id : " + id + "sucessfully deleted!");
        }
    }
}
