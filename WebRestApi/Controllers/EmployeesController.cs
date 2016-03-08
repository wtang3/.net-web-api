using System;
using System.Web.Http;
using WebRestApi.Providers;
using WebRestApi.Interfaces;
using WebRestApi.Helpers;
using WebRestApi.Models;

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

        public IHttpActionResult GetAllEmployees()
        {
            try
            {
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
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Nice try, but this won't work.");
                }
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] Employee employee) {
            try
            {
                if(employee.Name == null || employee.Department == null)
                {
                    return BadRequest("You gotta supply us something.");
                }
                else {
                    var status = _repository.SetEmployee(employee.Name, employee.Department, employee.Id);
                    if (status)
                    {
                        return Ok("Success");
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }

            }
            catch (Exception)
            {
                return InternalServerError();
            }  
        }

    }
}
