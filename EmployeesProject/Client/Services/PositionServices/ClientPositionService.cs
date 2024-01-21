using EmployeesProject.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;

namespace EmployeesProject.Client.Services.PositionServices
{
	public class ClientPositionService : IPositionService
	{
        private readonly HttpClient _httpClient;
        private readonly NavigationManager _navigatorManager;

        public ClientPositionService(HttpClient httpClient, NavigationManager navigatorManager)
        {
            _httpClient = httpClient;
            _navigatorManager = navigatorManager;
        }
        public async Task AddPosition(Position position)
		{
            await _httpClient.PostAsJsonAsync("/api/position", position);
            _navigatorManager.NavigateTo("positions");
        }

        public async Task DeletePosition(int id)
        {
            await _httpClient.DeleteAsync($"/api/position/{id}");
            _navigatorManager.NavigateTo("positions");
        }

        public async Task<List<Position>> GetAllPositions()
		{
            var result = await _httpClient.GetFromJsonAsync<List<Position>>("api/position");
            return result ?? new List<Position>();
        }

        public async Task<Position?> GetPositionById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Position>($"/api/position/{id}");
        }

        public async Task UpdatePosition(int id, Position position)
        {
            await _httpClient.PutAsJsonAsync($"/api/position/{id}", position);
            _navigatorManager.NavigateTo("positions");
        }
    }
}
