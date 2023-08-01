using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    #region Enemy Spawning  
    //spawning vars for testing
    public bool SpawnOneType = true;
    public bool EnemiesEnabled = true;
    public int EnemyIndex = 0;
    private readonly List<float> _ENEMY_SPAWN_WEIGHTS = new() {
        5, //basic goblin   - 0
        5, //shooter golin  - 1
        2, //human cleric   - 2
        1, //mage           - 3
        2, //axe wielder    - 4
        4, //ice archer     - 5
    };
    private float _SPAWN_WEIGHT_TOTAL = 0;
    //end test vars
    private float enemySpawnTimer, enemySpawnTime = 0.5f;
    float minRad = 3, maxRad = 10;
    [SerializeField] private int maxEnemies = 20;
    public bool spawnEnemies = false;
    #endregion

   [HideInInspector]public List<Enemy> enemyList;
    public List<Enemy> enemyPrefabList;
    //public static EnemySpawnManager Instance;
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        //if (Instance != null && Instance != this) {
        //    Destroy(this);
        //}
        //else {
        //    Instance = this;
        //}
        enemyList = new();

        for (int x = 0; x < _ENEMY_SPAWN_WEIGHTS.Count; x++) {
            _SPAWN_WEIGHT_TOTAL += _ENEMY_SPAWN_WEIGHTS[x];
        }
        if (Input.GetKeyDown(KeyCode.F3)) {
            spawnEnemies = !spawnEnemies;
        } 
    }
    private void Update() {
        if (spawnEnemies) {
            if (player != null) {
                SpawnEnemies();
            }
        }
    }
    public void StartSpawning(Player player) {
        this.player = player;
        spawnEnemies = true;
    }

    public void SpawnEnemies() {
        
        if (enemySpawnTimer <= 0 && enemyList.Count < maxEnemies) {
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
        var rand = Random.Range(0f, _SPAWN_WEIGHT_TOTAL);

        float total = 0;
        for (int i = 0; i < _ENEMY_SPAWN_WEIGHTS.Count; i++) {
            total += _ENEMY_SPAWN_WEIGHTS[i];
            if (rand < total) {
                index = i;
                break;
            }
        }

        if (SpawnOneType) {
            index = EnemyIndex;
        }

        var distanceAwayFromPlayer = Random.Range(minRad, maxRad);
        var angle = Random.Range(0f, 2 * Mathf.PI);


        var x = distanceAwayFromPlayer * Mathf.Cos(angle);
        var y = distanceAwayFromPlayer * Mathf.Sin(angle);



        var pos = new Vector3(x, y, 0) + player.transform.position;


        var enemy = Instantiate(enemyPrefabList[index], pos, Quaternion.identity);

        enemy.Init(
            num: enemyList.Count,
            player: player,
            allEnemies: enemyList,
            health: 50,
            isActive: EnemiesEnabled
            );

        enemyList.Add(enemy);
    }

    public void RemoveEnemyFromList(Enemy enemy) {
        enemyList.Remove(enemy);
    }

    public Enemy this[int key] {
        get {
            return enemyList[key];
        }
        //set {
        //    int index = keys.IndexOf(key);
        //    if (index != -1) {
        //        values[index] = value;
        //    }
        //    else {
        //        Debug.LogWarning("SerializableDictionary: Key not found in the dictionary. Adding a new entry.");
        //        keys.Add(key);
        //        values.Add(value);
        //    }
        //}
    }

}
