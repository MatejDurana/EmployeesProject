using EmployeesProject.Server.Services.PositionServices;
using EmployeesProject.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesProject.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PositionController : ControllerBase
	{
		private readonly IPositionService _positionService;

		public PositionController(IPositionService positionService)
        {
			_positionService = positionService;
		}

		[HttpGet]
		public async Task<ActionResult<List<Position>>> GetAll()
		{
			return Ok(await _positionService.GetAllPositions());
		}

		[HttpPost]
		public async Task<ActionResult<Position>> AddPosition(Position position)
		{
			return Ok(await _positionService.AddPosition(position));
		}

		[HttpGet("{id}")]
        public async Task<ActionResult<Position>> GetPositionById(int id)
        {
            return Ok(await _positionService.GetPositionById(id));
        }

		[HttpPut("{id}")]
		public async Task<ActionResult<Position>> UpdatePosition(int id, Position position)
        {
			var result = await _positionService.UpdatePosition(id, position);

            if (result == null)
			{
				return BadRequest(result);
			}
            return Ok(result);
        }
		
		[HttpDelete("{id}")]
		public async Task<ActionResult<bool>> DeletePosition(int id)
        {
			var result = await _positionService.DeletePosition(id);
			if(result == false)
			{
				return BadRequest();
			}
            return Ok();
        }

		[Route("importFromJson")]
		[HttpPost]
		public async Task<ActionResult<ServiceResponse<bool>>> AddPositionsFromJson([FromBody] string fileContent)
		{
            return Ok(await _positionService.AddPositionFromJson(fileContent));
		}

    }
}
