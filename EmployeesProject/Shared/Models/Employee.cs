using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesProject.Shared.Models
{
    public class IPAddressAttribute : RegularExpressionAttribute
    {
        private const string IPAddressPattern = @"^(\d{1,3}\.){3}\d{1,3}$|^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|[0-9a-fA-F]{1,4}(:[0-9a-fA-F]{1,4}){1,6})$";

        public IPAddressAttribute() : base(IPAddressPattern)
        {
            ErrorMessage = "Please enter a valid IP address.";
        }
    }

    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Employee name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Employee surname is required")]
        public string Surname { get; set; }

        [Required(ErrorMessage ="Employee birth date is required")]
        [Range(typeof(DateTime), "1900-01-01", "2100-12-31", ErrorMessage = "The birth date must be between {1} and {2}.")]
        public DateTime BirthDate { get; set; }


        [Required(ErrorMessage = "Employee position is required")]
        public int? PositionId { get; set; }

        public Position Position { get; set; }

        [Required(ErrorMessage = "Employee IP address is required")]
        [IPAddress]
        public string IPAddress { get; set; }

        public string IPCountryCode { get; set; }
    }
}
