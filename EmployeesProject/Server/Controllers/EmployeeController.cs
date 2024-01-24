using Azure;
using EmployeesProject.Server.Services.EmployeeServices;
using EmployeesProject.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeesProject.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<Employee>>>> GetAll()
        {
            var response = await _employeeService.GetAllEmployees();
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Employee>>> AddEmployee(Employee employee)
        {
            var response = await _employeeService.AddEmployee(employee);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Employee>>> GetEmployeeById(int id)
        {
            var response = await _employeeService.GetEmployeeById(id);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Employee>>> UpdateEmployee(Employee employee)
        {
            var response = await _employeeService.UpdateEmployee(employee);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteEmployee(int id)
        {
            var response = await _employeeService.DeleteEmployee(id);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

        [Route("importFromJson")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<bool>>> AddEmployeesFromJson([FromBody] string fileContent)
        {
            var response = await _employeeService.AddEmployeesFromJson(fileContent);
            return response.Success ? Ok(response) : StatusCode(500, response);
        }

    }
}
