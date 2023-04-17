using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    public List<GameObject> ZoneSeparators { get; private set; } = new List<GameObject>();
    public LevelData Data { get; private set; }
    public Vector3 WorldStartPosition { get; private set; }
    public Vector3 WorldFinishPosition { get; private set; }
    private List<GameObject> _groundList = new List<GameObject>();

    public void Init(LevelData levelData)
    {
        Data = levelData;

        WorldStartPosition = new Vector3(0, Data.GroundLevel, 0);
        Vector3 groundSize = calculateGroundTileSize();
        Vector3 position = WorldStartPosition;
       
        foreach (ZoneData zone in Data.Zones)
        {
            var newZone = new GameObject(zone.Name);
            newZone.transform.SetParent(this.transform);
            for (int i = 0; i < zone.SizeInTiles; i++)
            {
                var ground = Instantiate(_groundPrefab, position, Quaternion.identity, newZone.transform);
                ground.GetComponent<SpriteRenderer>().sprite = zone.Sprite;
                var collderHeight = groundSize.y * (1 - Data.DrowningInGround);
                var boxCollider = ground.GetComponent<BoxCollider2D>();
                boxCollider.size = new Vector2(groundSize.x, collderHeight);
                boxCollider.offset = new Vector2(0, -((groundSize.y - collderHeight) / 2));
                position.x += groundSize.x;
                _groundList.Add(ground);
            }
            var separatorPosition = position;
            separatorPosition.x -= groundSize.x / 2;
            var separator = Instantiate(_separatorPrefab, separatorPosition, Quaternion.identity, newZone.transform);
            separator.GetComponent<ZoneSeparatorController>().Init(zone.Id, ZoneSeparators.Count, (separatorPosition.y- groundSize.y), (separatorPosition.y + groundSize.y));
            ZoneSeparators.Add(separator);
        }
        WorldFinishPosition = new Vector3(position.x, WorldStartPosition.y, 0);
        var backgroundSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        backgroundSpriteRenderer.sprite = Data.Background;
        backgroundSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        backgroundSpriteRenderer.size = new Vector2((WorldFinishPosition.x - WorldStartPosition.x) * 1.6f, 12.64f);
        var background = this.transform.GetChild(0);
        background.position = new Vector3((WorldFinishPosition.x - WorldStartPosition.x) / 2, 0, 0);
        
    }
    private Vector3 calculateGroundTileSize() //temporal, need rework
    {
        var ground = Instantiate(_groundPrefab, WorldStartPosition, Quaternion.identity);
        var groundRenderer = ground.GetComponent<SpriteRenderer>();
        groundRenderer.sprite = DataLoader.Levels[0].Zones[0].Sprite;
        var size = groundRenderer.sprite.bounds.size;
        GameObject.Destroy(ground);
        return size;
    }
}
