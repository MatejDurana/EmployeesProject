using EmployeesProject.Shared.Models;
using Microsoft.AspNetCore.Components;
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
        private readonly NavigationManager _navigatorManager;

        public ClientEmployeeService(HttpClient httpClient, NavigationManager navigatorManager)
        {
            _httpClient = httpClient;
            _navigatorManager = navigatorManager;
        }

        public async Task<ServiceResponse<Employee>> AddEmployee(Employee employee)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/employee", employee);
                var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<Employee>>();
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Employee> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteEmployee(int employeeId)
        {
            try { 
                var result = await _httpClient.DeleteAsync($"api/employee/{employeeId}");
                return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<List<Employee>>> GetAllEmployees()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Employee>>>("/api/employee");
                return result;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<Employee>> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<Employee>> GetEmployeeById(int employeeId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ServiceResponse<Employee>>($"/api/employee/{employeeId}");
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Employee> { Message = ex.Message, Success = false };
            }
        }

        public async Task<ServiceResponse<Employee>> UpdateEmployee(Employee employee)
        {
            try { 
                var result =  await _httpClient.PutAsJsonAsync("/api/employee", employee);
                var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<Employee>>();
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Employee> { Message = ex.Message, Success = false };
            }
        }
    }
}
