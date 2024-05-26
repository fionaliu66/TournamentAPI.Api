using Bogus;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;

namespace TournamentAPI.Data.Data
{
    public class SeedData
    {
        private static Faker faker;
        private static Random random = new Random();
        public static async Task InitAsync(TournamentAPIApiContext context)
        {
            if (await context.Tournaments.AnyAsync()) return;

            faker = new Faker();

            var tournamentList = new List<Tournament>();
            for (int i = 0; i < 10; i++)
            {
                var tournament = new Tournament
                {
                    Title = faker.Lorem.Word(),
                    StartDate = faker.Date.Past(),
                    Games = new List<Game>()
                };
                await context.Tournaments.AddAsync(tournament);
                await context.SaveChangesAsync();
                for (int j = 0; j < faker.Random.Int(1, 11); j++)
                {
                    var Game = new Game
                    {
                        Title = faker.Lorem.Word(),
                        Time = faker.Date.Between(tournament.StartDate, tournament.StartDate.AddDays(10)),
                        TournamentId = tournament.Id
                    };
                    await context.Games.AddAsync(Game);
                    await context.SaveChangesAsync();
                }

            }
        }
    }
}
