using DataAccessService;
using DataAccessService.CommandHandlers;
using Domain.Commands;
using Microsoft.EntityFrameworkCore;
using RegistrarAPI.Decorators;
using RegistrarAPI.DTO;
using RegistrarAPI.QueryHandlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });

builder.Services.AddDbContext<RegistrarDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"))
    .UseLoggerFactory(loggerFactory));

builder.Services.AddScoped<ICommandHandler<EnrollStudentCommand>>(provider =>
{
    var enrollStudentCommandHandler = new EnrollStudentCommandHandler(provider.GetRequiredService<RegistrarDbContext>());
    return new AuditLoggingDecorator<EnrollStudentCommand>(enrollStudentCommandHandler);
});

builder.Services.AddScoped<ICommandHandler<EditStudentInformationCommand>, EditStudentInfotmationCommandHandler>();
builder.Services.AddScoped<ICommandHandler<RegisterCourseCommand>, RegisterCourseCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetStudentListQuery, List<StudentDTO>>, GetStudentsListHandler>();
builder.Services.AddTransient<Message>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
