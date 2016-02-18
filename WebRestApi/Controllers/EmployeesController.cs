using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebRestApi.Models;

namespace WebRestApi.Controllers
{
    public class EmployeesController : ApiController
    {
        List<Employee> employees = new List<Employee>();

        Employee employee1 = new Employee()
        {
            Id = 1,
            Name = "Will Tang",
            Department = "Computer Science"
        };

        Employee employee2 = new Employee()
        {
            Id = 2,
            Name = "John Do",
            Department = "Physics"
        };

        Employee employee3 = new Employee()
        {
            Id = 3,
            Name = "Daniel Kim",
            Department = "Chemistry"
        };

        Employee employee4 = new Employee()
        {
            Id = 4,
            Name = "Raj Kumar",
            Department = "Engineering"
        };

        public EmployeesController()
        {
            employees.Add(employee1);
            employees.Add(employee2);
            employees.Add(employee3);
            employees.Add(employee4);
        }

        public Employee Get(int id)
        {
            var employee = employees.Where(x => x.Id == id);
            if (employee.Any())
            {
                return new Employee()
                {
                    Id = employee.Select(x => x.Id).Single(),
                    Name = employee.Select(x => x.Name).Single(),
                    Department = employee.Select(x => x.Department).Single()
                };
            }
            return null;
        }
        public List<Employee> GetAllEmployees()
        {
            return employees;
        }
    }
}
