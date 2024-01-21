using EmployeesProject.Shared.Models;

namespace EmployeesProject.Client.Services.PositionServices
{
	public interface IPositionService
	{
		Task<List<Position>> GetAllPositions();
		Task AddPosition(Position position);
		Task<Position?> GetPositionById(int id);
		Task UpdatePosition(int id, Position position);
		Task DeletePosition(int id);

    }
}
