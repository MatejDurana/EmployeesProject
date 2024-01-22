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
            return Ok(await _employeeService.GetAllEmployees());
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<Employee>>> AddEmployee(Employee employee)
        {
            return Ok(await _employeeService.AddEmployee(employee));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<Employee>>> GetEmployeeById(int id)
        {
            return Ok(await _employeeService.GetEmployeeById(id));
        }

        [HttpPut]
        public async Task<ActionResult<ServiceResponse<Employee>>> UpdateEmployee(Employee employee)
        {
            return Ok(await _employeeService.UpdateEmployee(employee));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteEmployee(int id)
        {
            return Ok(await _employeeService.DeleteEmployee(id));
        }


    }
}
