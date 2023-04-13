using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    public Vector3 WorldStartPosition { get; private set; }
    public Vector3 WorldFinishPosition { get; private set; }

    private List<GameObject> _groundList = new List<GameObject>();
    private List<GameObject> _LevelList = new List<GameObject>();
    private GameObject _level;
    public GameObject CurrentLevel {
        get {   return _level;}
        private set { _level = value;} }
    public int CurrentLevelZonesCount { get; private set; } 

    public delegate void LevelCreated(GameObject Level);
    public static event LevelCreated LevelIsCreated;

    private void Start()
    {
        CreateLevel(0);
    }
    void CreateLevel(int levelId)
    {
        if (levelId > DataLoader.Levels.Count - 1 || levelId < 0)
        {
            Debug.LogError("LevelController: trying to create unexisted level");
            return;
        }
        WorldStartPosition = new Vector3(0, DataLoader.Levels[levelId].GroundLevel, 0);
        Vector3 groundSize = calculateGroundTileSize();
        Vector3 position = WorldStartPosition;
        _level = Instantiate(_levelPrefab, Vector3.zero, Quaternion.identity);
        CurrentLevelZonesCount = DataLoader.Levels[levelId].ZonesCount;

        foreach (ZoneData zone in DataLoader.Levels[levelId].Zones)
        {
            var newZone = new GameObject(zone.Name);
            newZone.transform.SetParent(_level.transform);
            for (int i = 0; i < zone.SizeInTiles; i++)
            {
                var ground = Instantiate(_groundPrefab, position, Quaternion.identity, newZone.transform);
                ground.GetComponent<SpriteRenderer>().sprite = zone.Sprite;
                var collderHeight = groundSize.y * (1 - DataLoader.Levels[levelId].DrowningInGround);
                var boxCollider = ground.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2 (groundSize.x, collderHeight);
                boxCollider.offset = new Vector2(0,-((groundSize.y - collderHeight) / 2));
                position.x += groundSize.x;
                _groundList.Add(ground);
            }
            var separatorPosition = position;
            separatorPosition.x -= groundSize.x / 2;
            var separator = Instantiate(_separatorPrefab, separatorPosition, Quaternion.identity, newZone.transform);
            separator.GetComponent<ZoneSeparatorController>().ZoneId = zone.Id;
        }
        WorldFinishPosition = new Vector3(position.x, WorldStartPosition.y, 0);
        var backgroundSpriteRenderer = _level.GetComponentInChildren<SpriteRenderer>();
        backgroundSpriteRenderer.sprite = DataLoader.Levels[levelId].Background;
        backgroundSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        backgroundSpriteRenderer.size = new Vector2( (WorldFinishPosition.x - WorldStartPosition.x)*1.6f, 12.64f);
        //backgroundSpriteRenderer.bounds.
        var background = _level.transform.GetChild(0);
        background.position = new Vector3((WorldFinishPosition.x - WorldStartPosition.x) / 2, 0, 0);
        _LevelList.Add(_level);
        LevelIsCreated?.Invoke(_level);
    }

    private Vector3 calculateGroundTileSize()
    {
        var ground = Instantiate(_groundPrefab, WorldStartPosition, Quaternion.identity);
        var groundRenderer = ground.GetComponent<SpriteRenderer>();
        groundRenderer.sprite = DataLoader.Levels[0].Zones[0].Sprite;
        var size = groundRenderer.sprite.bounds.size;
        GameObject.Destroy(ground);
        return size;
    }
}
