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
using AutoMapper;
using TournamentAPI.Core.Dto;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public GamesController(TournamentAPIApiContext context, IMapper mapper)
        {
            unitOfWork = new UnitOfWork(context);
            _mapper = mapper;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGame(string title)
        {
            var games =  await unitOfWork.GameRepository.GetAllAsync();
            if(games == null || !games.Any()) 
            {
                return NotFound();
            }
            else
            {
                var gamesDto = _mapper.Map<Game>(games);
                return Ok(gamesDto);
            }
        
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
            var gameDto = _mapper.Map<Game>(game);
            return Ok(gameDto);
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
            catch (Exception ex)
            {
             
                return StatusCode(500, "An error occurred while saving the game.");
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            unitOfWork.GameRepository.Add(game);
            await unitOfWork.CompleteAsync();
            var GameDto = _mapper.Map<Game>(game);
            return CreatedAtAction(nameof(GetGame), new { id = game.Id }, GameDto);
        }
        [HttpPatch("{id}")]
        public async Task<ActionResult<GameDto>> PatchGame(int id, JsonPatchDocument<GameDto> patchDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!GameExists(id))
            {
                return NotFound();
            }
            var gameEntity = await unitOfWork.GameRepository.GetAsync(id);
            var gameToPatch = _mapper.Map<GameDto>(gameEntity);
            patchDocument.ApplyTo(gameToPatch, ModelState);
            if (!TryValidateModel(gameToPatch))
            {
                return BadRequest(ModelState);
            }
            _mapper.Map(gameToPatch, gameEntity);
            await unitOfWork.CompleteAsync();
            return NoContent();
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
