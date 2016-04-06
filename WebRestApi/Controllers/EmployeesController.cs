using System;
using System.Web.Http;
using WebRestApi.Providers;
using WebRestApi.Interfaces;
using WebRestApi.Helpers;
using WebRestApi.Models;
using System.Collections.Generic;

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
                if (employee.Name == null || employee.Department == null)
                {
                    return BadRequest("You gotta supply us something.");
                }
                else {
                    var result = _repository.SetEmployee(employee.Id, employee.Name, employee.Department);
                    if (result != null)
                    {
                        return Created(Request.RequestUri + "/", result);
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

        public IHttpActionResult Put([FromBody] Employee employee)
        {
            try
            {
                if (employee.Id == 0 )
                {
                    return BadRequest("You gotta supply us something.");
                }
                else {
                    var result = _repository.UpdateEmployee(employee.Id, employee.Name, employee.Department);
                    if (result != null)
                    {
                        return Ok(result);
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

        public IHttpActionResult Delete(int id)
        {
            try
            {
                var result = _repository.DeleteEmployee(id);
                if(result == true)
                {
                    return Ok(result);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return InternalServerError();
            }
        }

    }

    /*public class EmployeesController : ODataController
      {
           IDataRepository _dataRepo = new TestData();

           [EnableQuery]
           public IQueryable<Employee> Get()
           {
               return _dataRepo.GetEmployees();
           }
       }
    */
}
