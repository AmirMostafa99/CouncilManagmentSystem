using CouncilsManagmentSystem.DTOs;
using CouncilsManagmentSystem.Models;
using CouncilsManagmentSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CouncilsManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouncilsController : ControllerBase
    {
        private readonly ICouncilsServies _councilServies;

        public CouncilsController(ICouncilsServies councilServies)
        {
            _councilServies = councilServies;
        }
        [Authorize]
        [Authorize(Policy = "RequireAddCouncilPermission")]
        [HttpPost(template: "CreateCouncil")]
        public async Task<IActionResult> createcouncil(AddCouncilsDTO DTO)
        {
            if (ModelState.IsValid)
            {
                var council = new Councils {
                    Title = DTO.Title,
                    Date = DTO.Date,
                    HallId = DTO.HallId,
                    TypeCouncilId = DTO.TypeCouncilId
                };
                var councilres = await _councilServies.AddCouncil(council);
                return Ok(councilres);

            }
            return BadRequest("you have wrong in your data. ");
        }

        [HttpGet(template: "GetAllCouncils")]
        public async Task<IActionResult> getallcouncils()
        {
            var councils = await _councilServies.GetallCouncils();
            return Ok(councils);
        }

        [HttpGet(template: "GetCouncilById")]
        public async Task<IActionResult> getcouncilbyid(int id)
        {
            var council=await _councilServies.GetCouncilById(id);
            return Ok(council);
        }

        [HttpGet(template:"GetAllCouncilsByIdType")]
        public async Task<IActionResult> getallcouncilsbyidtype(int typeid)
        {
            if(ModelState.IsValid)
            {
                var councils =await _councilServies.GetAllCouncilsByIdType(typeid);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }

        [HttpGet(template:"GetAllCouncilsByIdHall")]
        public async Task<IActionResult> getalcouncilsbyidhall(int hallid)
        {
            if (ModelState.IsValid)
            {
                var councils=await _councilServies.GetCouncilSbyIDhalls(hallid);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }

        [HttpGet(template:"GetAllCouncilByname")]
        public async Task<IActionResult> getallcouncilsbyname (string name)
        {
            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetCouncilsByTitle(name);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }

        [HttpGet(template:"GetCouncilBydate")]
        public async Task<IActionResult> getCouncilbydate(DateTime date)
        {
            if (ModelState.IsValid)
            {
                var councils = await _councilServies.GetCouncilByDate(date);
                return Ok(councils);
            }
            return BadRequest("you have wrong in your data. ");
        }


        [Authorize]
        [Authorize(Policy = "RequireEditCouncilPermission")]
        [HttpPut(template:"UpdateCouncil")]
        public async Task<IActionResult> updatecouncil (int id , AddCouncilsDTO DTO)
        {
            var council=await _councilServies.GetCouncilById(id);
            if(council==null)
            {
                return BadRequest("This id is not vaild");
            }
            council.Title = DTO.Title;
            council.Date = DTO.Date;
            council.TypeCouncilId = DTO.TypeCouncilId;
            council.HallId = DTO.HallId;
            var councilres = await _councilServies.UpdateCouncil(council);
            return Ok(councilres);
        }




    }
}
