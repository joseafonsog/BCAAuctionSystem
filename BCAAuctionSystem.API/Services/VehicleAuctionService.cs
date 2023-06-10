using BCAAuctionSystem.API.Models;

namespace BCAAuctionSystem.API.Services
{
    public class VehicleAuctionService : IVehicleAuctionService
    {
        private readonly List<Vehicle> _vehicles;

        public VehicleAuctionService()
        {
            _vehicles = new List<Vehicle>();
        }

        public void AddVehicle(Vehicle vehicle)
        {
            if (vehicle == null)
            {
                throw new ArgumentNullException("Vehicle cannot be null.");
            }

            if (_vehicles.Any(v => v.Id == vehicle.Id))
            {
                throw new InvalidOperationException("Vehicle with the same identifier already exists.");
            }

            _vehicles.Add(vehicle);
        }

        public List<Vehicle> SearchVehicles(string type, string manufacturer, string model, int year)
        {
            var result = _vehicles.Where(v =>
                (string.IsNullOrEmpty(type) || v.Type.ToString() == type) &&
                (string.IsNullOrEmpty(manufacturer) || v.Manufacturer == manufacturer) &&
                (string.IsNullOrEmpty(model) || v.Model == model) &&
                (year == 0 || v.Year == year)
            ).ToList();

            return result;
        }

        public void StartAuction(string identifier)
        {
            var vehicle = GetVehicleById(identifier);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found.");
            }

            if (vehicle.IsAuctionActive)
            {
                throw new InvalidOperationException("Auction is already active for this vehicle.");
            }

            vehicle.IsAuctionActive = true;
        }

        public void PlaceBid(string identifier, decimal amount, string bidder)
        {
            var vehicle = GetVehicleById(identifier);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found.");
            }

            if (!vehicle.IsAuctionActive)
            {
                throw new InvalidOperationException("Auction is not active for this vehicle.");
            }

            if (amount <= vehicle.HighestBid)
            {
                throw new ArgumentException("Bid amount should be higher than the current highest bid.");
            }

            vehicle.HighestBid = amount;
            vehicle.HighestBidder = bidder;
        }

        public void CloseAuction(string identifier)
        {
            var vehicle = GetVehicleById(identifier);
            if (vehicle == null)
            {
                throw new ArgumentException("Vehicle not found.");
            }

            if (!vehicle.IsAuctionActive)
            {
                throw new InvalidOperationException("Auction is not active for this vehicle.");
            }

            vehicle.IsAuctionActive = false;
            vehicle.StartingBid = vehicle.HighestBid;
        }

        public Vehicle? GetVehicleById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Id cannot be null or empty.");
            }

            return _vehicles.FirstOrDefault(v => v.Id == id);
        }
    }
}
