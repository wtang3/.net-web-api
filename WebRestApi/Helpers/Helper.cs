using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}