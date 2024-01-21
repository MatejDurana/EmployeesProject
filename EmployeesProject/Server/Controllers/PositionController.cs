﻿using EmployeesProject.Server.Services.PositionServices;
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
		public async Task<ActionResult<Position>?> CreatePosition(Position position)
		{
			return Ok(await _positionService.CreatePosition(position));
		}
	}
}
