using Azure;
using EmployeesProject.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

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

        public async Task<List<Position>> GetAllPositions()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Position>>("/api/position");
            return result ?? new List<Position>();
        }

        public async Task<Position?> GetPositionById(int id)
        {
            return await _httpClient.GetFromJsonAsync<Position>($"/api/position/{id}");
        }

        public async Task AddPosition(Position position)
		{
            await _httpClient.PostAsJsonAsync("/api/position", position);
            _navigatorManager.NavigateTo("positions");
        }

        public async Task UpdatePosition(int id, Position position)
        {
            await _httpClient.PutAsJsonAsync($"/api/position/{id}", position);
            _navigatorManager.NavigateTo("positions");
        }

        public async Task<string> DeletePosition(int id)
        {
            try
            {
                var result = await _httpClient.DeleteAsync($"/api/position/{id}");
            }
            catch (Exception)
            {
                return "Position cannot be deleted.";
            }
            
            _navigatorManager.NavigateTo("positions");
            return "";
        }

        public async Task<ServiceResponse<bool>> AddPositionsFromJson(string fileContent)
        {
            var response = new ServiceResponse<bool>();

            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/position/importFromJson", fileContent);

                var controllerResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
                if (controllerResponse == null || !controllerResponse.Success)
                {
                    if (controllerResponse != null && !controllerResponse.Hidden)
                    {
                        throw new Exception(controllerResponse.Message);
                    }
                    else
                    {
                        throw new Exception("Import failed. Please try again.");
                    }
                }
                response.Message = "Import successful.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            
            return response;
        }
    }
}
