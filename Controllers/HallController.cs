using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HallController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public HallController(ApplicationDbContext context)
        {
            _context = context;
        }


        [Authorize]
        [Authorize(Policy = "RequireAddHallPermission")]
        [HttpPost(template: "AddHall")]

        public IActionResult CreateHall([FromBody] HallDTOs Dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hall = new Hall
            {
                Name = Dto.Name,
                NumberOfSeats = Dto.NumberOfSeats,
                Location = Dto.Location
            };

            _context.Halls.Add(hall);
            _context.SaveChanges();

            return Ok(hall);
        }

        [Authorize]
        [Authorize(Policy = "RequireAddHallPermission")]
        [HttpDelete(template: "DeleteHall")]

        public async Task<IActionResult> DeleteHall(int id)
        {
            var hall = await _context.Halls.FindAsync(id);
            if (hall == null)
            {
                return NotFound();
            }

            _context.Halls.Remove(hall);
            await _context.SaveChangesAsync();

            return Ok("The Hall Is Deleted");
        }

        [Authorize]
        [Authorize(Policy = "RequireAddHallPermission")]
        [HttpPut(template: "UpdateHall")]
        public async Task<IActionResult> UpdateHall([FromBody] HallDTOs Dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var hall = await _context.Halls.FindAsync(Dto.id);
            if (hall == null)
            {
                return NotFound();
            }

            hall.Name = Dto.Name;
            hall.NumberOfSeats = Dto.NumberOfSeats;
            hall.Location = Dto.Location;   
            _context.Entry(hall).State = EntityState.Modified;

             await _context.SaveChangesAsync();

            return Ok("The Hall Updated");
        }

        [Authorize]
       
        [HttpPost(template: "SearchHallByName")]

        public IActionResult SearchHallsByName([FromBody] SearchHallsByNameDto dto)
        {
            if (string.IsNullOrEmpty(dto.name))
            {
                return BadRequest("Name parameter is required.");
            }

            var halls = _context.Halls.Where(h => h.Name.Contains(dto.name)).ToList();

            if (halls.Count == 0)
            {
                return NotFound("No halls found with the given name.");
            }

            return Ok(halls);
        }

        
        [Authorize]
       
        [HttpGet(template: "GetAllHalls")]
        public IActionResult GetAllHalls()
        {
            var halls = _context.Halls.ToList();
            return Ok(halls);
        }

        [Authorize]
        
        [HttpPost("GetHallById")]
        public async Task<IActionResult> GetHallById([FromBody] GetHallByIdDTO request)
        {
            var hall = await _context.Halls
                                    .Where(h => h.Id == request.HallId)
                                    .Select(h => new HallDetailDTO
                                    {
                                        Id = h.Id,
                                        Name = h.Name,
                                        NumberOfSeats = h.NumberOfSeats,
                                        Location = h.Location
                                    })
                                    .FirstOrDefaultAsync();

            if (hall == null)
            {
                return NotFound();
            }

            return Ok(hall);
        }

    }

}

