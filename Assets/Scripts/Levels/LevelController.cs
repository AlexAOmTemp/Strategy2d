using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
public class LevelController : MonoBehaviour
{
    [SerializeField] private BuildingPlacerController _placerPrefab;
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private GameObject _groundPrefab;
    [SerializeField] private GameObject _separatorPrefab;
    [SerializeField] private BuildingSpawner _buildingSpawner;
    [SerializeField] private GameObject _blockerPrefab;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private Transform _finishPosition;

    public int Id { get; private set; }
    public bool Reverse { get; private set; }
    public float PlacementLevel { get; private set; }
    public Transform BuildingPlacer { get; private set; }

    public LevelData Data { get; private set; }
    private List<GameObject> _groundList = new List<GameObject>();
    private Vector3 _groundSize;
    private List<GameObject> _zoneSeparators = new List<GameObject>();
    private List<GameObject> _pathPoints = new List<GameObject>();

    public void Init(float startPositionX, LevelData levelData, bool reverse, int Id, bool player)
    {
        this.Id = Id;
        Reverse = reverse;
        Data = levelData;
        this.name = Data.Name;
        _startPosition.position = new Vector3(startPositionX, Data.GroundLevel, 0);
        _groundSize = calculateGroundTileSize();
        PlacementLevel = Data.GroundLevel + _groundSize.y / 2 - _groundSize.y * Data.DrowningInGround;

        _buildingSpawner.Init(Id);
        Vector3 currentPlacementPosition = calculateFirstTilePosition();

        foreach (ZoneData zone in Data.Zones)
            createZone(zone, ref currentPlacementPosition);

        _finishPosition.position = calculateFinishPosition(currentPlacementPosition);

        createBlockers();

        BuildingPlacer = this.transform.Find("BuildingPlacer");
        BuildingPlacer.position = new Vector3(BuildingPlacer.position.x, PlacementLevel, 0);

        Vector3 buildingPosition = new Vector3(5,PlacementLevel+1,0);
        if (Data.MainBuilding!=null)
            buildingPosition = placeMainBuilding();
        backgroundSet();

        if (player == true)
            createPlayer(buildingPosition);
    }
    public List<GameObject> GetZoneSeparators()
    {
        return new List<GameObject>(_zoneSeparators);
    }
    public List<GameObject> GetPathPoints()
    {
        return new List<GameObject>(_pathPoints);
    }
    public Vector3 GetFinishPosition()
    {
        return _finishPosition.position;
    }
    public Vector3 GetStartPosition()
    {
        return _startPosition.position;
    }
    public Vector3 GetPortalCoords()
    {
        return _zoneSeparators.Last().transform.position;
    }
    public void SetupPortal(Vector3 destinationCoords)
    {
        var portal = _zoneSeparators.Last().transform.GetChild(0).gameObject;
        portal.SetActive(true);
        portal.GetComponent<Portal>().Init(destinationCoords);
    }
    private Vector3 calculateGroundTileSize() //temporal, need rework
    {
        var ground = Instantiate(_groundPrefab, _startPosition.position, Quaternion.identity);
        var groundRenderer = ground.GetComponent<SpriteRenderer>();
        groundRenderer.sprite = DataLoader.Levels[0].Zones[0].GroundSprite;
        var size = groundRenderer.sprite.bounds.size;
        GameObject.Destroy(ground);
        return size;
    }
    private void backgroundSet()
    {
        var backgroundSpriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        backgroundSpriteRenderer.sprite = Data.Background;
        backgroundSpriteRenderer.drawMode = SpriteDrawMode.Tiled;
        backgroundSpriteRenderer.size = new Vector2((Math.Abs(_finishPosition.position.x - _startPosition.position.x)) + 50, 12.64f);
        var background = this.transform.GetChild(0);
        background.position = new Vector3((_finishPosition.position.x - _startPosition.position.x) / 2, 0, 0);
    }
    private GameObject instantiateGroundTile(Vector3 position, Transform parent, ZoneData zone, Vector3 groundSize)
    {
        var ground = Instantiate(_groundPrefab, position, Quaternion.identity, parent);
        ground.GetComponent<SpriteRenderer>().sprite = zone.GroundSprite;
        var collderHeight = groundSize.y * (1 - Data.DrowningInGround);
        var boxCollider = ground.GetComponent<BoxCollider2D>();
        boxCollider.size = new Vector2(groundSize.x, collderHeight);
        boxCollider.offset = new Vector2(0, -((groundSize.y - collderHeight) / 2));

        return ground;
    }
    private GameObject instantiateSeparator(Vector3 position, Transform parent, ZoneData zone, Vector3 groundSize)
    {
        var separator = Instantiate(_separatorPrefab, position, Quaternion.identity, parent);
        separator.GetComponent<SeparatorController>().Init(zone.Id, zone.SeparatorSprite, Reverse);
        float height = separator.GetComponent<SpriteRenderer>().size.y;
        separator.transform.Translate(Vector3.up * height / 2);
        if (Reverse)
            separator.transform.localScale = new Vector3(-1, 1, 1);
        return separator;
    }
    private void placeBuilding(string buildingName, Vector3 position, bool main)
    {
        BuildingData? building = DataLoader.Buildings.Find(i => i.Name == buildingName);
        if (building == null)
            Debug.LogError("LevelController: Main building not found");
        _buildingSpawner.InstantBuild(position, (BuildingData)building, main);
    }
    private void addPoints(float separatorPosX)
    {
        if (Reverse == false)
        {
            _pathPoints.Add(createPathPoint( separatorPosX - _groundSize.x * 1.5f));
            _pathPoints.Add(createPathPoint(separatorPosX + _groundSize.x * 1.5f));
        }
        else
        {
            _pathPoints.Add(createPathPoint(separatorPosX + _groundSize.x * 1.5f));
            _pathPoints.Add(createPathPoint( separatorPosX - _groundSize.x * 1.5f));
        }
    }
    private GameObject createPathPoint(float positionX)
    {
        var pathPoint = new GameObject();
        pathPoint.transform.SetParent(this.transform);
        pathPoint.transform.position = new Vector3(positionX, 0, 0);
        return pathPoint;
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
            placeBuilding(zone.EndBuilding, endBuildingPosition, false);
        }
    }
    private void createZone(ZoneData zone, ref Vector3 position)
    {
        var zoneHierarchyFolder = new GameObject(zone.Name);

        zoneHierarchyFolder.transform.SetParent(this.transform);
        _zoneSeparators.Add(instantiateSeparator(new Vector3(position.x, PlacementLevel, 0), zoneHierarchyFolder.transform, zone, _groundSize));

        var startBackGroundPosition = position.x;
        addPoints(position.x);
        spawnGround(ref position, zoneHierarchyFolder.transform, zone);
        placeEndBuilding(position.x, zone);

        if (zone.Background != null)
        {
            var background = new GameObject("Background");
            var backgroundRenderer = background.AddComponent<SpriteRenderer>();
            backgroundRenderer.sprite = zone.Background;
            backgroundRenderer.drawMode = SpriteDrawMode.Tiled;
            backgroundRenderer.size = new Vector2((Math.Abs(startBackGroundPosition - position.x)), backgroundRenderer.size.y);
            backgroundRenderer.sortingOrder = 1;
            backgroundRenderer.sortingLayerName = "Background";
            background.transform.SetParent(zoneHierarchyFolder.transform);

            float backgroundPositionX;
            if (Reverse == false)
                backgroundPositionX = startBackGroundPosition + (position.x - startBackGroundPosition) / 2 - _groundSize.x;
            else
                backgroundPositionX = startBackGroundPosition + (position.x - startBackGroundPosition) / 2 + _groundSize.x;

            var backgroundPositionY = PlacementLevel + backgroundRenderer.size.y / 2;
            background.transform.position = new Vector3(backgroundPositionX, backgroundPositionY, 0);
        }
    }
    private void createBlockers()
    {
        var blockerStart = Instantiate(_blockerPrefab, new Vector3(_startPosition.position.x, PlacementLevel, 0), Quaternion.identity, this.transform);
        var blockerFinish = Instantiate(_blockerPrefab, new Vector3(_finishPosition.position.x, PlacementLevel, 0), Quaternion.identity, this.transform);
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
            mainBuildingPosition = new Vector3(_startPosition.position.x + _groundSize.x * 5, PlacementLevel, 0);
        else
            mainBuildingPosition = new Vector3(_startPosition.position.x - _groundSize.x * 5, PlacementLevel, 0);
        placeBuilding(Data.MainBuilding, mainBuildingPosition, true);

        return mainBuildingPosition;
    }
    private Vector3 calculateFinishPosition(Vector3 position)
    {
        Vector3 result;
        if (Reverse == false)
            result = new Vector3(position.x - _groundSize.x / 2, _startPosition.position.y, 0);
        else
            result = new Vector3(position.x + _groundSize.x / 2, _startPosition.position.y, 0);
        return result;
    }
    private Vector3 calculateFirstTilePosition()
    {
        Vector3 position;
        if (Reverse == false)
            position = new Vector3(_startPosition.position.x + _groundSize.x / 2, _startPosition.position.y, 0);
        else
            position = new Vector3(_startPosition.position.x - _groundSize.x / 2, _startPosition.position.y, 0);
        return position;
    }
    private void createPlayer(Vector3 position)
    {
        var player = Instantiate(_playerPrefab, position, Quaternion.identity, this.transform);
        player.GetComponent<Team>().Number = this.Id;
        _placerPrefab.setPlayer(player);
    }
}
