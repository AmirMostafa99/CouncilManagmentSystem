using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CollageController : ControllerBase
    {
        private readonly ICollageServies _collageServies;

        public CollageController(ICollageServies collageServies)
        {
            _collageServies = collageServies;
        }
        [Authorize]
        //get all collages
        [HttpGet("GetAllCollages")]
        public async Task<IActionResult> getAllcollages()
        {
            var collages = await _collageServies.getAllcollages();
            return Ok(collages);
        }
        [Authorize]
        //get collage by id
        [HttpGet("GetCollageBy{id}")]
        public async Task<IActionResult> getByid(int id)
        {
            var collage = _collageServies.GetCollageByid(id);
            return Ok(collage);
        }
        //add collages in db
        [Authorize]
        [Authorize(Policy = "RequireAddCollagePermission")]
        [HttpPost("AddCollage")]
        public async Task<IActionResult> AddCollage([FromBody] AddCollageDTO dto)
        {
            if (ModelState.IsValid)
            {
                var collage = new Collage { Name = dto.collage_name };
                await _collageServies.Addcollages(collage);
                return Ok(collage);
            }
            return BadRequest("you have wrong in your data. ");
        }
        [Authorize]
        [Authorize(Policy = "RequireAddCollagePermission")]
        //update collages
        [HttpPut("UpdateCollage{id}")]
        public async Task<IActionResult> updateCollage(UpdateCollageDTO dto)
        {
            if (ModelState.IsValid)
            {
                var collage = await _collageServies.GetCollageByid(dto.id);
                if (collage == null)
                {
                    return NotFound("Not found this collage");

                }
                collage.Name = dto.collage_name;
                _collageServies.updatecollage(collage);
                return Ok(collage);

            }
            return BadRequest("you have wrong in your data. ");
        }
        ////delete collage
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> deleteCollage(int id)
        //{
        //    var collage=await _collageServies.GetCollageByid(id);
        //    if(collage==null)
        //    {
        //        return NotFound("Not found this collage");
        //    }
        //    _collageServies.Deletecollage(collage);
        //    return Ok(collage);
        //}

    }

}
