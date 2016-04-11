using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Web;
using WebRestApi.Models;

namespace WebRestApi.Helpers
{
    public static class Helper
    {
        public static bool IsTypeInt(this object o)
        {
            switch (Type.GetTypeCode(o.GetType()))
            {
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        // TODO data shaping
        public static dynamic CreateDataShapingObject<T>(T data, List<string> fields)
        {
            if (!fields.Any())
            {
                return data;
            }
            else
            {
                IDictionary<string, object> dataShaped = new ExpandoObject();
                var type = data.GetType();
                foreach(var pair in fields.Select(x => new {
                    Name = x,
                    Property = type.GetProperty(x)
                }))
                {
                    dataShaped[pair.Name] = pair.Property.GetValue(data, new object[0]);

                }

                /*var dataShaped = new ExpandoObject();
                foreach (var field in fields)
                {
                    var fieldValue = data.GetType().GetProperty(field, BindingFlags.IgnoreCase |
                                                                       BindingFlags.Instance |
                                                                       BindingFlags.Public)
                                                   .GetValue(data, null);
                    ((IDictionary<string, object>) dataShaped).Add(field, fieldValue);
                }*/

                return dataShaped;
            }
        }

        public static object CreatePaginationObject<T>(ICollection<T> data,
                                                    System.Net.Http.HttpRequestMessage Request, 
                                                    string sort, 
                                                    int page, 
                                                    int pageSize, 
                                                    string fields)
        {
            var count = data.Count;
            var totalPages = (int)Math.Ceiling((double)count / pageSize);
            var urlHelper = new System.Web.Http.Routing.UrlHelper(Request);
            const int upperPageBound = 10;

            // cap the page size
            if (pageSize > upperPageBound)
            {
                pageSize = upperPageBound;
            }

            var previousLink = page > 1 ? urlHelper.Link("EmployeeList",
                new
                {
                    page = page - 1,
                    pageSize = pageSize,
                    sort = sort,
                    fields = fields
                }) : "";
            var nextLink = page < totalPages ? urlHelper.Link("EmployeeList",
                new
                {
                    page = page + 1,
                    pageSize = pageSize,
                    sort = sort,
                    fields = fields
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

            return pagination;
        }
    }
}