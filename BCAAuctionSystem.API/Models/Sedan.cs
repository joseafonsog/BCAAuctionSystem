using BCAAuctionSystem.API.Commons;

namespace BCAAuctionSystem.API.Models
{
    public class Sedan : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public override VehicleType Type => VehicleType.Sedan;
    }
}
