using EmployeesProject.Server.Data;
using EmployeesProject.Server.Services.IPAddressServices;
using EmployeesProject.Server.Services.PositionServices;
using EmployeesProject.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace EmployeesProject.Server.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;
        private readonly IPAddressService _ipAddressService;
        private readonly IPositionService _positionService;

        public EmployeeService(DataContext dataContext, IPAddressService ipAddressService, IPositionService positionService)
        {
            _context = dataContext;
            _ipAddressService = ipAddressService;
            _positionService = positionService;
        }

        public async Task<ServiceResponse<Employee>> AddEmployee(Employee employee)
        {
            var serviceResponse = new ServiceResponse<Employee>();
            try {

                if (await EmployeeExists(employee)) { 
                    serviceResponse.Hidden = false;
                    throw new Exception("Employee with these details already exists.");
                }

                var ipAddressServiceResponse = await _ipAddressService.GetCountryCodeFromIPAddress(employee.IPAddress);
                if(!ipAddressServiceResponse.Success) {
                    serviceResponse.Hidden = false;
                    //ipAddressServiceResponse.Message
                    throw new Exception("This IP address cannot be processed. Please try another.");
                }
                employee.IPCountryCode = ipAddressServiceResponse.Data!;

                _context.Employees.Add(employee);
                _context.Positions.Attach(employee.Position);
                await _context.SaveChangesAsync();

                serviceResponse.Data = employee;
                serviceResponse.Message = "Employee created successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(AddEmployee), ex.Message);
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
      
        public async Task<bool> EmployeeExists(Employee employee)
        {
             return await _context.Employees.AnyAsync(e =>
                 e.Name == employee.Name &&
                 e.Surname == employee.Surname &&
                 e.BirthDate == employee.BirthDate);
        }

        public async Task<ServiceResponse<List<Employee>>> GetAllEmployees()
        {
            var serviceResponse = new ServiceResponse<List<Employee>>();
            try
            {
                serviceResponse.Data = await _context.Employees.Include(e => e.Position).ToListAsync();
                serviceResponse.Message = "Employee data obtained successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(GetAllEmployees), ex.Message);
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            
            return serviceResponse;
        }

        public async Task<ServiceResponse<Employee>> GetEmployeeById(int employeeId)
        {
            var serviceResponse = new ServiceResponse<Employee>();
            try
            {
                var employee = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(e => e.Id == employeeId);
                if (employee == null)
                    throw new Exception($"Employee with id {employeeId} not found");

                serviceResponse.Data = employee;
                serviceResponse.Message = "Employee data obtained successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(GetEmployeeById), ex.Message);
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<Employee>> UpdateEmployee(Employee employee)
        {
            var serviceResponse = new ServiceResponse<Employee>();

            try
            {
                if (await EmployeeExists(employee))
                {
                    serviceResponse.Hidden = false;
                    throw new Exception("Employee with these details already exists.");
                }

                var dbEmployee = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(e => e.Id == employee.Id);
                if (dbEmployee == null)
                    throw new Exception($"Employee with Id '{employee.Id}' not found.");

                dbEmployee.Name = employee.Name;
                dbEmployee.Surname = employee.Surname;
                dbEmployee.BirthDate = employee.BirthDate;
                dbEmployee.IPAddress = employee.IPAddress;

                var ipAddressServiceResponse = await _ipAddressService.GetCountryCodeFromIPAddress(employee.IPAddress);
                if (!ipAddressServiceResponse.Success)
                {
                    serviceResponse.Hidden = false;
                    //ipAddressServiceResponse.Message
                    throw new Exception("This IP address cannot be processed. Please try another.");
                }
                dbEmployee.IPCountryCode = ipAddressServiceResponse.Data!;
         
                dbEmployee.PositionId = employee.Position.Id;

                await _context.SaveChangesAsync();

                serviceResponse.Data = dbEmployee;
                serviceResponse.Message = "Employee updated successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(UpdateEmployee), ex.Message);
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<bool>> DeleteEmployee(int id)
        {
            var serviceResponse = new ServiceResponse<bool>();

            try
            {
                var dbEmployee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                if (dbEmployee == null)
                    throw new Exception($"Employee with Id '{id}' not found.");

                _context.Employees.Remove(dbEmployee);

                await _context.SaveChangesAsync();

                serviceResponse.Data = true;
                serviceResponse.Message = "Employee deleted successfully.";
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(DeleteEmployee), ex.Message);
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }


        public async Task<ServiceResponse<bool>> AddEmployeesFromJson(string fileContent)
        {
            ServiceResponse<bool> serviceResponse = new ServiceResponse<bool>();
            List<Employee> employees = new List<Employee>();

            try
            {
                dynamic? employeeData = JsonConvert.DeserializeObject(fileContent);

                if(employeeData == null || employeeData.employees == null || employeeData.employees.Count == 0)
                {
                    serviceResponse.Hidden = false;
                    throw new Exception("Invalid or empty employee data.");
                }

                foreach (var employee in employeeData.employees)
                {
                    if (JsonEmployeeHasRequiredAttributes(employee))
                    {
                        employees.Add(new Employee()
                        {
                            Name = employee.Name,
                            Surname = employee.Surname,
                            BirthDate = DateTime.Parse(employee.BirthDate.ToString()), 
                            Position = new Position() { PositionName = employee.Position },
                            IPAddress = employee.IpAddress,
                        });
                    }
                    else
                    {
                        serviceResponse.Hidden = false;
                        throw new Exception("Invalid employee data format. Missing required attributes.");
                    }
                }

                List<Employee> filteredEmployees = new List<Employee>();

                foreach (var employee in employees)
                {
                    
                    if (!await EmployeeExists(employee))
                    {
                        int? positionId = await _positionService.PositionExists(employee.Position.PositionName);
                        if (positionId != null)
                        {
                            employee.PositionId = positionId;
                        }
                        employee.Position = null;

                        var ipAddressServiceResponse = await _ipAddressService.GetCountryCodeFromIPAddress(employee.IPAddress);
                        if (!ipAddressServiceResponse.Success)
                        {
                            serviceResponse.Hidden = false;
                            throw new Exception($"IP address {employee.IPAddress} cannot be processed.");
                        }
                        employee.IPCountryCode = ipAddressServiceResponse.Data!;

                        filteredEmployees.Add(employee);
                    }
                }

                
                _context.Employees.AddRange(filteredEmployees);
                _context.SaveChanges();

                serviceResponse.Message = $"Employee import was successful.";
                if(filteredEmployees.Count > 0)
                {
                    serviceResponse.Message += $" Added {filteredEmployees.Count} new employees.";
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "{Method}: {Message}", nameof(AddEmployeesFromJson), ex.Message);
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }
        public bool JsonEmployeeHasRequiredAttributes(dynamic employee)
        {
            return employee != null &&
                   employee.Name != null &&
                   employee.Surname != null &&
                   employee.BirthDate != null &&
                   employee.Position != null &&
                   employee.IpAddress != null;
        }
    }
}
