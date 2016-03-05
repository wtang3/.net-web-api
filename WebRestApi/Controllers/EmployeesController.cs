using System.Collections.Generic;
using System.Web.Http;
using WebRestApi.Models;
using WebRestApi.Interfaces;
using WebRestApi.Providers;

namespace WebRestApi.Controllers
{
    public class EmployeesController : ApiController
    {

        IDataRepository DataRepo = new TestData();

        public Employee Get(int id)
        {
            var employee = DataRepo.GetEmployee(id);

            return employee;
        }
        public List<Employee> GetAllEmployees()
        {

            return DataRepo.GetEmployees();
        }
    }
}
