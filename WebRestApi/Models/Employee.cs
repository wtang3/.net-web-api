﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebRestApi.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public static explicit operator Employee(List<object> v)
        {
            throw new NotImplementedException();
        }
    }
}