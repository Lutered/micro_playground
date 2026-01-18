using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

//Containers
var postgres = builder
                    .AddPostgres("microservice-db")
                    .WithDataVolume()
                    .WithPgAdmin();

var authDb = postgres.AddDatabase("authdb");
var usersDb = postgres.AddDatabase("usersdb");
var coursesDb = postgres.AddDatabase("coursesdb");

var rabbitMq = builder
                    .AddRabbitMQ("rabbitmqm")
                    .WithManagementPlugin();

Console.WriteLine("RabbitMq Password - " + rabbitMq.Resource.PasswordParameter.Value);

var redis = builder.AddRedis("redis").WithRedisCommander();

var elasticSearch = builder
                    .AddElasticsearch("elasticsearch")
                    .WithDataVolume()
                    .WithEnvironment("discovery.type", "single-node")
                    .WithEnvironment("xpack.security.enabled", "false")
                    .WithEnvironment("network.host", "0.0.0.0");

//var logstash = builder.AddContainer("logstash", "docker.elastic.co/logstash/logstash:8.14.0")
//   // .WithEnvironment("xpack.monitoring.elasticsearch.hosts", "http://elasticsearch:9200")
//   // .WithEnvironment("ELASTICSEARCH_HOST", "http://elasticsearch:9200")
//    //.WithReference(elasticSearch)
//    .WithBindMount("logstash/pipeline", "/usr/share/logstash/pipeline") // local folder
//    .WithEndpoint(5044, 5044);

var kibana = builder
            .AddContainer("kibana", "docker.elastic.co/kibana/kibana:8.14.0")
            .WithReference(elasticSearch)
            .WithEndpoint(5601, 5601);
            //.WithEnvironment("ELASTICSEARCH_HOSTS", "http://elastic-search:9200")
            //.WithEndpoint(targetPort: 5601, name: "http", port: 5601);


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

builder.AddProject<Projects.CoursesAPI>("coursesapi")
     .WithReference(coursesDb)
     .WithReference(rabbitMq);

builder.AddProject<Projects.Gateway>("gateway");

builder.Build().Run();
