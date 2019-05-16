using DotNetCoreAPI;
using DotNetCoreAPI.Dtos;
using DotNetCoreAPI.Models;
using DotNetCoreAPI.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace APITest.DatabaseTests
{
    public class DatabaseTest
    {
        [Fact]
        public void CreateProduct_AddsToDatabase()
        {
            var options = new DbContextOptionsBuilder<DALContext>()
                .UseInMemoryDatabase(databaseName: "CreateProductAddsToDatabase")
                .Options;

            var categoryToSearch = "Fruit";
            var fruit = "Strawberries";

            var categorySearched = new Category
            {
                Name = categoryToSearch
            };

            // Run the test against one instance of the context
            using (var context = new DALContext(options))
            {
                var service = new ProductService(context);
                service.CreateProduct(new ProductDto()
                {
                    Name = fruit,
                    Description = fruit,
                    Category = categorySearched.Name
                });
            }

            // Use a separate instance of the context to verify correct data was saved to database
            using (var context = new DALContext(options))
            {
                Assert.Equal(1, context.Products.Count());
                Assert.Equal(fruit, context.Products.Single().Name);
            }
        }

        [Fact]
        public void GetAllProductsByCategory_ReturnsProductsForCategory()
        {
            var options = new DbContextOptionsBuilder<DALContext>()
                .UseInMemoryDatabase(databaseName: "GetAllProductsByCategory")
                .Options;

            var name = "new product";
            var categoryToSearch = "Fruit";
            var listOfFruits = new List<string>
            {
                "Strawberries", "Bananas", "Oranges"
            };
            var listOfNotFruits = new List<string>
            {
                "Bear", "Window"
            };

            var categorySearched = new Category
            {
                Name = categoryToSearch
            };

            var notCategorySearched = new Category
            {
                Name = "Not this"
            };

            var allProducts = new List<Product>();

            foreach (var fruit in listOfFruits)
            {
                allProducts.Add(new Product
                {
                    Name = fruit,
                    Description = fruit,
                    Category = categorySearched
                });
            }

            foreach (var notFruit in listOfNotFruits)
            {
                allProducts.Add(new Product
                {
                    Name = notFruit,
                    Description = notFruit,
                    Category = notCategorySearched
                });
            }

            // Insert seed data into the database using one instance of the context
            using (var context = new DALContext(options))
            {
                foreach(var product in allProducts)
                {
                    context.Products.Add(product);
                }
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new DALContext(options))
            {
                var service = new ProductService(context);
                var result = service.GetAllProductsByCategory(categoryToSearch);
                Assert.Equal(3, result.Count());
            }
        }
    }
}
