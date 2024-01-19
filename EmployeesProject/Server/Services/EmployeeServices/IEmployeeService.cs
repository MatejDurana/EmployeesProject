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
		List<Employee> Employees { get; set; }
		Task<List<Employee>> GetAllEmployees();
	}
}
