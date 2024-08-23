using Facebook.Application;
using Facebook.Infrastructure;
using Facebook.Infrastructure.Common.Initializers;
using Facebook.Server;
using Microsoft.Extensions.FileProviders;

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

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("StaticFilesCorsPolicy", builder =>
//    {
//        builder.AllowAnyOrigin()
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//    });
//});

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

app.UseCors(options => options.SetIsOriginAllowed(origin => true)
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "images")))
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "images"));
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "images")),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<ChatHub>("/chatHub");

UserAndRolesInitializer.SeedData(app);
ActionsSeeder.SeedData(app);
FeelingsSeeder.SeedData(app);

app.Run();
