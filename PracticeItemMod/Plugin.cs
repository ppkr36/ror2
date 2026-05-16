using BepInEx;

namespace PracticeItemMod;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
public class PracticeItemModPlugin : BaseUnityPlugin
{
    public const string PluginGuid = "yourname.practiceitemmod";
    public const string PluginName = "PracticeItemMod";
    public const string PluginVersion = "0.0.1";

    private void Awake()
    {
        Logger.LogInfo($"{PluginName} loaded successfully.");
    }
}