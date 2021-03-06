﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebRestApi.Models;

namespace WebRestApi.Interfaces
{
    public interface IDataRepository
    {
        ICollection<object> GetEmployees(string sort, int page, int pageSize, List<string> fields);
        Employee GetEmployee(int id);
        Employee SetEmployee(int Id, string Name, string Department);
        Employee UpdateEmployee(int Id, string Name, string Department);
        bool DeleteEmployee(int id);
    }
}