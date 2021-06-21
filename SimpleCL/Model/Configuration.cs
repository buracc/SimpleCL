namespace SimpleCL.Model
{
    public class Configuration
    {
        private static Configuration Instance = null;
        private Configuration()
        {
        }

        public static Configuration GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Configuration();
            }

            return Instance;
        }
    }
}