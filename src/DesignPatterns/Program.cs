﻿using DesignPatterns.Solid.LiskovSubstitution;
using DesignPatterns.Solid.OpenClosed;
using DesignPatterns.Solid.OpenClosed.Enums;
using DesignPatterns.Solid.OpenClosed.Filters.Bad;
using DesignPatterns.Solid.OpenClosed.Filters.Good;
using DesignPatterns.Solid.OpenClosed.Filters.Good.Specifications;
using DesignPatterns.Solid.SingleResponsibility;
using System;
using System.Collections.Generic;

namespace DesignPatterns
{
    class Program
    {
        static void Main()
        {
            #region Single Responsibility

            var shoppingCart = new ShoppingCart();

            shoppingCart.Add(new ShoppingItem { Id = 123, Name = "Keyboard", Price = 450 });
            shoppingCart.Add(new ShoppingItem { Id = 124, Name = "Mouse", Price = 300 });
            shoppingCart.Add(new ShoppingItem { Id = 125, Name = "Monitor", Price = 2200 });

            var utils = new InvoiceUtility();
            utils.SaveToFile(shoppingCart, "C:\\Users\\Giorgi\\Desktop\\Invoice.txt", true);

            #endregion

            #region Open-Closed

            var vehiclesAtDealership = new List<Vehicle>
            {
                new Vehicle(Brand.Ford, VehicleType.SUV, Color.Blue, 2.5m),
                new Vehicle(Brand.Toyota, VehicleType.Sedan, Color.White, 2m),
                new Vehicle(Brand.Nissan, VehicleType.Sedan, Color.Red, 1.6m),
                new Vehicle(Brand.Dodge, VehicleType.Pickup, Color.Black, 5.5m),
                new Vehicle(Brand.Ford, VehicleType.Crossover, Color.Orange, 0),
                new Vehicle(Brand.Chevrolet, VehicleType.Truck, Color.Purple, 7.5m)
            };

            // BAD PRACTICE ALERT:
            var badFilter = new BadVehicleFilter();

            // First let's filter it by brand
            var filteredVehiclesByBrand = badFilter.FilterByBrand(vehiclesAtDealership, Brand.Ford);
            foreach (var vehicle in filteredVehiclesByBrand)
                Console.WriteLine(vehicle.Brand.ToString());

            // When we need to filter by other properties, we need to modify our filter and add yet another method:
            var filteredVehiclesByColor = badFilter.FilterByColor(vehiclesAtDealership, Color.Black);
            foreach (var vehicle in filteredVehiclesByColor)
                Console.WriteLine(vehicle.Brand.ToString());

            // This will apply to any other property

            //Now let's use the specification pattern:
            var goodFilter = new GoodFilter();
            var filteredVehicles = goodFilter.Filter(vehiclesAtDealership, new ColorSpecification(Color.Blue));
            foreach (var vehicle in filteredVehicles)
                Console.WriteLine(vehicle.Brand.ToString());

            // Multiple Specifications:
            var filteredVehiclesByColorAndBrand = goodFilter.Filter(
                vehiclesAtDealership,
                new AndSpecification<Vehicle>(
                    new ColorSpecification(Color.Purple),
                    new BrandSpecification(Brand.Chevrolet)));

            foreach (var vehicle in filteredVehiclesByColorAndBrand)
                Console.WriteLine(vehicle.Brand.ToString());

            #endregion

            #region Liskov Substitution
            var rectangle = new Rectangle(5, 2);
            Console.WriteLine(rectangle);

            Rectangle square = new Square(); // It should work without any side effects, but it won't.
            square.Width = 4;

            Console.WriteLine(square);

            //To fix this, we can make rectangle properties virtual and override them in a square class.
            #endregion
        }
    }
}
