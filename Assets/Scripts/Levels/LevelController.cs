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
    private PathPointsController _pathPointsController;
    public void Init(LevelData levelData)
    {
        _pathPointsController = this.transform.GetComponent<PathPointsController>();
        Data = levelData;

        WorldStartPosition = new Vector3(0, Data.GroundLevel, 0);
        Vector3 groundSize = calculateGroundTileSize();
        Vector3 position = WorldStartPosition;

        foreach (ZoneData zone in Data.Zones)
        {
             var zoneHierarchyFolder = new GameObject(zone.Name);
            zoneHierarchyFolder.transform.SetParent(this.transform);

            var separatorPosition = position;
            separatorPosition.y += groundSize.y - groundSize.y * Data.DrowningInGround;
            ZoneSeparators.Add(instantiateSeparator(separatorPosition, zoneHierarchyFolder.transform, zone, groundSize));
            _pathPointsController.AddPoint(separatorPosition.x - groundSize.x * 1.5f);
            _pathPointsController.AddPoint(separatorPosition.x + groundSize.x * 1.5f);

            for (int i = 0; i < zone.SizeInTiles; i++)
            {
                _groundList.Add(instantiateGroundTile(position, zoneHierarchyFolder.transform, zone, groundSize));
                position.x += groundSize.x;
            }
            
        }
        if (ZoneSeparators.Count > 2)
            _pathPointsController.ChangeCurrentPathPoint(ZoneSeparators.Count - 2, 2);
        WorldFinishPosition = new Vector3(position.x, WorldStartPosition.y, 0);
        backgroundSet();
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
    private void backgroundSet()
    {
        var backgroundSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        backgroundSpriteRenderer.sprite = Data.Background;
        backgroundSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        backgroundSpriteRenderer.size = new Vector2((WorldFinishPosition.x - WorldStartPosition.x) * 1.6f, 12.64f);
        var background = this.transform.GetChild(0);
        background.position = new Vector3((WorldFinishPosition.x - WorldStartPosition.x) / 2, 0, 0);
    }
    private GameObject instantiateGroundTile(Vector3 position, Transform parent, ZoneData zone, Vector3 groundSize)
    {
        var ground = Instantiate(_groundPrefab, position, Quaternion.identity, parent);
        ground.GetComponent<SpriteRenderer>().sprite = zone.Sprite;
        var collderHeight = groundSize.y * (1 - Data.DrowningInGround);
        var boxCollider = ground.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(groundSize.x, collderHeight);
        boxCollider.offset = new Vector2(0, -((groundSize.y - collderHeight) / 2));
        
        return ground;
    }
    private GameObject instantiateSeparator(Vector3 position, Transform parent, ZoneData zone, Vector3 groundSize)
    {
        var separator = Instantiate(_separatorPrefab, position, Quaternion.identity, parent);
        separator.GetComponent<ZoneSeparatorController>().Init(zone.Id, ZoneSeparators.Count, zone.SeparatorSprite);
        return separator;
    }
}
