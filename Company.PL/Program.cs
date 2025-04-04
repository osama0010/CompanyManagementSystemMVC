using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Xml;
using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Contexts;
using Company.DAL.Models;
using Company.PL.Helpers;
using Company.PL.MappingProfiles;
using Company.PL.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using NuGet.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Company.PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var Builder = WebApplication.CreateBuilder(args);

            #region Configure Services that Allow Dependency Injection
            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<CompanyAppDbContext>(options =>
            {
                options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            }); // Allow Dependency Injection

            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>(); // Allow Dependency Injection for class DepartmentRepo
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new EmployeeProfile(), new UserProfile(), new RoleProfile() }));

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
            })
                        .AddEntityFrameworkStores<CompanyAppDbContext>()
                        .AddDefaultTokenProviders(); ;

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(Options =>
            {
                Options.LoginPath = "Account/Login";
                Options.AccessDeniedPath = "Home/Error";
            });

            Builder.Services.AddTransient<IEmailSettings, EmailSettings>();

            Builder.Services.Configure<MailSettings>(Builder.Configuration.GetSection("MailSettings"));

            #endregion

            var app = Builder.Build(); //Kestrel

            #region Configure Http Request Pipelines "Middlewares"
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
            #endregion

            app.Run();
        }

        #region How to send an email by Emailkit
        //1— add email settings in appsettings(Email, DisplayName , Password, Host, Port)
        //2— add new folder(settings) , add mailSettings class
        //3— add it in program.cs in services
        //4— install packages mailkit, mimekit
        //5— in helpers folder add interface IEmailSettings have signature method that sendmail
        //6— in helpers folder add new class called EmailSettings that impelement the interface and implement the method
        //7— allow dependency injection to IEmailSettings, EmailSettings
        //8— add it in account controller
        #endregion

        #region How to send sms by twilio
        //1— create account xn twilio
        //2—add sms settings in appsettings(AccountSId , AuthToken , twilioPhoneNumber)
        //3— add smsMessage model include PhoneNumber , Body
        //4— in settings folder add new class TwiotoSettings include(AccountSId , AuthToken , twilioPhoneNumber)
        //5- in program.cs add it in services
        //6— install twilio package
        #endregion

        #region Steps to inpuenent ajax search
        //1— open employee index page
        //2— open employee controller , add search action that return partial view
        //3—in employee in employeepartialvieus add partial view and add the table
        //4— in Layout add jquery link before render body
        //5—add js code in employee index page
        #endregion


    }
}
