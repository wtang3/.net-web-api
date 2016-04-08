﻿using System;
using System.Web.Http;
using WebRestApi.Providers;
using WebRestApi.Interfaces;
using WebRestApi.Helpers;
using WebRestApi.Models;
using System.Web;
using System.Linq.Dynamic;

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

        [Route("api/Employees", Name = "EmployeeList")]
        public IHttpActionResult GetAllEmployees(string sort="id", int page = 1, int pageSize = 5)
        {
            try
            {
                var employees = _repository.GetEmployees();
                var count = employees.Count;
                var totalPages = (int)Math.Ceiling((double)count / pageSize);
                var urlHelper = new System.Web.Http.Routing.UrlHelper(Request);
                const int upperPageBound = 10;

                // cap the page size
                if(pageSize > upperPageBound)
                {
                    pageSize = upperPageBound;
                }

                var previousLink = page > 1 ? urlHelper.Link("EmployeeList",
                    new
                    {
                        page = page - 1,
                        pageSize = pageSize,
                        sort = sort
                    }) : "";
                var nextLink = page < totalPages ? urlHelper.Link("EmployeeList",
                    new
                    {
                        page = page + 1,
                        pageSize = pageSize,
                        sort = sort
                    }) : "";

                var pagination = new
                {
                    currentPage = page,
                    pageSize = pageSize,
                    totalCount = count,
                    totalPages = totalPages,
                    previousPageLink = previousLink,
                    nextPageLink = nextLink
                };

                HttpContext.Current.Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(pagination));

                if(employees != null)
                {
                    return Ok(employees.ApplySort(sort).Skip(pageSize * (page - 1)).Take(pageSize));
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
