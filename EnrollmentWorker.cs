using Microsoft.Extensions.DependencyInjection;

public class EnrollmentWorker(IServiceScopeFactory scopeFactory)
{
    public void ProcessBatch()
    {
        // Create a short-lived scope
        using var scope = scopeFactory.CreateScope();

        // Resolve the scoped service
        var enrollmentService =
            scope.ServiceProvider.GetRequiredService<IEnrollmentService>();

        // Use the service
        var enrollments = enrollmentService.GetAllAsync().Result;

        Console.WriteLine($"Found {enrollments.Count()} enrollments.");

        // Scope is automatically disposed here
    }
}