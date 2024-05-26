﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class GameRepository : IGameRepository
    {
        private TournamentAPIApiContext _context;

        public GameRepository(TournamentAPIApiContext tournamentAPIApiContext)
        {
            _context = tournamentAPIApiContext;
        }
        public void Add(Game game)
        {
            _context.Add(game);
        }

        public Task<bool> AnyAsync(int id)
        {
            return _context.Game.AnyAsync(g => g.Id == id);
        }

        public void Delete(Game game)
        {
            _context.Remove(game);
        }

        public async Task<IEnumerable<Game>> GetAllAsync()
        {
            return await _context.Game.ToListAsync();
        }

        public Task<Game> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            _context.Game.Update(game);
        }
    }
}