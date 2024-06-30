using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public TopicsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;

        }

        [Authorize]
        
        [HttpPost(template: "AddTopic")]
        public async Task<IActionResult> AddTopic([FromForm] TopicDto topicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var council = await _context.Councils.FindAsync(topicDto.CouncilId);
            if (council == null)
            {
                return BadRequest("Council not found.");
            }

            string path = Path.Combine(_environment.ContentRootPath, "TopicsFiles");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var topic = new Topic
            {
                Title = topicDto.Title,
                Notes = topicDto.Notes,
                DateTimeCreated = DateTime.Now,
                CouncilId = topicDto.CouncilId,
                Type = topicDto.Type
            };

            if (topicDto.Attachment != null)
            {
                path = Path.Combine(path, topicDto.Attachment.FileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await topicDto.Attachment.CopyToAsync(stream);
                    topic.Attachment = topicDto.Attachment.FileName;
                }
            }

            _context.topics.Add(topic);
            await _context.SaveChangesAsync();

            return Ok("Topic Added Successfully");
        }

        [Authorize]
        [HttpPost("SearchTopicsByTitle")]
        public async Task<IActionResult> SearchTopicsByTitle([FromBody] SearchTopicsByTitleDto dto)
        {
            // Check if the title exists in the database
            var existingTopic = await _context.topics.AnyAsync(t => t.Title == dto.title);

            if (!existingTopic)
            {
                return NotFound($"No topic found with the title {dto.title}.");
            }

            // Search for topics that contain the provided title
            var topics = await _context.topics
                .Where(t => t.Title.Contains(dto.title))
                .ToListAsync();

            return Ok(topics);
        }

       [Authorize]
        [HttpPost("GetAllTopicsByIdCouncil")]
        public async Task<IActionResult> GetAllTopicsByIdCouncil([FromBody] CouncilIdDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid payload.");
            }

            var topics = await _context.topics
                .Where(t => t.CouncilId == dto.CouncilId)
                .ToListAsync();

            if (topics == null || !topics.Any())
            {
                return NotFound($"No topics found for CouncilId: {dto.CouncilId}");
            }
            return Ok(topics);
        }


        [Authorize]
        [Authorize(Policy = "RequireAddResultPermission")]
        [HttpPost("AddResultToTopic")]
        public async Task<IActionResult> AddResultToTopic([FromBody] AddResultToTopicDto addResultToTopicDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var council = await _context.Councils.FindAsync(addResultToTopicDto.CouncilId);
            if (council == null)
            {
                return NotFound($"Council with ID {addResultToTopicDto.CouncilId} not found.");
            }

            var topic = await _context.topics.FirstOrDefaultAsync(t => t.Id == addResultToTopicDto.TopicId && t.CouncilId == addResultToTopicDto.CouncilId);
            if (topic == null)
            {
                return NotFound($"Topic with ID {addResultToTopicDto.TopicId} not found in Council with ID {addResultToTopicDto.CouncilId}.");
            }

            topic.Result = addResultToTopicDto.Result;

            _context.topics.Update(topic);
            await _context.SaveChangesAsync();

            return Ok("Result added to topic successfully.");
        }

        [Authorize]
        [HttpGet("GetTopicsOrderedByTitle")]
        public async Task<IActionResult> GetTopicsOrderedByTitle()
        {
            var topics = await _context.topics.OrderBy(t => t.Title).ToListAsync();
            return Ok(topics);
        }
        [Authorize]
        [HttpGet("GetTopicsOrderedByDate")]
        public async Task<IActionResult> GetTopicsOrderedByDate()
        {
            var topics = await _context.topics
                .OrderBy(t => t.DateTimeCreated)
                .ToListAsync();

            return Ok(topics);
        }

        [Authorize]
        [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost("Report")]
        public async Task<IActionResult> Report([FromBody] ReportRequestDto reportRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var council = await _context.Councils.FindAsync(reportRequest.CouncilId);
            if (council == null)
            {
                return NotFound($"Council with ID {reportRequest.CouncilId} not found.");
            }

            var topics = await _context.topics
                .Where(t => t.CouncilId == reportRequest.CouncilId && reportRequest.TopicIds.Contains(t.Id))
                .ToListAsync();

            if (topics == null || topics.Count == 0)
            {
                return NotFound("No topics found for the provided IDs.");
            }

            return Ok(topics);
        }



    }
}