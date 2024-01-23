using EmployeesProject.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesProject.Server.Services.EmployeeServices
{
	public interface IEmployeeService
	{
		Task<ServiceResponse<List<Employee>>> GetAllEmployees();
		Task<ServiceResponse<Employee>> AddEmployee(Employee employee);
		Task<ServiceResponse<Employee>> GetEmployeeById(int employeeId);
		Task<ServiceResponse<Employee>> UpdateEmployee(Employee employee);
		Task<ServiceResponse<bool>> DeleteEmployee(int employeeId);
		Task<bool> EmployeeExists(Employee employee);
    }
}
