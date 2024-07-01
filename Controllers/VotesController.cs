using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VotesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost("CreateVote")]
        public async Task<IActionResult> CreateVote([FromBody] int topicId)
        {
            var topic = await _context.topics.FindAsync(topicId);
            if (topic == null)
            {
                return NotFound($"Topic with ID {topicId} not found.");
            }

            var vote = new Vote
            {
                TopicId = topicId,
                Agree = 0,
                Disagree = 0,
                Abstaining = 0
            };

            _context.Votes.Add(vote);
            await _context.SaveChangesAsync();

            return Ok(vote);
        }

        [Authorize]
        [HttpGet("GetVoteById/{id}")]
        public async Task<IActionResult> GetVoteById(int id)
        {
            var vote = await _context.Votes.Include(v => v.Topic).FirstOrDefaultAsync(v => v.Id == id);
            if (vote == null)
            {
                return NotFound($"Vote with ID {id} not found.");
            }

            return Ok(vote.Topic.Title);
        }

        [Authorize]
        [HttpPost("Voting")]
        public async Task<IActionResult> Voting([FromBody] VotingDto votingDto)
        {
            var vote = await _context.Votes.FindAsync(votingDto.VoteId);
            if (vote == null)
            {
                return NotFound($"Vote with ID {votingDto.VoteId} not found.");
            }

            switch (votingDto.VoteType)
            {
                case "agree":
                    vote.Agree++;
                    break;
                case "disagree":
                    vote.Disagree++;
                    break;
                case "abstaining":
                    vote.Abstaining++;
                    break;
                default:
                    return BadRequest("Invalid vote type.");
            }

            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();

            return Ok("Vote counted successfully.");
        }

        [Authorize]
        [HttpGet("GetResultOfTheVote/{id}")]
        public async Task<IActionResult> GetResultOfTheVote(int id)
        {
            var vote = await _context.Votes.FindAsync(id);
            if (vote == null)
            {
                return NotFound($"Vote with ID {id} not found.");
            }

            var result = new
            {
                Agree = vote.Agree,
                Disagree = vote.Disagree,
                Abstaining = vote.Abstaining
            };

            return Ok(result);
        }

        [Authorize]
        [HttpPost("EndVoting")]
        public async Task<IActionResult> EndVoting([FromBody] int voteId)
        {
            var vote = await _context.Votes.FindAsync(voteId);
            if (vote == null)
            {
                return NotFound($"Vote with ID {voteId} not found.");
            }

            vote.Agree = 0;
            vote.Disagree = 0;
            vote.Abstaining = 0;

            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();

            return Ok("Voting ended and counts reset to zero.");
        }
    }
}
