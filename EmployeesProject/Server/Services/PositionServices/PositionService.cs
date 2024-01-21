using EmployeesProject.Server.Data;
using EmployeesProject.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeesProject.Server.Services.PositionServices
{
	public class PositionService : IPositionService
	{
		private readonly DataContext _context;

		public PositionService(DataContext dataContext)
        {
			_context = dataContext;
		}
        public async Task<Position> CreatePosition(Position position)
		{
			_context.Add(position);
			await _context.SaveChangesAsync();
			return position;
		}

		public async Task<List<Position>> GetAllPositions()
		{
			return await _context.Positions.ToListAsync();
		}
	}
}
