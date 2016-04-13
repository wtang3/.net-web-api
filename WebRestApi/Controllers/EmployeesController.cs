using System;
using System.Web.Http;
using WebRestApi.Providers;
using WebRestApi.Interfaces;
using WebRestApi.Helpers;
using WebRestApi.Models;
using System.Web;
using System.Linq.Dynamic;
using System.Collections.Generic;
using System.Linq;

namespace WebRestApi.Controllers
{
   

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        [Route("api/Employees", Name = "EmployeeList")]
        public IHttpActionResult GetAllEmployees(string sort="id", int page = 1, int pageSize = 5, string fields = null)
        {
            List<string> fieldList = new List<string>();

            fieldList = fields != null ? fieldList = fields.ToLower().Split(',').ToList() : null;

            try
            {
                var employees = _repository.GetEmployees(sort, page, pageSize, fieldList);
                var pagination = Helper.CreatePaginationObject(employees, Request, sort, page, pageSize, fields);
             
                HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(pagination));

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
}
