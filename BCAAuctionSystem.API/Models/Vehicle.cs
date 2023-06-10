using BCAAuctionSystem.API.Commons;

namespace BCAAuctionSystem.API.Models
{

    public abstract class Vehicle
    {
        public string Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public decimal StartingBid { get; set; }
        public bool IsAuctionActive { get; set; }
        public decimal HighestBid { get; set; }
        public string HighestBidder { get; set; }
        public abstract VehicleType Type { get; }
    }
}
