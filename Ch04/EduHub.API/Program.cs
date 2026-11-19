using EduHub.Application.Clubs;
using EduHub.Application.Events;
using EduHub.Application.Registrations;
using EduHub.Application.Reports;
using EduHub.Domain.Abstractions;
using EduHub.Infrastructure.Persistence;
using EduHub.Infrastructure.Reports;
using EduHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// TODO [Nhiệm vụ 6] Serilog: builder.Host.UseSerilog((ctx, cfg) => cfg.ReadFrom.Configuration(ctx.Configuration).Enrich.FromLogContext());
//  và bổ sung mục "Serilog" (MinimumLevel, WriteTo Console) trong appsettings.json.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO [Nhiệm vụ 6] builder.Services.AddProblemDetails();
//                   builder.Services.AddExceptionHandler<GlobalExceptionHandler>();  (cần using EduHub.API.Middleware)

// Connection string: appsettings.Development.json hoặc biến môi trường ConnectionStrings__DefaultConnection (Docker)
builder.Services.AddDbContext<EduHubDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// TODO [Nhiệm vụ 2-5] Đăng ký DI (Scoped): IUnitOfWork -> EfUnitOfWork, IClubRepository -> EfClubRepository,
//  IEventRepository -> EfEventRepository, IRegistrationRepository -> EfRegistrationRepository,
//  IEventReportQuery -> DapperEventReportQuery, ClubService, EventService, GetEventsQueryHandler,
//  RegisterForEventCommandHandler, GetEventRegistrationsQueryHandler, CancelRegistrationCommandHandler.

var app = builder.Build();

// Tự áp migration + seed khi khởi động (đặt Database__AutoMigrate=true, mặc định bật ở Development)
if (app.Configuration.GetValue("Database:AutoMigrate", app.Environment.IsDevelopment()))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<EduHubDbContext>();
    await db.Database.MigrateAsync();
    await db.SeedAsync();
}

// TODO [Nhiệm vụ 6] app.UseSerilogRequestLogging();  rồi  app.UseExceptionHandler();  rồi  app.UseStatusCodePages();
//  (thứ tự này để log request thấy đúng mã trạng thái sau khi lỗi được đổi thành ProblemDetails)

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();
