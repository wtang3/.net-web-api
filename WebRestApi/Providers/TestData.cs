using System.Collections.Generic;
using WebRestApi.Models;
using WebRestApi.Interfaces;
using System.Linq;
using System;
using System.Web.Http;
using WebRestApi.Helpers;

namespace WebRestApi.Providers
{
    public class TestData : IDataRepository
    {
        static List<Employee> employees = new List<Employee>();

        public TestData()
        {
            if(employees.Count() == 0) {
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

                Employee employee5 = new Employee()
                {

                    Id = 5,
                    Name = "Sasha Nisimova",
                    Department = "Neuroscience"
                };

                Employee employee6 = new Employee()
                {

                    Id = 5,
                    Name = "Becky Smith",
                    Department = "Biology"
                };

                Employee employee7 = new Employee()
                {

                    Id = 6,
                    Name = "Dave Johnson",
                    Department = "Human Resources"
                };

                Employee employee8 = new Employee()
                {

                    Id = 7,
                    Name = "Tatyana Garcia",
                    Department = "Human Resources"
                };

                Employee employee9 = new Employee()
                {

                    Id = 8,
                    Name = "Karen Yang",
                    Department = "Physics"
                };

                Employee employee10 = new Employee()
                {

                    Id = 9,
                    Name = "Sophie Chihiro",
                    Department = "Aerospace"
                };

                employees.Add(employee1);
                employees.Add(employee2);
                employees.Add(employee3);
                employees.Add(employee4);
                employees.Add(employee5);
                employees.Add(employee6);
                employees.Add(employee7);
                employees.Add(employee8);
                employees.Add(employee9);
                employees.Add(employee10);
            }
        }

        /// <summary>
        /// Method for returning all employees
        /// </summary>
        /// <returns> Returns a list of Employee objects</returns>
        public ICollection<Employee> GetEmployees(string sort, int page, int pageSize, List<string> fields)
        {
            // TODO Data Shaping

            /*var test = new { "Id", "Department" };
            
            var temp = employees.ApplySort(sort)
                                      .Skip(pageSize * (page - 1))
                                      .Take(pageSize);

            
            var data = temp.Select(x => Helper.CreateDataShapingObject(temp, fields));
            
                                     

            List<Employee> data = new List<Employee>();
            data.Add((Employee) temp);*/
            return employees.ApplySort(sort)
                                      .Skip(pageSize * (page - 1))
                                      .Take(pageSize).ToList();
        }

        /// <summary>
        /// Gets a single employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a single Employee</returns>
        public Employee GetEmployee(int id)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="employee"></param>
        public Employee SetEmployee(int Id, string Name, string Department)
        {
            if (Id == 0)
            {
                var name = employees.Where(x => x.Name == Name);
                var employee = new Employee();
                if (!name.Any())
                {
                    Id = employees.Last().Id;
                    employee.Id = Id + 1;
                    employee.Name = Name;
                    employee.Department = Department;
                    employees.Add(employee);

                    return employee;
                }
            }
            else
            {
                return null;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="Name"></param>
        /// <param name="Department"></param>
        /// <returns></returns>
        public Employee UpdateEmployee(int Id, string Name, string Department)
        {
            if (Id !=0 )
            {
                var name = employees.Where(x => x.Id == Id);
                var employee = new Employee();
                if (name.Any())
                {
                    employee.Id = Id;
                    employee.Name = Name;
                    employee.Department = Department;
                    employees.Add(employee);

                    return employee;
                }
            }
            else
            {
                return null;
            }
            return null;
        }

        public bool DeleteEmployee(int id)
        {
            var employee = employees.FirstOrDefault(x => x.Id == id);
            if (employee != null){
                employees.Remove(employee);
                return true;
            }
            return false;
        }
    }
}