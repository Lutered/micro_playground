using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;
using UsersAPI.Configuration;
using UsersAPI.Data.Entities;
using UsersAPI.Interfaces;

namespace UsersAPI.Services
{
    public class ElasticService : IElasticService
    {
        private readonly ElasticsearchClient _client;
        private readonly ElasticSettings _settings; 

        public ElasticService(IOptions<ElasticSettings> elasticOption)
        {
            _settings = elasticOption.Value;

            var elasticSettings = new ElasticsearchClientSettings(new Uri(_settings.Url))
                .DefaultIndex(_settings.DefaultIndex);

            _client = new ElasticsearchClient(elasticSettings);
        }
        public async Task CreateIndexIfNotExistsAsync(string index)
        {
            if((await _client.Indices.ExistsAsync(index)).Exists)
                await _client.Indices.CreateAsync(index);
        }

        public async Task<bool> AddOrUpdateUserAsync(User user)
        {
            var response = await _client
                            .IndexAsync(user, idx => idx.Index(_settings.DefaultIndex)
                            .OpType(OpType.Index));

            return response.IsValidResponse;
        }
    }
}
