using EmployeesProject.Server.Data;
using EmployeesProject.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EmployeesProject.Server.Services.PositionServices
{
    public class PositionService : IPositionService
	{
		private readonly DataContext _context;

		public PositionService(DataContext dataContext)
        {
			_context = dataContext;
		}
        public async Task<Position> AddPosition(Position position)
		{
			_context.Add(position);
			await _context.SaveChangesAsync();
			return position;
		}

        public async Task<ServiceResponse<bool>> AddPositionsFromJson(string fileContent)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            List<Position> positions = new List<Position>();

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(fileContent))
                {
                    JsonElement root = doc.RootElement;

                    if (root.TryGetProperty("positions", out JsonElement positionsElement))
                    {
                        positions = positionsElement.EnumerateArray().Select(x => new Position() { PositionName = x.GetString() }).Distinct().ToList();
                    }
                    else
                    {
                        serviceResponse.Hidden = false;
                        throw new Exception("The 'positions' parameter is not found or is not an array in the JSON file.");
                    }
                }

                List<Position> filteredPositions = new List<Position>();

                foreach (var position in positions)
                {
                    if(await PositionExists(position.PositionName) == null)
                    {
                        filteredPositions.Add(position);
                    }
                }

                _context.Positions.AddRange(filteredPositions);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

        public async Task<int?> PositionExists(string positionName)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(e => e.PositionName == positionName);

            return position?.Id;
        }

        public async Task<bool> DeletePosition(int id)
        {
            var dbPosition = await _context.Positions.FindAsync(id);
            if (dbPosition == null)
            {
				return false;
            }

            bool hasEmployees = await _context.Employees.AnyAsync(e => e.PositionId == dbPosition.Id);
            if (hasEmployees)
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
			var result = await _context.Positions.FindAsync(id);
			if (result != null) { 
				return result;
			}
			throw new Exception("Position not found");
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
