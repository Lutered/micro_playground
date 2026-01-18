namespace Shared.Helpers
{
    public static class CacheHelper
    {
        public static string GenerateCacheKey(string prefix, object request)
        {
            string result = prefix;
            var requestType = request.GetType();

            foreach (var property in requestType.GetProperties().OrderBy(x => x.Name))
            {
                var name = property.Name;
                var value = property.GetValue(request);

                if (value == null) continue;

                result += $":{property.Name}|{property.GetValue(request)}";
            }

            return result;
        }
    }
}
