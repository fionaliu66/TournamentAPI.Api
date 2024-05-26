using Microsoft.EntityFrameworkCore;
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
    public class TournamentRepository : ITournamentRepository
    {
        private TournamentAPIApiContext _context;
        public TournamentRepository(TournamentAPIApiContext tournamentAPIApiContext) {
            _context = tournamentAPIApiContext;
        }
        public void Add(Tournament tournament)
        {
            _context.Tournaments.Add(tournament);
        }

        public  Task<bool> AnyAsync(int id)
        {
            return _context.Tournaments.AnyAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Tournament>> GetAllAsync()
        {
            return await _context.Tournaments.ToListAsync();
        }

        public async Task<Tournament> GetAsync(int id)
        {
            //handel null here
            return await _context.Tournaments.FindAsync(id);
        }

        public void Remove(Tournament tournament)
        {
             _context.Tournaments.Remove(tournament);
        }

        public void Update(Tournament tournament)
        {
            _context.Entry(tournament).State = EntityState.Modified;
        }
    }
}
