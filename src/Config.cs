using BepInEx.Configuration;
using ModMenu.Config;

namespace PlinkArms {
    internal static class Config {
        [Field("Enabled")]
        internal static ConfigEntry<bool> enabled { get; private set; }

        internal static void Init(ConfigFile configFile) {
            enabled = configFile.Bind(
                "General", "enabled", true,
                "Whether the arm textures should be replaced."
            );
        }
    }
}
