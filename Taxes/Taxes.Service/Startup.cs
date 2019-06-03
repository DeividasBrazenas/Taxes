using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OData.Edm;
using Taxes.Service.DataLayer;

namespace Taxes.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            
            services.AddDbContext<TaxesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc(routes =>
            {
                routes.Select().Expand().Filter().OrderBy().MaxTop(100).Count();
                routes.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routes.MapRoute("Default", "{controller}/{action=Index}/{id?}");
            });
        }

        private static IEdmModel GetEdmModel ()
        {
            var builder = new ODataConventionModelBuilder();

            builder.EntitySet<DataLayer.Models.Municipality>("Municipalities");
            builder.EntitySet<DataLayer.Models.Tax>("Taxes");

            var function = builder.Function("MunicipalityWithTax");
            function.ReturnsCollectionViaEntitySetPath<DataLayer.Models.Municipality>("Municipalities");

            return builder.GetEdmModel();
        }
    }
}
