namespace Shared.Configurations
{
    public static class CacheKeys
    {
        //public const UserKeys USERS = "Users";

        public static class User
        {
            public static string Key = "Users";
            public static string List = $"{Key}:list";
            public static string UserId = $"{Key}:user_id";
            public static string UserName = $"{Key}:user_name";
        }
    }
}
