using BCAAuctionSystem.API.Controllers;
using BCAAuctionSystem.API.Infrastructure;
using BCAAuctionSystem.API.Models;
using BCAAuctionSystem.API.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;

using Moq;

using System;

namespace BCAAuctionSystem.Tests
{
    public class VehicleAuctionServiceTests
    {
        [Fact]
        public void AddVehicle_ValidVehicle_AddsVehicleToInventory()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4
            };

            // Act
            service.AddVehicle(vehicle);

            // Assert
            var addedVehicle = service.GetVehicleById("ABC123");
            Assert.Equal(vehicle, addedVehicle);
        }

        [Fact]
        public void AddVehicle_DuplicateId_ThrowsException()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.AddVehicle(vehicle));
        }

        [Fact]
        public void SearchVehicles_ValidCriteria_ReturnsMatchingVehicles()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var sedan = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            var suv = new SUV
            {
                Id = "DEF456",
                Manufacturer = "Ford",
                Model = "Explorer",
                Year = 2022,
                StartingBid = 15000,
                NumberOfSeats = 7
            };
            service.AddVehicle(sedan);
            service.AddVehicle(suv);

            // Act
            var result = service.SearchVehicles("Sedan", "", "", 2022);

            // Assert
            Assert.Single(result);
            Assert.Equal(sedan, result[0]);
        }

        [Fact]
        public void StartAuction_ValidId_StartsAuctionForVehicle()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            service.AddVehicle(vehicle);

            // Act
            service.StartAuction("ABC123");

            // Assert
            Assert.True(vehicle.IsAuctionActive);
        }

        [Fact]
        public void StartAuction_InvalidId_ThrowsException()
        {
            // Arrange
            var service = new VehicleAuctionService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.StartAuction("INVALID"));
        }

        [Fact]
        public void PlaceBid_ValidRequest_PlacesBidOnVehicle()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                IsAuctionActive= true
            };
            service.AddVehicle(vehicle);

            // Act
            service.PlaceBid("ABC123", 15000, "John");

            // Assert
            Assert.Equal(15000, vehicle.HighestBid);
            Assert.Equal("John", vehicle.HighestBidder);
        }

        [Fact]
        public void PlaceBid_InvalidVehicle_ThrowsException()
        {
            // Arrange
            var service = new VehicleAuctionService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.PlaceBid("INVALID", 15000, "John"));
        }

        [Fact]
        public void PlaceBid_InvalidAmount_ThrowsException()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                HighestBid = 15000
            };
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.PlaceBid("ABC123", 10000, "John"));
        }

        [Fact]
        public void CloseAuction_ValidId_ClosesAuctionForVehicle()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                IsAuctionActive = true
            };
            service.AddVehicle(vehicle);

            // Act
            service.CloseAuction("ABC123");

            // Assert
            Assert.False(vehicle.IsAuctionActive);
        }

        [Fact]
        public void CloseAuction_InvalidId_ThrowsException()
        {
            // Arrange
            var service = new VehicleAuctionService();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => service.CloseAuction("INVALID"));
        }

        [Fact]
        public void AddVehicle_NullVehicle_ThrowsArgumentNullException()
        {
            // Arrange
            var service = new VehicleAuctionService();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.AddVehicle(null));
        }

        [Fact]
        public void GetVehicleById_InvalidId_ReturnsNull()
        {
            // Arrange
            var service = new VehicleAuctionService();

            // Act
            var result = service.GetVehicleById("INVALID");

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void SearchVehicles_NoMatchingCriteria_ReturnsEmptyList()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var sedan = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4
            };
            service.AddVehicle(sedan);

            // Act
            var result = service.SearchVehicles("SUV", "Ford", "Explorer", 2022);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void StartAuction_AlreadyActiveAuction_ThrowsInvalidOperationException()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                IsAuctionActive = true
            };
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.StartAuction("ABC123"));
        }

        [Fact]
        public void PlaceBid_HighestBidHigher_ThrowsArgumentException()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                HighestBid = 15000,
                HighestBidder = "John"
            };
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.PlaceBid("ABC123", 12000, "Jane"));
        }

        [Fact]
        public void CloseAuction_AuctionNotActive_ThrowsInvalidOperationException()
        {
            // Arrange
            var service = new VehicleAuctionService();
            var vehicle = new Sedan
            {
                Id = "ABC123",
                Manufacturer = "Toyota",
                Model = "Camry",
                Year = 2022,
                StartingBid = 10000,
                NumberOfDoors = 4,
                IsAuctionActive = false
            };
            service.AddVehicle(vehicle);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => service.CloseAuction("ABC123"));
        }
    }
}