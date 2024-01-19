using EmployeesProject.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace EmployeesProject.Client.Services.EmployeeServices
{
    public class ClientEmployeeService : IEmployeeService
    {
        private readonly HttpClient _httpClient;

        public ClientEmployeeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public List<Employee> Employees { get; set; } = new List<Employee>();

        public async Task<List<Employee>> GetAllEmployees()
        {
            var result = await _httpClient.GetFromJsonAsync<List<Employee>>("api/employee");
            if (result is not null)
                Employees = result;
            return Employees;
        }
    }
}
