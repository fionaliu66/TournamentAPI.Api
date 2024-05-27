using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TournamentAPI.Data.Data;
using TournamentAPI.Core.Entities;
using TournamentAPI.Data.Repositories;
using TournamentAPI.Core.Repositories;
using AutoMapper;
using TournamentAPI.Core.Dto;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
namespace TournamentAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public TournamentsController(TournamentAPIApiContext context,IMapper mapper)
        {
            unitOfWork = new UnitOfWork(context);
            _mapper = mapper;
        }

        // GET: api/Tournaments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournament()
        {
            var tournaments =  await unitOfWork.TournamentRepository.GetAllAsync();
            var tournamentsDto = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);
            return Ok(tournamentsDto);
        }

        // GET: api/Tournaments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(int id)
        {
            var tournament = await unitOfWork.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound();
            }
            var tournamentDto = _mapper.Map<TournamentDto>(tournament);
            return Ok(tournamentDto);
        }

        // PUT: api/Tournaments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return BadRequest("ID in URL does not match ID in the body.");
            }

            var existingTournament = await unitOfWork.TournamentRepository.GetAsync(id);
            if (existingTournament == null)
            {
                return NotFound();
            }

            existingTournament.Title = tournament.Title;
            existingTournament.StartDate = tournament.StartDate;
            //To do, update game list here
            unitOfWork.TournamentRepository.Update(existingTournament);

            try
            {
                await unitOfWork.CompleteAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(id))
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

        // POST: api/Tournaments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(Tournament tournament)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            unitOfWork.TournamentRepository.Add(tournament);
            await unitOfWork.CompleteAsync();

            var tournamentDto = _mapper.Map<TournamentDto>(tournament); 
            return CreatedAtAction(nameof(GetTournament), new { id = tournament.Id }, tournamentDto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<TournamentDto>> PatchTournament(int id,JsonPatchDocument<TournamentDto> patchDocument)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!TournamentExists(id))
            {
                return NotFound();
            }
            var tournamentEntity = await unitOfWork.TournamentRepository.GetAsync(id);
            var tournamentToPatch = _mapper.Map<TournamentDto>(tournamentEntity);
            patchDocument.ApplyTo(tournamentToPatch, ModelState);

            if (!TryValidateModel(tournamentToPatch))
            {
                return BadRequest(ModelState);
            }
         
              _mapper.Map(tournamentToPatch, tournamentEntity);
            await unitOfWork.CompleteAsync();
            return NoContent();
        }
        // DELETE: api/Tournaments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await unitOfWork.TournamentRepository.GetAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

           unitOfWork.TournamentRepository.Remove(tournament);
            await unitOfWork.CompleteAsync();

            return NoContent();
        }

        private bool TournamentExists(int id)
        {
            return unitOfWork.TournamentRepository.AnyAsync(id).Result;
        }
    }
}
