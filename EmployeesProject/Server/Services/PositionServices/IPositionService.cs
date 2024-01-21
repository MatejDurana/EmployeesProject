using EmployeesProject.Shared.Models;

namespace EmployeesProject.Server.Services.PositionServices
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositions();
        Task<Position> CreatePosition(Position position);
        Task<Position?> GetPositionById(int id);
        Task<Position?> UpdatePosition(int id, Position position);
        Task<bool> DeletePosition(int id);
    }
}
