var builder = DistributedApplication.CreateBuilder(args);

//Containers
var postgres = builder
                    .AddPostgres("microservice-db")
                    .WithDataVolume()
                    .WithPgAdmin();

var authDb = postgres.AddDatabase("authdb");
var usersDb = postgres.AddDatabase("usersdb");

var rabbitMq = builder
                    .AddRabbitMQ("rabbitmq")
                    .WithManagementPlugin();

var redis = builder.AddRedis("redis");

var elasticSearch = builder
                    .AddElasticsearch("elasticsearch")
                    .WithDataVolume()
                    .WithEnvironment("discovery.type", "single-node")
                    .WithEnvironment("xpack.security.enabled", "false")
                    .WithEnvironment("network.host", "0.0.0.0");

var kibana = builder
            .AddContainer("kibana", "docker.elastic.co/kibana/kibana:8.14.0")
            .WithEnvironment("ELASTICSEARCH_HOSTS", "http://elastic-search:9200")
            .WithEndpoint(targetPort: 5601, name: "http");


//Projects
builder.AddProject<Projects.AuthAPI>("authapi")
        .WithReference(authDb)
        .WithReference(rabbitMq)
        .WithReference(elasticSearch);

builder.AddProject<Projects.UsersAPI>("usersapi")
    .WithReference(usersDb)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(elasticSearch);

builder.AddProject<Projects.Gateway>("gateway");

builder.Build().Run();
