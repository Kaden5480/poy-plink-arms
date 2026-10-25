using System.IO;
using System.Linq;
using System.Reflection;

using BepInEx;
using HarmonyLib;
using ModMenu;
using UnityEngine;

namespace PlinkArms {
    [BepInDependency(
        "com.github.Kaden5480.poy-mod-menu",
        BepInDependency.DependencyFlags.SoftDependency
    )]
    [BepInPlugin("com.github.Kaden5480.poy-plink-arms", "Plink Arms", PluginInfo.PLUGIN_VERSION)]
    internal class Plugin : BaseUnityPlugin {
        private static Texture2D arms = LoadTexture("arms.png");
        private static Texture2D armsChalk = LoadTexture("arms_chalk.png");

        /**
         * <summary>
         * Loads a texture from memory and returns it.
         * </summary>
         */
        private static Texture2D LoadTexture(string name) {
            Texture2D texture = new Texture2D(1, 1);

            Assembly assembly = Assembly.GetExecutingAssembly();
            string assemblyName = assembly.GetName().Name;

            using (Stream stream = assembly.GetManifestResourceStream(
                $"{assemblyName}.res.{name}"
            )) {
                using (MemoryStream mem = new MemoryStream()) {
                    stream.CopyTo(mem);
                    ImageConversion.LoadImage(texture, mem.ToArray());
                }
            }

            return texture;
        }

        /**
         * <summary>
         * Applies the custom textures when ChalkBag is
         * being initialized.
         * </summary>
         */
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ChalkBag), "Awake")]
        private static void ApplyTextures(ChalkBag __instance) {
            if (PlinkArms.Config.enabled.Value == false) {
                return;
            }

            __instance.defaultarms = arms;
            __instance.defaultarmschalk = armsChalk;
        }

        /**
         * <summary>
         * Executes when the plugin is being loaded.
         * </summary>
         */
        private void Awake() {
            PlinkArms.Config.Init(this.Config);

            if (AccessTools.AllAssemblies().FirstOrDefault(
                    a => a.GetName().Name == "ModMenu"
                ) != null
            ) {
                Register();
            }

            Harmony.CreateAndPatchAll(typeof(Plugin));
        }

        /**
         * <summary>
         * Registers with Mod Menu.
         * </summary>
         */
        private void Register() {
            ModInfo info = ModManager.Register(this);
            info.license = "GPL-3.0";
            info.Add(typeof(PlinkArms.Config));
        }
    }
}
