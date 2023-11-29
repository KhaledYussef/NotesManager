using Data.Context;

using Microsoft.AspNetCore.Builder;

using Website;
using Website.Routing;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddMyDbContext();
builder.Services.AddMyIdentity(builder.Configuration);
builder.Services.AddMyServices();
builder.Services.AddMyActionFilters();
builder.Services.AddMySwagger();
builder.Services.Configure<RouteOptions>(options =>
            options.ConstraintMap.Add("NoteShareRoute", typeof(NoteShareRouteConstraint)));

//builder.Services.AddRouting(options =>
//{
//    options.ConstraintMap.Add("note", typeof(NoteShareRouteConstraint));
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    c.RoutePrefix = "api";
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// note share route
app.MapControllerRoute(
        name: "note",
        pattern: "note/{shareLink:NoteShareRoute}",
        defaults: new { controller = "Notes", action = "Shared" }
    );




app.MapControllers();

app.SeedData();
app.Run();