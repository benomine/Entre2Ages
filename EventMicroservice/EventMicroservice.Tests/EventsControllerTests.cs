using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using EventMicroservice.API.Controllers;
using EventMicroservice.Domain.Models;
using EventMicroservice.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EventMicroservice.Tests
{
    public class EventsControllerTests
    {
        public Faker<Event> Faker { get; set; }
        public EventDbContextFactory Factory { get; set; }
        public EventsController Controller { get; set; }

        public EventsControllerTests()
        {
            Faker = new Faker<Event>()
                .RuleFor(e => e.Author, f => f.Internet.Email())
                .RuleFor(e => e.Guest, f => f.Internet.Email())
                .RuleFor(e => e.Status, f => f.Random.Bool())
                .RuleFor(e => e.Subject, f => f.Lorem.Sentence(6))
                .RuleFor(e => e.EpochStart, f => f.Date.Recent().ToFileTime())
                .RuleFor(e => e.EpochEnd, (_, u) => DateTimeOffset.FromFileTime(u.EpochStart).AddMinutes(30).ToFileTime())
                .RuleFor(e => e.Id, f => f.Random.Guid());

            Action<DbContextOptionsBuilder> builder = o => o.UseInMemoryDatabase("entre2ages");
            Factory = new EventDbContextFactory(builder);
            Controller = new EventsController(Factory);
        }

        [Fact]
        public async Task GetAll_ReturnEvents()
        {
            var context = Factory.CreateDbContext();
            var events = Enumerable.Range(1, 10)
              .Select(_ => Faker.Generate())
              .ToList();

            context.Events.AddRange(events);
            context.SaveChanges();

            var result = await Controller.GetAll();
            var model = Assert.IsAssignableFrom<IEnumerable<Event>>(result.Value);
            Assert.True(model.Count() >= 10);
        }
    }
}
