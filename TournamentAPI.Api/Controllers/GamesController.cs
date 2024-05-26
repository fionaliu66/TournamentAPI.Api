using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Data.Repositories;
using TournamentAPI.Core.Entities;
using TournamentAPI.Core.Repositories;

namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;

        public GamesController(TournamentAPIApiContext context)
        {
            unitOfWork = new UnitOfWork(context);
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame()
        {
            var games =  await unitOfWork.GameRepository.GetAllAsync();
            return Ok(games);
        }

        // GET: api/Games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await unitOfWork.GameRepository.GetAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/Games/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            if (id != game.Id)
            {
                return BadRequest("ID in URL does not match ID in the body.");

            }

            var existingGame = await unitOfWork.GameRepository.GetAsync(id);
            if(existingGame == null)
            {
                return NotFound();
            }
            existingGame.Title = game.Title;
            existingGame.Time = game.Time;
            unitOfWork.GameRepository.Update(existingGame);

            try
            {
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            var tournamentExists = await unitOfWork.TournamentRepository.AnyAsync(game.TournamentId);
            if (!tournamentExists)
            {
                return BadRequest("Invalid TournamentId");
            }
            unitOfWork.GameRepository.Add(game);
            await unitOfWork.CompleteAsync();

            return CreatedAtAction("GetGame", new { id = game.Id }, game);
        }

        // DELETE: api/Games/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await unitOfWork.GameRepository.GetAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            unitOfWork.GameRepository.Delete(game);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }

        private bool GameExists(int id)
        {
            return  unitOfWork.GameRepository.AnyAsync(id).Result;
        }
    }
}
