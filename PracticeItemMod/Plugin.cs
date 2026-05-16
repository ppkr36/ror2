using BepInEx;
using R2API;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace PracticeItemMod;

[BepInPlugin(PluginGuid, PluginName, PluginVersion)]
[BepInDependency(ItemAPI.PluginGUID)]
[BepInDependency(LanguageAPI.PluginGUID)]
public class Plugin : BaseUnityPlugin
{
    public const string PluginGuid = "dimch.practiceitemmod";
    public const string PluginName = "PracticeItemMod";
    public const string PluginVersion = "0.0.2";

    private static ItemDef lightweightCoreItemDef;

    private void Awake()
    {
        Logger.LogInfo($"{PluginName} loading...");

        CreateLightweightCoreItem();

        Logger.LogInfo($"{PluginName} loaded. Start a run and press F2.");
    }

    private void CreateLightweightCoreItem()
    {
        lightweightCoreItemDef = ScriptableObject.CreateInstance<ItemDef>();

        lightweightCoreItemDef.name = "DIMCH_LIGHTWEIGHT_CORE";
        lightweightCoreItemDef.nameToken = "DIMCH_LIGHTWEIGHT_CORE_NAME";
        lightweightCoreItemDef.pickupToken = "DIMCH_LIGHTWEIGHT_CORE_PICKUP";
        lightweightCoreItemDef.descriptionToken = "DIMCH_LIGHTWEIGHT_CORE_DESC";
        lightweightCoreItemDef.loreToken = "DIMCH_LIGHTWEIGHT_CORE_LORE";

        lightweightCoreItemDef.deprecatedTier = ItemTier.Tier1;

        lightweightCoreItemDef.pickupIconSprite =
            Addressables.LoadAssetAsync<Sprite>("RoR2/Base/Common/MiscIcons/texMysteryIcon.png").WaitForCompletion();

        lightweightCoreItemDef.pickupModelPrefab =
            Addressables.LoadAssetAsync<GameObject>("RoR2/Base/Mystery/PickupMystery.prefab").WaitForCompletion();

        lightweightCoreItemDef.canRemove = true;
        lightweightCoreItemDef.hidden = false;

        lightweightCoreItemDef.tags = new[]
        {
            ItemTag.Utility
        };

        LanguageAPI.Add("DIMCH_LIGHTWEIGHT_CORE_NAME", "Lightweight Core");
        LanguageAPI.Add("DIMCH_LIGHTWEIGHT_CORE_PICKUP", "A simple test item.");
        LanguageAPI.Add("DIMCH_LIGHTWEIGHT_CORE_DESC", "This item currently does nothing. It is only a test item.");
        LanguageAPI.Add("DIMCH_LIGHTWEIGHT_CORE_LORE", "A tiny core used for learning modding.");

        var displayRules = new ItemDisplayRuleDict(null);

        ItemAPI.Add(new CustomItem(lightweightCoreItemDef, displayRules));

        Logger.LogInfo("Lightweight Core registered.");
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.F2))
        {
            return;
        }

        SpawnTestItem();
    }

    private void SpawnTestItem()
    {
        if (PlayerCharacterMasterController.instances.Count <= 0)
        {
            Logger.LogWarning("No player found. Start a run first.");
            return;
        }

        CharacterMaster master = PlayerCharacterMasterController.instances[0].master;

        if (master == null)
        {
            Logger.LogWarning("Player master is null.");
            return;
        }

        GameObject bodyObject = master.GetBodyObject();

        if (bodyObject == null)
        {
            Logger.LogWarning("Player body is null.");
            return;
        }

        Transform playerTransform = bodyObject.transform;

        PickupIndex pickupIndex = PickupCatalog.FindPickupIndex(lightweightCoreItemDef.itemIndex);

        PickupDropletController.CreatePickupDroplet(
            pickupIndex,
            playerTransform.position + Vector3.up * 2f,
            playerTransform.forward * 20f
        );

        Logger.LogInfo("Spawned Lightweight Core.");
    }
}