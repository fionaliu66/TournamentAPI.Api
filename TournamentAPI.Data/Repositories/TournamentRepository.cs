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
            throw new NotImplementedException();
        }

        public Task<bool> AnyAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Tournament>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Tournament> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        public void Update(Tournament tournament)
        {
            throw new NotImplementedException();
        }
    }
}
