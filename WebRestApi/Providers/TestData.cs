﻿using System.Collections.Generic;
using WebRestApi.Models;
using WebRestApi.Interfaces;
using System.Linq;
using System;
using System.Web.Http;

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

                employees.Add(employee1);
                employees.Add(employee2);
                employees.Add(employee3);
                employees.Add(employee4);
            }
        }

        /// <summary>
        /// Method for returning all employees
        /// </summary>
        /// <returns> Returns a list of Employee objects</returns>
        public List<Employee> GetEmployees() {
            return employees;
        }

        /// <summary>
        /// Gets a single employee
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Returns a single Employee</returns>
        Employee IDataRepository.GetEmployee(int id)
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
        public string SetEmployee(string Name, string Department, int id=-1)
        {
            if (id != -1)
            {
                var name = employees.Where(x => x.Name == Name);
                var employee = new Employee();
                if (!name.Any())
                {
                    id = employees.Last().Id;
                    employee.Id = id + 1;
                    employee.Name = Name;
                    employee.Department = Department;
                    employees.Add(employee);

                    return "Success";
                }
            }
            else
            {
                var name = employees.Where(x => x.Name == Name);
                var employee = new Employee();
                if (!name.Any())
                {
                    employee.Id = id;
                    employee.Name = Name;
                    employee.Department = Department;
                    employees.Add(employee);

                    return "Success";
                }
            }
            return "Request Failed";
        }
    }
}