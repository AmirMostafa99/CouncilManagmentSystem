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
        [Authorize(Policy = "RequireAddTopicPermission")]
        [HttpPost(template: "AddTpic")]
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
                IsDiscussed = topicDto.IsDiscussed,
                Notes = topicDto.Notes,
                Result = topicDto.Result,
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


        [HttpGet("SearchTopicsByTitle")]
        public async Task<IActionResult> SearchTopicsByTitle(string title)
        {
            // Check if the title exists in the database
            var existingTopic = await _context.topics.AnyAsync(t => t.Title == title);

            if (!existingTopic)
            {
                return NotFound($"No topic found with the title {title}.");
            }

            // Search for topics that contain the provided title
            var topics = await _context.topics
                .Where(t => t.Title.Contains(title))
                .ToListAsync();

            return Ok(topics);
        }

    }
}