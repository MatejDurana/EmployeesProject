using EmployeesProject.Client.Pages;
using EmployeesProject.Server.Data;
using EmployeesProject.Server.Services.IPAddressServices;
using EmployeesProject.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;


namespace EmployeesProject.Server.Services.EmployeeServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DataContext _context;
        private readonly IPAddressService _ipAddressService;

        public EmployeeService(DataContext dataContext, IPAddressServices.IPAddressService ipAddressService)
        {
            _context = dataContext;
            _ipAddressService = ipAddressService;
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
                var dbCharacters = await _context.Employees.Include(e => e.Position).ToListAsync();
                serviceResponse.Data = dbCharacters;
                serviceResponse.Message = "Employee data obtained successfully.";
            }
            catch (Exception ex)
            {
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
                serviceResponse.Data = false;
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }

            return serviceResponse;
        }

    }
}
