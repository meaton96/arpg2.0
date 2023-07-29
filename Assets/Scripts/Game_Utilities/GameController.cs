using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Collections;
using Assets.HeroEditor4D.Common.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using UnityEngine;



public class GameController : MonoBehaviour {

    #region Enemy Spawning
    //spawning vars for testing
    public const bool SPAWN_ONLY_ONE_ENEMY_TYPE = false;
    public const bool ENABLE_ENEMY_LOGIC = true;
    public const int ENEMY_INDEX = 2;
    private readonly List<float> _ENEMY_SPAWN_WEIGHTS = new() {
        5, //basic goblin
        5, //shooter golin
        2, //human cleric
        1 //mage
    };
    private float _SPAWN_WEIGHT_TOTAL = 0;
    //end test vars
    public float enemySpawnTimer, enemySpawnTime = 0.5f;
    float minRad = 3, maxRad = 10;
    private int maxEnemies = 10;
    private bool spawnEnemies = true;
    #endregion

    #region Layer and JSON constants
    public const int ENEMY_LAYER = 7;
    public const int ENEMY_COLLISION_LAYER = 10;
    public const int PROJECTILE_LAYER = 8;
    public const int SPELL_EFFECT_LAYER = 9;
    public const int PLAYER_LAYER = 3;
    public const int BACKGROUND_LAYER = 11;
    public const int ENEMY_PROJECTILE_LAYER = 12;
    public const int IGNORE_COLLISION_LAYER = 13;
   // public const int EFFECT_SPELL_ID_START_NUMBER = 1000;
    
    public const int CAMERA_Z = -15;
    public const string JSON_PATH_BUFFS = "/JSON/abilities/buffs.json";
    public const string JSON_PATH_ABILITIES = "/JSON/Abilities/player.json";
    #endregion

    #region Vars - Game Settings
    [HideInInspector] public int DISPLAY_ENEMY_HEALTH_ALWAYS;
    public bool DisplayEnemyHealth { get { return DISPLAY_ENEMY_HEALTH_ALWAYS == 1; } }

    [HideInInspector] public int DISPLAY_FLOATING_COMBAT_TEXT;
    public bool DisplayFloatingCombatText { get { return DISPLAY_FLOATING_COMBAT_TEXT == 1; } }
    #endregion

    public static GameController Instance;
    [SerializeField] private GameObject playerPrefab;
    
    //[SerializeField] private GameObject playerPrefabBow;
    public Player player;
    //private Dictionary<int, Ability> allSpells = new();
    //private Dictionary<int, Buff> allBuffsDebuffs = new();

    public SpriteCollection itemSpriteCollection;
    public IconCollection iconCollection;

    public List<Enemy> enemyList;
    public List<Enemy> enemyPrefabList;

    public List<Enemy> testList;

    

    // Start is called before the first frame update
    void Start() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
        InitSettings();
        enemyList = new();
        
        for (int x = 0; x < _ENEMY_SPAWN_WEIGHTS.Count; x++) {
            _SPAWN_WEIGHT_TOTAL += _ENEMY_SPAWN_WEIGHTS[x];
        }

        SetPhysicsIgnores();        

       // InitializeDictionaries();

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
            player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        }
        if (Input.GetKeyDown(KeyCode.F2)) {
           // player.spellBar.EquipAura(0, allSpells[900] as Aura);
          //  player.spellBar.EquipAura(1, allSpells[901] as Aura);
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            spawnEnemies = !spawnEnemies;
        }

        if (player != null && enemyList.Count < maxEnemies && spawnEnemies) {
            SpawnEnemies();
        }
    }

    public static Vector3 CameraToWorldPointMousePos() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -CAMERA_Z));
    }

    //private void InitializeDictionaries() {
    //    allBuffsDebuffs = JsonHelper.ParseAllBuffsAndDebuffs(JSON_PATH_BUFFS);
    //    allSpells = JsonHelper.ParseAllAbilities(JSON_PATH_ABILITIES);
    //}

    private void SpawnEnemies() {
        if (enemySpawnTimer <= 0) {
            SpawnEnemy();
            enemySpawnTimer = enemySpawnTime;
        }
        else {
            enemySpawnTimer -= Time.deltaTime;
        }
    }
    //spawns a single enemy within minRad and maxRad radius of player randomly 
    private void SpawnEnemy() {
        int index = 0;
        var rand = UnityEngine.Random.Range(0f, _SPAWN_WEIGHT_TOTAL);

        float total = 0;
        for (int i = 0; i < _ENEMY_SPAWN_WEIGHTS.Count; i++) {
            total += _ENEMY_SPAWN_WEIGHTS[i];
            if (rand < total) {
                index = i;
                break;
            }
        }
        
        if (SPAWN_ONLY_ONE_ENEMY_TYPE) {
            index = ENEMY_INDEX;
        }

        var distanceAwayFromPlayer = UnityEngine.Random.Range(minRad, maxRad);
        var angle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);


        var x = distanceAwayFromPlayer * Mathf.Cos(angle);
        var y = distanceAwayFromPlayer * Mathf.Sin(angle);

        

        var pos = new Vector3(x, y, 0) + player.transform.position;


        var enemy = Instantiate(enemyPrefabList[index], pos, Quaternion.identity);
        enemy.Init(
            player: player,
            allEnemies: enemyList,
            health: 50,
            isActive: ENABLE_ENEMY_LOGIC
            );

        enemyList.Add(enemy);
    }

    public void RemoveEnemyFromList(Enemy enemy) {
        enemyList.Remove(enemy);
    }

    void SetPhysicsIgnores() {
        Physics2D.IgnoreLayerCollision(PROJECTILE_LAYER, PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_PROJECTILE_LAYER, ENEMY_PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(PROJECTILE_LAYER, ENEMY_PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, PLAYER_LAYER);
        Physics2D.IgnoreLayerCollision(PROJECTILE_LAYER, ENEMY_COLLISION_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_LAYER, ENEMY_COLLISION_LAYER);
        Physics2D.IgnoreLayerCollision(PLAYER_LAYER, ENEMY_COLLISION_LAYER);
        Physics2D.IgnoreLayerCollision(ENEMY_PROJECTILE_LAYER, ENEMY_COLLISION_LAYER);

        Physics2D.IgnoreLayerCollision(IGNORE_COLLISION_LAYER, PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(IGNORE_COLLISION_LAYER, ENEMY_PROJECTILE_LAYER);
        Physics2D.IgnoreLayerCollision(IGNORE_COLLISION_LAYER, ENEMY_LAYER);
        Physics2D.IgnoreLayerCollision(IGNORE_COLLISION_LAYER, PLAYER_LAYER);
        Physics2D.IgnoreLayerCollision(IGNORE_COLLISION_LAYER, ENEMY_COLLISION_LAYER);

    }
    void InitSettings() {
        PlayerSettingsHelper.InitObjectSettings(this, "Game");
    }

    //public Ability GetAbilityByID(int id) {
    //    Ability ability = null;
    //    try {
    //        ability = allSpells[id].CopyInstance();
    //    } catch (KeyNotFoundException e) {
    //        Debug.Log(e);
    //    }
    //    return ability;
    //}
    //public Buff GetBuffByID(int id) {
    //    Buff buff = null;
    //    try {
    //        buff = allBuffsDebuffs[id].CopyInstance();
    //    }
    //    catch (KeyNotFoundException e) {
    //        Debug.Log(e);
    //    }
    //    return buff;
    //}





}
