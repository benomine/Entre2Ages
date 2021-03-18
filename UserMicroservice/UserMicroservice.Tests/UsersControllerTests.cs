using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Bogus.DataSets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserMicroservice.API.Controllers;
using UserMicroservice.Domain.Models;
using UserMicroservice.EntityFramework;
using Xunit;
using static Bogus.DataSets.Name;

namespace UserMicroservice.Tests
{
    public class UsersControllerTests
    {
        public Faker<User> Faker { get; set; }
        public UserDbContextFactory Factory { get; set; }
        public UsersController Controller { get; set; }

        public UsersControllerTests()
        {
            Faker = new Faker<User>()
                .RuleFor(u => u.Name, f => f.Name.FirstName(Gender.Male))
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Address, f => f.Address.StreetAddress())
                .RuleFor(u => u.City, f => f.Address.City())
                .RuleFor(u => u.ZipCode, f => f.Address.ZipCode())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Phone2, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Id, f => f.Random.Guid());

            var fakeJwtSettings = new Faker<JwtSettings>();
            Action<DbContextOptionsBuilder> builder = o => o.UseInMemoryDatabase("entre2ages");
            Factory = new UserDbContextFactory(builder);
            Controller = new UsersController(Factory, fakeJwtSettings);
        }

        [Fact]
        public async Task GetAll_ReturnAllUsers()
        {
            var context = Factory.CreateDbContext();
            var users = Enumerable.Range(1, 10)
              .Select(_ => Faker.Generate())
              .ToList();

            await context.Users.AddRangeAsync(users);
            await context.SaveChangesAsync();

            var result = await Controller.GetAll();
            var model = Assert.IsAssignableFrom<IEnumerable<User>>(result.Value);
            Assert.True(model.Count()>=10);
        }

        [Theory]
        [InlineData(4, 0)]
        [InlineData(12, 5)]
        public async Task Test2(int a, int b)
        {
            Assert.True(a>b);
        }
        
        [Fact]
        public async Task GetById_ShouldReturnUser()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var getResult = await Controller.GetById(user.Id);

            var result = (getResult.Result as OkObjectResult);
            var value = (result.Value as User);
            
            var model = Assert.IsAssignableFrom<User>(value);
            Assert.Equal(user.Id, model.Id);
            Assert.Equal(user.Name, model.Name);
            Assert.Equal(user.Password, model.Password);
            Assert.Equal(user.Phone, model.Phone);
            Assert.Equal(user.Phone2, model.Phone2);
            Assert.Equal(user.Address, model.Address);
            Assert.Equal(user.City, model.City);
            Assert.Equal(user.ZipCode, model.ZipCode);
            Assert.Equal(user.Email, model.Email);
        }

        [Fact]
        public async Task Post_ShouldReturnUser()
        {
            var userWithoutId = Faker.Ignore("Id").Generate();
            var result = (await Controller.Post(userWithoutId)).Result as CreatedAtActionResult;
            var value = (result.Value as User);
            Assert.Equal(201,result.StatusCode);
            Assert.Equal(userWithoutId.Name, value.Name);
            Assert.Equal(userWithoutId.Password, value.Password);
            Assert.Equal(userWithoutId.Phone, value.Phone);
            Assert.Equal(userWithoutId.Phone2, value.Phone2);
            Assert.Equal(userWithoutId.Address, value.Address);
            Assert.Equal(userWithoutId.City, value.City);
            Assert.Equal(userWithoutId.ZipCode, value.ZipCode);
            Assert.Equal(userWithoutId.Email, value.Email);
        }

        [Fact]
        public async Task Get_ShouldReturnNullWithWrongId()
        {
            var guid = Guid.NewGuid();
            var result = await Controller.GetById(guid);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(404, notFoundResult.StatusCode);
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Get_ShouldReturnNullWithWrongEmail()
        {
            var result = await Controller.GetByEmail(new Internet().Email());
            Assert.Null(result.Value);
        }

        [Fact]
        public async Task Update_ShouldReturnNoContentStatusCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var userUpdated = Faker.RuleFor(u => u.Id, f => user.Id).Generate();
            var result = await Controller.Update(user.Id, userUpdated) as NoContentResult;
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnBadRequestCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var userUpdated = Faker.RuleFor(u => u.Id, f => f.Random.Guid()).Generate();
            var result = await Controller.Update(user.Id, userUpdated) as BadRequestResult;
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Update_ShouldReturnNotFoundRequestCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            var userUpdated = Faker.RuleFor(u => u.Id, f => user.Id).Generate();
            var result = await Controller.Update(user.Id, userUpdated) as NotFoundResult;
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task GetByEmail_ShouldReturnUser()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var result = (await Controller.GetByEmail(user.Email)).Result as OkObjectResult;
            var value = (result.Value as User);
            var model = Assert.IsAssignableFrom<User>(value);
            Assert.Equal(user.Id, model.Id);
            Assert.Equal(user.Name, model.Name);
            Assert.Equal(user.Password, model.Password);
            Assert.Equal(user.Phone, model.Phone);
            Assert.Equal(user.Phone2, model.Phone2);
            Assert.Equal(user.Address, model.Address);
            Assert.Equal(user.City, model.City);
            Assert.Equal(user.ZipCode, model.ZipCode);
            Assert.Equal(user.Email, model.Email);
        }

        [Fact]
        public async Task Delete_ShouldReturnNoContentStatusCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var deleteResult = await Controller.DeleteById(user.Id) as NoContentResult;
            Assert.Equal(204,deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteByMail_ShouldReturnNoContentStatusCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var deleteResult = await Controller.DeleteByEmail(user.Email) as NoContentResult;
            Assert.Equal(204, deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteByMail_ShouldReturnNotFoundStatusCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Ignore("Email").Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var deleteResult = await Controller.DeleteByEmail(new Internet().Email()) as NotFoundResult;
            Assert.Equal(404, deleteResult.StatusCode);
        }

        [Fact]
        public async Task DeleteById_ShouldReturnNotFoundStatusCode()
        {
            var context = Factory.CreateDbContext();
            var user = Faker.Generate();
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            var deleteResult = await Controller.DeleteById(Guid.NewGuid()) as NotFoundResult;
            Assert.Equal(404, deleteResult.StatusCode);
        }
    }
}
