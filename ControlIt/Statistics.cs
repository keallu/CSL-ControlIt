namespace ControlIt
{
    public class Statistics
    {
        public int UserGeneratedContentDetailsRequestRestricted { get; set; }

        private static Statistics instance;

        public static Statistics Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Statistics();
                }

                return instance;
            }
        }
    }
}
