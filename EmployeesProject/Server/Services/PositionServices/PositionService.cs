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

        public async Task<bool> DeletePosition(int id)
        {
            var dbPosition = await _context.Positions.FindAsync(id);
            if (dbPosition == null)
            {
				return false;
            }
            _context.Remove(dbPosition);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Position>> GetAllPositions()
		{
			return await _context.Positions.ToListAsync();
		}

        public async Task<Position?> GetPositionById(int id)
        {
			return await _context.Positions.FindAsync(id);
        }

        public async Task<Position?> UpdatePosition(int id, Position position)
        {
			var dbPosition = await _context.Positions.FindAsync(id);
			if(dbPosition != null)
			{
				dbPosition.PositionName = position.PositionName;
				await _context.SaveChangesAsync();
			}
			return dbPosition;
        }
    }
}
