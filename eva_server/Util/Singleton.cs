namespace eva_server
{
    public class Singleton<TypeName>
        where TypeName : Singleton<TypeName>, new()
    {
        public static TypeName Instance => _instance;
        private static TypeName _instance = new TypeName();
    }
}
