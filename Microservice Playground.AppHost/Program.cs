var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Microservice Playground>("microservice playground");

builder.Build().Run();
