using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {
    public const float TILE_SIZE = .48f;
    public const int NUM_TILES = 50;
    public const int GRASS_TO_ROAD_TILES = 12;
    [SerializeField] private Sprite TILE_WALL;
    [SerializeField] private Sprite TILE_GRASS;
    [SerializeField] private Sprite TILE_ROAD;
    [SerializeField] private GameObject tilePrefab;
    // Start is called before the first frame update
    void Start() {
        CreateBasicTileMap();
    }

    // Update is called once per frame
    void Update() {

    }
    void CreateBasicTileMap() {

        int start = -NUM_TILES / 2;
        int end = -start - 1;

        for (int x = start; x < NUM_TILES; x++) {
            for (int y = start; y < NUM_TILES; y++) {
                var tile = Instantiate(tilePrefab, new Vector3(x * TILE_SIZE, y * TILE_SIZE, 0f), Quaternion.identity).GetComponent<SpriteRenderer>();
                if (x == start || y == start || x == end || y == end) {
                    tile.sprite = TILE_WALL;
                }
                else if (x % GRASS_TO_ROAD_TILES == 0 || y % GRASS_TO_ROAD_TILES == 0) {
                    tile.sprite = TILE_ROAD;
                }
                else
                    tile.sprite = TILE_GRASS;
            }
        }
    }
}
