
using Microsoft.EntityFrameworkCore;
using MinimalAPIsProtgress.Data;
using MinimalAPIsProtgress.Models;

namespace MinimalAPIsProtgress
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString = builder.Configuration.GetConnectionString("PostgreSqlConnection");
            builder.Services.AddDbContext<OfficeDb>(options =>
                options.UseNpgsql(connectionString));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapPost("/employees/", async (Employee e, OfficeDb db) =>
            {
                db.Employees.Add(e);
                await db.SaveChangesAsync();

                return Results.Created($"/employees/{e.Id}", e);


            });


            app.MapGet("/employees/{id:int}", async (int id , OfficeDb db) =>
            {

                return await db.Employees.FindAsync(id)
                    is Employee e
                    ? Results.Ok(e)
                    : Results.NotFound();
            });

            app.MapGet("employee", async (OfficeDb db) => await db.Employees.ToListAsync());


            app.MapPut("/employees{id:int}", async (int id, Employee e, OfficeDb db) =>
            {

                var employee = await db.Employees.FindAsync(id);
                if (employee is null) return Results.NotFound();

                employee.FirtsName = e.FirtsName;
                employee.LastName = e.LastName;
                employee.Branch = e.Branch;
                employee.Age = e.Age;

                await db.SaveChangesAsync();
                return Results.Ok(employee);
            });

            app.MapDelete("/employees/{id:int}", async (int id, OfficeDb db) =>
            {
                var employee = await db.Employees.FindAsync(id);

                if (employee is null) return Results.NotFound();

                db.Employees.Remove(employee);
                await db.SaveChangesAsync();

                return Results.NoContent();
            });
            app.Run();
        }
    }
}
