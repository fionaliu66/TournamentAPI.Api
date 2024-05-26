using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentAPI.Core.Repositories;
using TournamentAPI.Data.Data;

namespace TournamentAPI.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private TournamentAPIApiContext _context;
        private TournamentRepository _tournamentRepo;
        private GameRepository _gameRepo;
        public UnitOfWork(TournamentAPIApiContext context)
        {
            _context = context;
        }

        public ITournamentRepository TournamentRepository
        {
            get
            {
                if(this._tournamentRepo == null)
                {
                    this._tournamentRepo = new TournamentRepository(_context);
                }
                return this._tournamentRepo;
            }
        }

        public IGameRepository GameRepository
        {
            get
            {
                if (this._gameRepo == null)
                {
                    this._gameRepo = new GameRepository(_context);
                }
                return this._gameRepo;
            }
        }

        public async Task CompleteAsync()
        {
             await _context.SaveChangesAsync();
        }
    }
}
