using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
    public const int PROJECTILE_LAYER = 8;
    public const int SPELL_EFFECT_LAYER = 9;
    public const int EFFECT_SPELL_ID_START_NUMBER = 1000;
    public const string JSON_PATH_BUFFS = "/JSON/abilities/buffs.json";
    public const string JSON_PATH_ABILITIES = "/JSON/Abilities/player.json";

    public static GameController Instance;
    [SerializeField] private GameObject playerPrefab;
    //[SerializeField] private GameObject playerPrefabBow;
    public Player player;
    public Dictionary<int, Ability> allSpells = new();
    public Dictionary<int, Buff> allBuffsDebuffs = new();

    public SpriteCollection itemSpriteCollection;
    public IconCollection iconCollection;

    // Start is called before the first frame update
    void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        Physics2D.IgnoreLayerCollision(PROJECTILE_LAYER, PROJECTILE_LAYER);
        InitializeDictionaries();   
            
        ItemCollection.Active = ScriptableObject.CreateInstance<ItemCollection>();
        ItemCollection.Active.SpriteCollections = new() { itemSpriteCollection };
        ItemCollection.Active.IconCollections = new() { iconCollection };  
        ItemCollection.Active.Items = new();
        CreateItem();

    }
    #region Create Item Test
    //move all item params to external source
    //Json/google sheets?
    public void CreateItem() {
        string Id = "FantasyHeroes.Basic.Armor.AcornArmor [ShowEars]";
        int Level = 1;
        ItemRarity Rarity = ItemRarity.Rare;
        ItemType Type = ItemType.Armor;
        ItemClass Class = ItemClass.Light;
        List<ItemTag> Tags = new();
        List<Property> Properties = new();
        int Price = 10;
        int Weight = 2;
        ItemMaterial Material = ItemMaterial.Wood;
        string IconId = "FantasyHeroes.Basic.Armor.AcornArmor [ShowEars]";
        string SpriteId = "FantasyHeroes.Basic.Armor.AcornArmor [ShowEars]";
        string Meta = "FantasyHeroes.Basic.Armor.AcornArmor [ShowEars]";

        ItemParams iParam = new() {
            Level = Level,
            Rarity = Rarity,
            Type = Type,
            Class = Class,
            Tags = Tags,
            Properties = Properties,
            Price = Price,
            Weight = Weight,
            Material = Material,
            IconId = IconId,
            SpriteId = SpriteId,
            Meta = Meta,
            Id = Id
        };

        ItemCollection.Active.Items.Add(iParam);

        
      //  var iconSprite = Resources.Load<Sprite>("Placeholder/Icon/Armor/Basic/AcornArmor");
     //   var sprites = Resources.LoadAll<Sprite>("Placeholder/Equipment/Armor/Basic/AcornArmor");
       // Debug.Log(sprites.Length);
      //  var spriteCol = ItemCollection.Active.SpriteCollections[0];
        

        //spriteCol.Armor.Add(itemSprite);

    }
    #endregion

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            Instantiate(playerPrefab, new Vector3(2, 0, 0), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.F2)) {
            player.EquipItem(new Item("FantasyHeroes.Basic.Armor.AcornArmor [ShowEars]"));


        }
        if (Input.GetKeyDown(KeyCode.F3)) {


        }
    }

    public static Vector3 CameraToWorldPointMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));
    }

    private void InitializeDictionaries() {
        //var temp = JsonHelper.ParseAllAbilities(JSON_PATH_BUFFS);
        //temp.Values.Cast<Buff>();

        //foreach (KeyValuePair<int, Ability> kvp in temp) {
        //    allBuffsDebuffs.Add(kvp.Key, kvp.Value as Buff);
        //}


        allSpells = JsonHelper.ParseAllAbilities(JSON_PATH_ABILITIES);

    }



}
