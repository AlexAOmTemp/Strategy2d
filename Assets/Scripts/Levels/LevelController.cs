using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private BuildingPlacerController _placerPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    [SerializeField] private BuildingSpawner _buildingSpawner;
    [SerializeField] private GameObject _blockerPrefab;
    public int Id { get; private set; }
    public bool Reverse { get; private set; }
    public List<float> PathPoints { get; private set; } = new List<float>();
    public float PlacementLevel { get; private set; }
    public Transform BuildingPlacer { get; private set; }
    public List<GameObject> ZoneSeparators { get; private set; } = new List<GameObject>();
    public LevelData Data { get; private set; }
    public Vector3 StartPosition { get; private set; }
    public Vector3 FinishPosition { get; private set; }
    private List<GameObject> _groundList = new List<GameObject>();
    private Vector3 _groundSize;
    public void Init(float startPositionX, LevelData levelData, bool reverse, int Id)
    {
        this.Id = Id;
        Reverse = reverse;
        Data = levelData;
        this.name = Data.Name;
        StartPosition = new Vector3(startPositionX, Data.GroundLevel, 0);
        _groundSize = calculateGroundTileSize();
        PlacementLevel = Data.GroundLevel + _groundSize.y / 2 - _groundSize.y * Data.DrowningInGround;

        _buildingSpawner.Init(Id);
        Vector3 currentPlacementPosition = calculateFirstTilePosition();

        foreach (ZoneData zone in Data.Zones)
            createZone(zone, ref currentPlacementPosition);

        FinishPosition = calculateFinishPosition(currentPlacementPosition);

        createBlockers();

        BuildingPlacer = this.transform.Find("BuildingPlacer");
        BuildingPlacer.position = new Vector3(BuildingPlacer.position.x, PlacementLevel, 0);

        Vector3 buildingPosition = placeMainBuilding();
        backgroundSet();

        if (Reverse == false)
            createPlayer(buildingPosition);
    }
    public Vector3 GetPortalCoords()
    {
        return ZoneSeparators.Last().transform.position;
    }
    public void SetupPortal(Vector3 destinationCoords)
    {
        var portal = ZoneSeparators.Last().transform.GetChild(0).gameObject;
        portal.SetActive(true);
        portal.GetComponent<Portal>().Init(destinationCoords);
    }
    private Vector3 calculateGroundTileSize() //temporal, need rework
    {
        var ground = Instantiate(_groundPrefab, StartPosition, Quaternion.identity);
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
        backgroundSpriteRenderer.size = new Vector2((FinishPosition.x - StartPosition.x) * 1.6f, 12.64f);
        var background = this.transform.GetChild(0);
        background.position = new Vector3((FinishPosition.x - StartPosition.x) / 2, 0, 0);
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
        separator.GetComponent<SeparatorController>().Init(zone.Id, ZoneSeparators.Count, zone.SeparatorSprite, Reverse);
        float height = separator.GetComponent<SpriteRenderer>().size.y;
        separator.transform.Translate(Vector3.up * height / 2);
        if (Reverse)
            separator.transform.localScale = new Vector3(-1, 1, 1);
        return separator;
    }
    private void placeBuilding(string buildingName, Vector3 position)
    {
        BuildingData? building = DataLoader.Buildings.Find(i => i.Name == buildingName);
        if (building == null)
            Debug.LogError("LevelController: Main building not found");
        _buildingSpawner.InstantBuild(position, (BuildingData)building);
    }
    private void addPoints(float separatorPosX)
    {
        if (Reverse == false)
        {
            PathPoints.Add(separatorPosX - _groundSize.x * 1.5f);
            PathPoints.Add(separatorPosX + _groundSize.x * 1.5f);
        }
        else
        {
            PathPoints.Add(separatorPosX + _groundSize.x * 1.5f);
            PathPoints.Add(separatorPosX - _groundSize.x * 1.5f);
        }
    }
    private void spawnGround(ref Vector3 position, Transform parent, ZoneData zone)
    {
        for (int i = 0; i < zone.SizeInTiles; i++)
        {
            _groundList.Add(instantiateGroundTile(position, parent.transform, zone, _groundSize));
            if (Reverse == false)
                position.x += _groundSize.x;
            else
                position.x -= _groundSize.x;
        }
    }
    private void placeEndBuilding(float positionX, ZoneData zone)
    {
        if (zone.EndBuilding != null)
        {
            Vector3 endBuildingPosition; 
            if (Reverse == false)
                endBuildingPosition = new Vector3(positionX - _groundSize.x, PlacementLevel, 0);
            else
                endBuildingPosition = new Vector3(positionX + _groundSize.x, PlacementLevel, 0);
            placeBuilding(zone.EndBuilding, endBuildingPosition);
        }
    }
    private void createZone(ZoneData zone, ref Vector3 position)
    {
        var zoneHierarchyFolder = new GameObject(zone.Name);
        zoneHierarchyFolder.transform.SetParent(this.transform);
        ZoneSeparators.Add(instantiateSeparator(new Vector3(position.x, PlacementLevel, 0), zoneHierarchyFolder.transform, zone, _groundSize));
        addPoints(position.x);
        spawnGround(ref position, zoneHierarchyFolder.transform, zone);
        placeEndBuilding(position.x, zone);
    }
    private void createBlockers()
    {
        var blockerStart = Instantiate(_blockerPrefab, new Vector3(StartPosition.x, PlacementLevel, 0), Quaternion.identity, this.transform);
        var blockerFinish = Instantiate(_blockerPrefab, new Vector3(FinishPosition.x, PlacementLevel, 0), Quaternion.identity, this.transform);
        if (Reverse == false)
        {
            blockerStart.transform.Translate(Vector3.left * blockerStart.GetComponent<BoxCollider2D>().size.x / 2f);
            blockerFinish.transform.Translate(Vector3.right * blockerFinish.GetComponent<BoxCollider2D>().size.x / 2f);
        }
        else
        {
            blockerStart.transform.Translate(Vector3.right * blockerStart.GetComponent<BoxCollider2D>().size.x / 2f);
            blockerFinish.transform.Translate(Vector3.left * blockerFinish.GetComponent<BoxCollider2D>().size.x / 2f);
        }
    }
    private Vector3 placeMainBuilding()
    {
        Vector3 mainBuildingPosition;
        if (Reverse == false)
            mainBuildingPosition = new Vector3(StartPosition.x + _groundSize.x * 5, PlacementLevel, 0);
        else
            mainBuildingPosition = new Vector3(StartPosition.x - _groundSize.x * 5, PlacementLevel, 0);
        placeBuilding(Data.MainBuilding, mainBuildingPosition);
        return mainBuildingPosition;
    }
    private Vector3 calculateFinishPosition(Vector3 position)
    {
        Vector3 result;
        if (Reverse == false)
            result = new Vector3(position.x - _groundSize.x / 2, StartPosition.y, 0);
        else
            result = new Vector3(position.x + _groundSize.x / 2, StartPosition.y, 0);
        return result;
    }
    private Vector3 calculateFirstTilePosition()
    {
        Vector3 position;
        if (Reverse == false)
            position = new Vector3(StartPosition.x + _groundSize.x / 2, StartPosition.y, 0);
        else
            position = new Vector3(StartPosition.x - _groundSize.x / 2, StartPosition.y, 0);
        return position;
    }
    private void createPlayer(Vector3 position)
    {
        var player = Instantiate(_playerPrefab, position, Quaternion.identity, this.transform);
        player.GetComponent<Team>().Number=this.Id;
        _placerPrefab.setPlayer(player);
    }
}
