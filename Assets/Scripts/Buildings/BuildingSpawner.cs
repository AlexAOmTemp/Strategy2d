using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unconstructBuildingPrefab;
    [SerializeField] private Transform _buildingPlacePoint;
    public List<GameObject> BuildingsInProcess = new List<GameObject>();
    private GameObject _currentBuilding;
    private int? _currentBuildingTypeId;
    public delegate void NewBuildingInProcess(GameObject Building);
    public static event NewBuildingInProcess NewBuildingIsInProcess;
    private int _teamTag;
    
    enum BuildingState
    {
        Idle,
        HeroAttached,
        Placed
    }
    private BuildingState _currentState = BuildingState.Idle;

    public void Init (int team)
    {
        _teamTag=team;
        ConstructionController.ConstructionIsFinished += onBuildingFinished;
    }
    public void CreateBuildingPlacer(int typeId)
    {
        if (_currentState != BuildingState.Idle)
            PlaceDeclined();
        if (typeId > DataLoader.Buildings.Count - 1)
        {
            Debug.LogError("BuildingSpawner: tried to create an unexistant building");
            return;
        }
        _currentBuildingTypeId = typeId;
        //_currentBuilding = setupBuilding(_buildingPlacePoint.position, DataLoader.Buildings[typeId], _buildingPlacePoint);

        _currentBuilding = Instantiate(_unconstructBuildingPrefab, Vector3.zero, Quaternion.identity, _buildingPlacePoint);
        var constructionController = _currentBuilding.GetComponent<ConstructionController>();
        constructionController.Init(DataLoader.Buildings[typeId]);
         _currentBuilding.GetComponent<Team>().Number=_teamTag;
        _currentBuilding.transform.position = _buildingPlacePoint.position;
        _currentBuilding.transform.Translate(Vector3.up * constructionController.Height / 2);
        _currentState = BuildingState.HeroAttached;
    }
    public void PlaceAssepted()
    {
        _currentBuilding.transform.SetParent(this.transform);

        var building = setupBuilding(_currentBuilding.transform.position, DataLoader.Buildings[(int)_currentBuildingTypeId], this.transform);

        GameObject.Destroy(_currentBuilding.gameObject);
        _currentBuildingTypeId = null;
        _currentState = BuildingState.Idle;

        _currentBuilding = null;
        BuildingsInProcess.Add(building);

        NewBuildingIsInProcess?.Invoke(BuildingsInProcess[BuildingsInProcess.Count - 1]);
        building.GetComponent<ConstructionController>().StartConstruction();
    }
    public void PlaceDeclined()
    {
        _currentBuilding.SetActive(false);
        Destroy(_currentBuilding);
        _currentBuilding = null;
        _currentState = BuildingState.Idle;
    }

    private void onBuildingFinished(GameObject building)
    {
        if (BuildingsInProcess.Contains(building))
        {
            Destroy(building.GetComponent<BuildingPlaceAvailible>());
            var constructionController = building.GetComponent<ConstructionController>();
            var spawnType = constructionController.GetSpawnType();
            Destroy(constructionController);
            if (spawnType == SpawnTypes.Order)
                building.GetComponent<ProductSpawner>().enabled = true;
            else if (spawnType == SpawnTypes.FreeSpawn)
                building.GetComponent<TempEnemySpawner>().enabled=true;
            building.GetComponent<SpriteRenderer>().sortingOrder = 0;
            BuildingsInProcess.Remove(building);
        }
    }
    private GameObject setupBuilding(Vector3 position, BuildingData buildingData, Transform parent)
    {
        var building = Instantiate(_unconstructBuildingPrefab, Vector3.zero, Quaternion.identity, parent);
        building.GetComponentInChildren<Canvas>().sortingOrder = 2;
        building.GetComponent<ProductSpawner>().Init(buildingData.TypeId);
        building.GetComponent<Team>().Number=_teamTag;
        var constructionController = building.GetComponent<ConstructionController>();
        building.transform.position = position;
        constructionController.Init(buildingData);
        return building;
    }
    public void InstantBuild(Vector3 position, BuildingData buildingData, bool main)
    {
        var building = setupBuilding(position, buildingData, this.transform);
        if (main == true)
            building.AddComponent<IsMainBuilding>();
        var constructionController = building.GetComponent<ConstructionController>();
        building.transform.Translate(Vector3.up * constructionController.Height / 2);
        BuildingsInProcess.Add(building);
        constructionController.InstantConstruction();
    }
}
