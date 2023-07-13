using Microsoft.EntityFrameworkCore;
using TestData.Repositories.GameRepository;
using TestData.Repositories.UserRepository;
using TestServices.DbService;
using TestServices.Services.GameService;
using TestServices.Services.UserService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var dbContextFactory = new DBContextFactory();
var dbContext = dbContextFactory.CreateDbContext(null);

builder.Services.AddSingleton(dbContext);

builder.Services.AddTransient<IGameRepository, GameRepository>();
builder.Services.AddTransient<IGameService, GameService>();


builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAuthentication("Bearer").AddJwtBearer();

builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseHttpsRedirection();

app.UseExceptionHandler("/error");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
