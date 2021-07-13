using FoodOrderingSystem.Models.account;
using FoodOrderingSystem.Models.customerOrder;
using FoodOrderingSystem.Models.category;
using FoodOrderingSystem.Models.item;
using FoodOrderingSystem.Models.orderDetails;
using FoodOrderingSystem.Services.Implements;
using FoodOrderingSystem.Services.Interfaces;
using FoodOrderingSystem.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodOrderingSystem
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
            // config connection string
            DBUtils.ConnectionString = Configuration.GetConnectionString("MySql");
            services.AddControllersWithViews();
            services.AddDistributedMemoryCache();           // Đăng ký dịch vụ lưu cache trong bộ nhớ (Session sẽ sử dụng nó)
            services.AddSession(cfg => {                    // Đăng ký dịch vụ Session
                cfg.Cookie.Name = "gudfood";                // Đặt tên Session - tên này sử dụng ở Browser (Cookie)
                cfg.IdleTimeout = new TimeSpan(0, 60, 0);    // Thời gian tồn tại của Session
            });
            services.AddTransient<IAccountDAO, AccountDAO>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IItemDAO, ItemDAO>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<ICategoryDAO, CategoryDAO>();
            services.AddTransient<ICategoryService, CategoryService>();
            //services.AddTransient<ICustomerOrderDAO, CustomerOrderDAO>();
            //services.AddTransient<ICustomerOrderService, CustomerOrderService>();
            //services.AddTransient<IOrderDetailsDAO, OrderDetailsDAO>();
            //services.AddTransient<IOrderDetailsService, OrderDetailsService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
