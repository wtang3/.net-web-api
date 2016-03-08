using System;
using System.Web.Http;
using WebRestApi.Providers;
using WebRestApi.Interfaces;
using WebRestApi.Helpers;

namespace WebRestApi.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeesController : ApiController
    {

        IDataRepository _repository;

        public EmployeesController()
        {
            _repository = new TestData();
        }

        public EmployeesController(IDataRepository repository)
        {
            _repository = repository;
        }

        public IHttpActionResult GetAllEmployees() {
            try {
                var employees = _repository.GetEmployees();
                
                if(employees != null)
                {
                    return Ok(employees);
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }

            return InternalServerError();
        }

        public IHttpActionResult Get(int id)
        {
            try
            {
                if (id.IsTypeInt() && id > 0)
                {
                    var employees = _repository.GetEmployee(id);

                    if (employees != null)
                    {
                        return Ok(employees);
                    }
                    else
                    {
                        return BadRequest("I don't think that exists.");
                    }
                }
                else
                {
                    return BadRequest("Nice try, but this won't work");
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        /*
        [Route("api/Employees/SetEmployee")]
        public string SetEmployee(string Name, string Department) {
            int id = 0; // TODO;
            var data = _repository.SetEmployee(Name, Department, id);
            return data;
        }*/

    }
}
