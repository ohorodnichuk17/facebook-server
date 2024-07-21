using Facebook.Application;
using Facebook.Infrastructure;
using Facebook.Infrastructure.Common.Initializers;
using Facebook.Server;
using Facebook.Server.Common;
using Facebook.Server.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddPresentation()
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

//NLog
//builder.Logging.ClearProviders();
//builder.Logging.SetMinimumLevel(LogLevel.Trace);
//builder.Host.UseNLog();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseCustomStaticFiles();

app.UseCors(options =>
    options.SetIsOriginAllowed(origin => true)
        .AllowAnyHeader()
        .AllowCredentials()
        .AllowAnyMethod());

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

UserAndRolesInitializer.SeedData(app);
ActionsSeeder.SeedData(app);
FeelingsSeeder.SeedData(app);

app.Run();
