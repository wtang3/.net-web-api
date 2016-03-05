using System.Collections.Generic;
using WebRestApi.Models;

namespace WebRestApi.Interfaces
{
    public interface IDataRepository
    {
        List<Employee> GetEmployees();
        Employee GetEmployee(int id);

        //TODO:  need to have return type.
        void PostEmployee(Employee employee);
        void PostEmployees(List<Employee> employees);
    }
}