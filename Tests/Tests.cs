using NUnit.Framework;
using WebRestApi.Providers;
using WebRestApi.Models;
using WebRestApi.Interfaces;
using System.Collections.Generic;
using System;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        IDataRepository TestRepo = new TestData();
        [SetUp]
        public void Setup() {
            Console.WriteLine("Setup");
        }

        [TearDown]
        public void TearDown() {
            Console.WriteLine("TearDown");
        }

        [TestCase]
        public void Employees_Are_Not_Empty()
        {
            // defaults
            string sort = null;
            int page = 1;
            int pageSize = 5;
            List<string> fields = new List<string>();

            var employees = TestRepo.GetEmployees(sort, page, pageSize, fields);
            Assert.IsNotEmpty(employees);
        }
        [TestCase(1)]
        public void Employees_Are_Not_Null(int id)
        {
            // defaults
            string sort = null;
            int page = 1;
            int pageSize = 5;
            List<string> fields = new List<string>();

            var employees = TestRepo.GetEmployees(sort, page, pageSize, fields);
            var employee = TestRepo.GetEmployee(id);
            Assert.IsNotNull(employees);
            Assert.IsNotNull(employee);
        }

        [TestCase]
        public void Test_Paging_Functionality()
        {
            string sort = null;
            int page = 2;
            int pageSize = 2;
            List<string> fields = new List<string>();

            var employees = TestRepo.GetEmployees(sort, page, pageSize, fields);
            Assert.That(employees.Count, Is.EqualTo(2));    
        }
    }
}
