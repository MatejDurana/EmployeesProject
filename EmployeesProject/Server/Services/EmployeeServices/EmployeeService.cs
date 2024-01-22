using EmployeesProject.Server.Data;
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

        public EmployeeService(DataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<ServiceResponse<Employee>> AddEmployee(Employee employee)
        {
            var serviceResponse = new ServiceResponse<Employee>();
            try { 
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
                serviceResponse.Data = await _context.Employees.Include(e => e.Position).FirstOrDefaultAsync(e => e.Id == employeeId);
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
                dbEmployee.IPCountryCode = employee.IPCountryCode;
         
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
