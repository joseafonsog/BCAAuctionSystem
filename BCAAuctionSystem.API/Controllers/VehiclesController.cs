using BCAAuctionSystem.API.Infrastructure;
using BCAAuctionSystem.API.Models;
using BCAAuctionSystem.API.Services;

using Microsoft.AspNetCore.Mvc;

namespace BCAAuctionSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleAuctionService _vehicleAuctionService;

        public VehiclesController(IVehicleAuctionService vehicleAuctionService)
        {
            _vehicleAuctionService = vehicleAuctionService;
        }

        [HttpPost]
        public IActionResult AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                return BadRequest("Vehicle cannot be null.");
            }

            try
            {
                _vehicleAuctionService.AddVehicle(vehicle);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpGet]
        public IActionResult SearchVehicles(string type = "", string manufacturer = "", string model = "", int year = 0)
        {
            var result = _vehicleAuctionService.SearchVehicles(type, manufacturer, model, year);
            return Ok(result);
        }

        [HttpPost("{identifier}/auction/start")]
        public IActionResult StartAuction(string identifier)
        {
            try
            {
                _vehicleAuctionService.StartAuction(identifier);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("{identifier}/bid")]
        public IActionResult PlaceBid(string identifier, [FromBody] BidRequest request)
        {
            try
            {
                _vehicleAuctionService.PlaceBid(identifier, request.Amount, request.Bidder);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{identifier}/auction/close")]
        public IActionResult CloseAuction(string identifier)
        {
            try
            {
                _vehicleAuctionService.CloseAuction(identifier);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}