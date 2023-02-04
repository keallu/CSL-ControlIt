namespace ControlIt
{
    [ConfigurationPath("ControlItConfig.xml")]
    public class ModConfig
    {
        public bool ConfigUpdated { get; set; }
        public bool ShowStatistics { get; set; } = true;
        public bool RestrictNews { get; set; } = false;
        public bool RestrictAdvertising { get; set; } = false;
        public bool RestrictUserGeneratedContentDetails { get; set; } = false;
        public bool RestrictTelemetry { get; set; } = false;
        public bool HideMenuBackground { get; set; } = false;
        public float NewsPanelOpacity { get; set; } = 1f;
        public float AccountPanelOpacity { get; set; } = 1f;
        public float DLCPanelOpacity { get; set; } = 1f;
        public float WorkshopPanelOpacity { get; set; } = 1f;
        public bool HideChirper { get; set; } = false;
        public bool UpdateUserGeneratedContentDetailsWhenBrowsing { get; set; } = true;
        public bool LogTelemetryEntriesToFile { get; set; } = false;

        private static ModConfig instance;

        public static ModConfig Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = Configuration<ModConfig>.Load();
                }

                return instance;
            }
        }

        public void Save()
        {
            Configuration<ModConfig>.Save();
            ConfigUpdated = true;
        }
    }
}