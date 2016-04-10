using System;
using System.Collections.Generic;
using System.Linq;
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