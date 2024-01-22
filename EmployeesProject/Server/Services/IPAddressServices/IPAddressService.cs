using EmployeesProject.Shared.Models;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

namespace EmployeesProject.Server.Services.IPAddressServices
{
    public class IPAddressService
    {
        private readonly HttpClient _httpClient;

        public IPAddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ServiceResponse<string>> GetCountryCodeFromIPAddress(string ipAddress)
        {
            string apiUrl = "https://api.country.is/" + ipAddress;
            ServiceResponse<string> serviceResponse = new ServiceResponse<string>();

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();


                    using (JsonDocument doc = JsonDocument.Parse(responseContent))
                    {
                        JsonElement root = doc.RootElement;

                        if (root.TryGetProperty("country", out JsonElement countryElement))
                        {
                            serviceResponse.Data = countryElement.GetString();
                        }
                        else
                        {
                            throw new Exception("The 'country' property is not found in the JSON.");
                        }
                    }
                }
                else
                {
                    throw new Exception($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
