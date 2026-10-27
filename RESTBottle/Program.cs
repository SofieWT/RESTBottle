using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RESTBottle.Interfaces;
using RESTBottle.repostory;
using RESTBottle.Service;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#region add CORS policy
//CORS:
const string allowAllCORSPolicy = "AllowAll";//This is a constant string that defines the name of the CORS policy. It is used to identify the policy when configuring CORS in the application. The name "AllowAll" indicates that this policy will allow all origins, methods, and headers for cross-origin requests.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowAllCORSPolicy,
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                              });//man kan lave flere policies på en gang, men her har vi kun lavet en policy, som tillader alle origins, metoder og headers. Dette er nyttigt for at tillade adgang til API'et fra forskellige domæner eller klienter.
});//allowmethods is used to allow all HTTP methods (GET, POST, PUT, DELETE, etc.) from any origin. This is useful for APIs that need to be accessed from different domains or clients.
//    options.AddPolicy(name: "zealandCorsPolicy",
//        policy =>
//        {
//            policy.WithOrigins("https://www.zealand.dk/").AllowAnyMethod().AllowAnyHeader();
//        });
//});//allowmethods is used to allow all HTTP methods (GET, POST, PUT, DELETE, etc.) from any origin. This is useful for APIs that need to be accessed from different domains or clients.
////end of CORS
#endregion


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();//This makes the API endpoints available for Swagger to document and test. It generates metadata about the API endpoints, which is used by Swagger to create interactive documentation and testing interfaces.
builder.Services.AddSwaggerGen();//This adds the Swagger generator to the service collection, which generates Swagger documentation for the API based on the controllers and their actions. It allows developers to explore and test the API endpoints through a web-based interface.



#region dependency injection for the service
//depandency injection for the repository
var optionsBuilder = new DbContextOptionsBuilder<BottlesDBContext>();
optionsBuilder.UseSqlServer(Secret.GetConnectionString);
BottlesDBContext _dbContext = new(optionsBuilder.Options);

builder.Services.AddSingleton<IBottlesRepository>(new BottleRepositoryDatabase(_dbContext));
//end of dependency injection for the repository
#endregion


#region JWT authentication opsætning

var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    
    };
});

builder.Services.AddAuthorization();

#endregion

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//Before this we are preparing the application to use OpenAPI (Swagger) for documenting and testing the API endpoints. This line adds the OpenAPI services to the service collection, enabling the generation of OpenAPI docum
var app = builder.Build();//This line builds the application using the configured services and middleware. It creates an instance of the WebApplication class, which represents the application and its request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication(); // Checks "Who are you?"
app.UseAuthorization();  // Checks "Are you allowed to be here?"

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(allowAllCORSPolicy);//policyen er nu i brug af alle controllers.

app.UseAuthorization();

app.MapControllers();

app.Run();
