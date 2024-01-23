using EmployeesProject.Shared.Models;

namespace EmployeesProject.Server.Services.PositionServices
{
    public interface IPositionService
    {
        Task<List<Position>> GetAllPositions();
        Task<Position> AddPosition(Position position);
        Task<Position?> GetPositionById(int id);
        Task<Position?> UpdatePosition(int id, Position position);
        Task<bool> DeletePosition(int id);
        Task<ServiceResponse<bool>> AddPositionsFromJson(string fileContent);
        Task<int?> PositionExists(string positionName);
    }
}
