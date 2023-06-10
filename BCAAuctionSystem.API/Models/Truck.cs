using BCAAuctionSystem.API.Commons;

namespace BCAAuctionSystem.API.Models
{
    public class Truck : Vehicle
    {
        public decimal LoadCapacity { get; set; }
        public override VehicleType Type => VehicleType.Truck;
    }
}
