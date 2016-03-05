﻿using NUnit.Framework;
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
        public void Employee_Is_Correct_Type()
        {
            var employees = TestRepo.GetEmployees();
            Assert.IsInstanceOf(typeof(List<Employee>), employees);
        }
        [TestCase(1)]
        public void Employees_Are_Not_Null(int id)
        {
            var employees = TestRepo.GetEmployees();
            var employee = TestRepo.GetEmployee(id);
            Assert.IsNotNull(employees);
            Assert.IsNotNull(employee);
        }

        //[TestCase("1")]
        //public void Employee
    }
}
