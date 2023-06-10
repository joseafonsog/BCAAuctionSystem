using BCAAuctionSystem.API.Commons;

namespace BCAAuctionSystem.API.Models
{
    public class SUV : Vehicle
    {
        public int NumberOfSeats { get; set; }
        public override VehicleType Type => VehicleType.SUV;
    }
}
