

// This solution uses Etity Fraework - Database First Approach
// This means that clases are created from an existing database
// In order to generate these classes, follows these stepes:
// 1- Open Package Manager Console
// 2- Execute the following command:
//  for MySQQL:;
//     //Scaffold-DbContext "server=localhost;port=3306;user=root;password=qpwoeiruty;database=SCANNING_SERVICES" Pomelo.EntityFrameworkCore.MySql -OutputDir MySQLEntities -Context ScanningDBContext -f
//     ( replace the information that correspond to the database)
//     The command wil lgenerate a folder called "Entities" where all the classes.
//     The command also generates a context file called "FinderDBContext" that is used to references all these classes under the "Entities" folder
//     Any change in the Finder Database will requires to re-run this command
//     The -f Flag at the end of the command forces to re-write the entities classess
//  For MS-SQL:
//     Scaffold-DbContext "Server=cdlalo4\SQLExpress;Database=SCANNING_SERVICES;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir MSSQLEntities -Context "ScanningDBContext" -f
// 3- Hit Enter
///
/// To get the Connection String from a System variable .. take a look at this article:
/// https://www.benday.com/2017/12/20/ef-core-asp-net-core-read-connections-strings-from-environment-variables/
// Sample link of how to perfor XML serialization of an Object
// https://codehandbook.org/c-object-xml/
/// Notes:
/// REQUIRED AT THE TIME THIS PACKAGE IS DELIVERED .. THE CONECTION STRING MUST BE EXTRACTED FROM (ScanningServicesDataObjects.GlobalVars.connectionString VARIABLE
/// 1- To get the Connection String from a System variable .. take a look at this article:
/// https://www.benday.com/2017/12/20/ef-core-asp-net-core-read-connections-strings-from-environment-variables/
/// The Context Db Class must be edited and replace the exiting OnConfiguring Method for the one below:
/// 
//      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//      {
//          optionsBuilder.UseSqlServer(ScanningServicesDataObjects.GlobalVars.connectionString);
//      }
// For testing proposes use: optionsBuilder.UseSqlServer(@"Server=CDNPRDDB2;Database=SCANNING_SERVICES;User Id=sa;Password=qpwoeiruty;");
//
// 2- The "AddEnvironmentVariables" in the Startup method, allow us to declare an System Environment Variable called
// ConnectionStrings__default (using 2 "_"), to extracyt the Database Connection String from the System Variable
// Anything declare on the system variable will replace the information obtained from the json application file
// 3- windows Services Implementation that use scheduled tasks. I found this on the internet:
//    https://www.hanselman.com/blog/HowToRunBackgroundTasksInASPNET.aspx
//    https://www.quartz-scheduler.net/index.html
//    Comparison between Hangfire and Quartz.Net : http://codingsight.com/hangfire-task-scheduler-for-net/#briefreviewofquartznet
//    Install Quartz as windoiws serviec: http://geekswithblogs.net/TarunArora/archive/2012/11/16/install-quartz.net-as-a-windows-service-and-test-installation.aspx
//    Using Crom Expresion with Quartz https://www.infoworld.com/article/3078781/application-development/how-to-work-with-quartz-net-in-c.html
//                                     https://www.quartz-scheduler.net/documentation/quartz-2.x/tutorial/crontriggers.html
//    Variety of posts: http://geekswithblogs.net/TarunArora/category/13586.aspx
// 4 - Article about CRON Expresions
// https://en.wikipedia.org/wiki/Cron#CRON_expression
// 5- Using XSLT to rendeder XML files in HTML format using Visual Studio: https://www.youtube.com/watch?v=4mUcfUEvS0g

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;

using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using System.Text;
using NLog;

namespace ScanningServices
{
    public class Startup
    {
        
        /// <summary>
        /// 
        /// </summary>
        public class Error
        {
            /// <summary>
            /// 
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Stacktrace { get; set; }
        }

        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //Getting teh connection string from appsettings.json file
            ScanningServicesDataObjects.GlobalVars.connectionString = Configuration.GetConnectionString("Default");
            logger.Trace("Scanning Serivices API Initialiation ...");
            logger.Trace("Database Connection String: " + ScanningServicesDataObjects.GlobalVars.connectionString);

            // Added to support Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Scanning Service APIs",
                    Description = "These are APIs to Manage a Scanning Services Environment." + "<br>" +
                                  "To visualize the APIs result, use the Json Viewer: https://codebeautify.org/jsonviewer",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Leonardo Carbone", Email = "lcarbone@cdlac.com", Url = "http://www.cdlac.com" },
                    License = new License { Name = "Use under COMPU-DATA International LLC License.", Url = "https://example.com/license" }
                });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "SwaggerUi.XML");
                c.IncludeXmlComments(xmlPath);

            });

            // Add framework services.
            //services.AddDbContext<FinderDB.Entities.FinderDBContext>(options =>
            //    options.UseMySql(Configuration.GetConnectionString("MySQLConnection")));
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            try
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseExceptionHandler(
                     builder =>
                     {
                         builder.Run(
                         async context =>
                         {
                             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                             context.Response.ContentType = "application/json";
                             var ex = context.Features.Get<IExceptionHandlerFeature>();
                             if (ex != null)
                             {
                                 var err = JsonConvert.SerializeObject(new Error()
                                 {
                                     Stacktrace = ex.Error.StackTrace,
                                     Message = ex.Error.Message
                                 });
                                 await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(err), 0, err.Length).ConfigureAwait(false);
                             }
                         });
                     }
                );

                app.UseMvc();

                //Added to spport Swagger
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //c.RoutePrefix = "help";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scanning Services APIs V1");
                    c.DocExpansion("none");
                    //c.InjectStylesheet("../css/swagger.min.css");
                });
            }
            catch (Exception ex)
            {
                app.Run(
                  async context =>
                  {
                      context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                      context.Response.ContentType = "application/json";
                      if (ex != null)
                      {
                          var err = JsonConvert.SerializeObject(new Error()
                          {
                              Stacktrace = ex.StackTrace,
                              Message = ex.Message
                          });
                          await context.Response.Body.WriteAsync(Encoding.ASCII.GetBytes(err), 0, err.Length).ConfigureAwait(false);
                      }
                  });
            }
        }
    }
}
