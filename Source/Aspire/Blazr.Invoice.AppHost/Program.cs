var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Blazr_App_Fluent_Server>("blazr-app-fluent-server");

builder.AddProject<Projects.Blazr_App_Server>("blazr-app-server");

builder.Build().Run();
