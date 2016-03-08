using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using WebRestApi.Models;

namespace WebRestApi.Interfaces
{
    public interface IDataRepository
    {
        List<Employee> GetEmployees();
        Employee  GetEmployee(int id);
        string SetEmployee(string Name, string Department, int id );
    }
}