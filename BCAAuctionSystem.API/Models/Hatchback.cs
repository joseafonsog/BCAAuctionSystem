using BCAAuctionSystem.API.Commons;

namespace BCAAuctionSystem.API.Models
{
    public class Hatchback : Vehicle
    {
        public int NumberOfDoors { get; set; }
        public override VehicleType Type => VehicleType.Hatchback;
    }
}
