var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.AuthAPI>("authapi");

builder.AddProject<Projects.Gateway>("gateway");

builder.AddProject<Projects.UsersAPI>("usersapi");

builder.Build().Run();
