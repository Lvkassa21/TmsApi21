using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);
// Services: add authentication / authorization services
//builder.Services.AddAuthentication("Training");
builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, TrainingAuthHandler>(
        "Training",
        null
    );
builder.Services.AddAuthorization();
builder.Services.AddSingleton<EnrollmentWorker>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Host.UseDefaultServiceProvider(options =>
{
options.ValidateScopes = true;
options.ValidateOnBuild = true;
});

var app = builder.Build();

// TODO1:Register routing in the pipeline where it belongs for your app.
app.UseMiddleware<RequestLoggingMiddleware>(); // FIRST

app.UseExceptionHandler("/error");

app.UseHttpsRedirection();
app.UseRouting();


// TODO2:Register authentication and authorization in the pipeline where your template anotected minimal API route.
app.UseAuthentication();
app.UseAuthorization();
// TODO3:MapGET/api/assessments/results with the same response body as the starter, but require authorization for that route.
app.MapGet("/api/assessments/results", () => Results.Ok(new
{
courseCode = "CS-101",
studentId = "S-001",
letterGrade = "A"
})).RequireAuthorization();

app.MapGet("/api/enrollments/worker-smoke", (EnrollmentWorker worker) =>
{
worker.ProcessBatch();
return Results.Ok("processed");
});

app.Run();