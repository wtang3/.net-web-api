﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using WebRestApi.Models;

namespace WebRestApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.MapHttpAttributeRoutes();

            /*
            For OData Purposes
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Employee>("Employees");
       
            config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());*/
 
            // Web API routes
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Return json in the body. but also support xml. This will not indent the json objects/
            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // Removes all support for xml, which will help you beautify the json below.
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear();

            // Make the json look pretty.
            config.Formatters.JsonFormatter.SerializerSettings.Formatting 
                = Newtonsoft.Json.Formatting.Indented;

            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver 
                = new CamelCasePropertyNamesContractResolver();

            // Cache Handler
            config.MessageHandlers.Add(new CacheCow.Server.CachingHandler(config));
        }
    }
}
