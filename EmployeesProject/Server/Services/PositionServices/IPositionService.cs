using EmployeesProject.Shared.Models;

namespace EmployeesProject.Server.Services.PositionServices
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositions();
        Task<Position> CreatePosition(Position position);
    }
}
