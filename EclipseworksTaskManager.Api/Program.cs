using EclipseworksTaskManager.Api.Middlewares;
using EclipseworksTaskManager.Domain.Interfaces;
using EclipseworksTaskManager.Domain.Interfaces.Repository;
using EclipseworksTaskManager.Domain.Interfaces.Service;
using EclipseworksTaskManager.Domain.Services;
using EclipseworksTaskManager.Infra;
using EclipseworksTaskManager.Infra.EntityConfig;
using EclipseworksTaskManager.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .UseKestrel()
    .ConfigureKestrel(x =>
    {
        x.ListenAnyIP(80);
    });

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<TaskManagerContext>(x =>
//    x.UseInMemoryDatabase("TaskManager"));

builder.Services.AddDbContext<TaskManagerContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), b =>
    {
        b.MigrationsAssembly("EclipseworksTaskManager.Api");
        b.EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null);
    }));

builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IJobRepository, JobRepository>();
builder.Services.AddTransient<IProjectRepository, ProjectRepository>();
builder.Services.AddTransient<IJobEventRepository, JobEventRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IJobService, JobService>();
builder.Services.AddTransient<IProjectService, ProjectService>();
builder.Services.AddTransient<IJobCommentService, JobCommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionMiddleware();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TaskManagerContext>();
    
    context.Database.Migrate();
}

app.Run();
