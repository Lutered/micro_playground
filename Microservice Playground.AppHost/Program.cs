using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("microservice-db")
                .WithDataVolume()
                .WithPgAdmin();

var rabbitMq = builder.AddRabbitMQ("rabbitmq")
                    .WithManagementPlugin();

var redis = builder.AddRedis("redis");

var elasticSearch = builder.AddElasticsearch("elastic-search")
        .WithDataVolume()
        .WithEnvironment("discovery.type", "single-node")
        .WithEnvironment("xpack.security.enabled", "false")
        .WithEnvironment("network.host", "0.0.0.0");

var kibana = builder.AddContainer("kibana", "docker.elastic.co/kibana/kibana:8.14.0")
    .WithEnvironment("ELASTICSEARCH_HOSTS", "http://elastic-search:9200")
    .WithEndpoint(targetPort: 5601, name: "http");


builder.AddProject<Projects.AuthAPI>("authapi")
        .WithReference(postgres)
        .WithReference(rabbitMq)
        .WithReference(elasticSearch)
        .WithEnvironment("ElasticUrl", "http://localhost:9200");

builder.AddProject<Projects.UsersAPI>("usersapi")
    .WithReference(postgres)
    .WithReference(rabbitMq)
    .WithReference(redis)
    .WithReference(elasticSearch);

builder.AddProject<Projects.Gateway>("gateway");

builder.Build().Run();
