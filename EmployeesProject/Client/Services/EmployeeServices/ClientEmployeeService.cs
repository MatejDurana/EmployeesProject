using Azure;
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
                if (serviceResponse == null || !serviceResponse.Success)
                    throw new Exception("An error occurred while attempting to add the employee.");
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
                var serviceResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
                if (serviceResponse == null || !serviceResponse.Success)
                    throw new Exception("An error occurred while attempting to delete the employee.");
                return serviceResponse;
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
                var serviceResponse = await _httpClient.GetFromJsonAsync<ServiceResponse<List<Employee>>>("/api/employee");
                if (serviceResponse == null || !serviceResponse.Success)
                    throw new Exception("An error occurred while attempting to get all the employees.");
                return serviceResponse;
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
                var serviceResponse = await _httpClient.GetFromJsonAsync<ServiceResponse<Employee>>($"/api/employee/{employeeId}");
                if (serviceResponse == null || !serviceResponse.Success)
                    throw new Exception("An error occurred while attempting to get the employee.");
                return serviceResponse;
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

                Console.WriteLine(serviceResponse.Message);

                if (serviceResponse == null || !serviceResponse.Success)
                    throw new Exception("An error occurred while attempting to update the employee.");
                return serviceResponse;
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Employee> { Message = ex.Message, Success = false };
            }
        }
    }
}
