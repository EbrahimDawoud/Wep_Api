
using lab2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace lab2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string test = "";
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ITIContext>(options =>
                           options.UseSqlServer(builder.Configuration.GetConnectionString("iticon")));
            //builder.Services.AddControllers().AddNewtonsoftJson(options =>
            //               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize);
            builder.Services.AddCors(opt=>
            {
                opt.AddPolicy(test, policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(opt=>opt.DefaultAuthenticateScheme="Scheme")
                .AddJwtBearer("Scheme", op =>
                    {
                        string keyData = "hello from the other side a7aaaaaaaa";
                        var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(keyData));

                        op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                        {
                            IssuerSigningKey = secretKey,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                        };  
                    }
                
                
                );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseCors(test);


            app.MapControllers();

            app.Run();
        }
    }
}
