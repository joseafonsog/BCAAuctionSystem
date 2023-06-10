using BCAAuctionSystem.API.Models;

namespace BCAAuctionSystem.API.Services
{
    public interface IVehicleAuctionService
    {
        void AddVehicle(Vehicle vehicle);
        List<Vehicle> SearchVehicles(string type, string manufacturer, string model, int year);
        void StartAuction(string identifier);
        void PlaceBid(string identifier, decimal amount, string bidder);
        void CloseAuction(string identifier);
    }
}
