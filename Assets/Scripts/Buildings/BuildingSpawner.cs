using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unconstructBuildingPrefab;
    [SerializeField] private GameObject _buildingPrefab;
    [SerializeField] private Transform _buildingPlacePoint;
    private GameObject _level;
    public List <GameObject> BuildingsInProcess = new List<GameObject>();
    private GameObject _currentBuilding;
    private int? _currentBuildingId;
    public delegate void NewBuildingInProcess(GameObject Building);
    public static event NewBuildingInProcess NewBuildingIsInProcess;

    enum BuildingState 
    {
        Idle,
        HeroAttached,
        Placed
    }
    private BuildingState _currentState = BuildingState.Idle;
    private void Awake()
    {
        LevelController.LevelIsCreated += onLevelCreated;
    }
  
    public void CreateBuildingPlacer(int id)
    {
        if (_currentState != BuildingState.Idle)
            PlaceDeclined();
        if (id > DataLoader.Buildings.Count - 1)
        {
            Debug.LogError("BuildingSpawner: tried to create an unexistant building");
            return;
        }
        _currentBuildingId = id;
        _currentBuilding = Instantiate(_unconstructBuildingPrefab, Vector3.zero, Quaternion.identity, _buildingPlacePoint);
        var param = _currentBuilding.GetComponent<ConstructionController>();
        param.Init(DataLoader.Buildings[id].Name,DataLoader.Buildings[id]);
        _currentBuilding.transform.position = _buildingPlacePoint.position;
        _currentBuilding.transform.Translate(Vector3.up * param.Height / 2);
        _currentState = BuildingState.HeroAttached;
    }
    public void PlaceAssepted()
    {
        _currentBuilding.transform.SetParent(_level.transform);
   
        var building = Instantiate(_unconstructBuildingPrefab, Vector3.zero, Quaternion.identity, _level.transform);
        var param = building.GetComponent<ConstructionController>();
        building.GetComponentInChildren<Canvas>().sortingOrder = 2;
        if (_currentBuildingId!=null)
        {
            param.Init(DataLoader.Buildings[(int)_currentBuildingId].Name,DataLoader.Buildings[(int)_currentBuildingId]);
            var spawner = building.GetComponent<ProductSpawner>();
            spawner.Init((int)_currentBuildingId);
        }

        building.transform.position = _currentBuilding.transform.position;
        building.GetComponentInChildren<Canvas>().sortingLayerName = "Buildings";
        GameObject.Destroy(_currentBuilding.gameObject);
        _currentBuildingId = null;
        _currentState = BuildingState.Idle;
        
        _currentBuilding = null;
        BuildingsInProcess.Add(building);
        
        NewBuildingIsInProcess?.Invoke(BuildingsInProcess[BuildingsInProcess.Count-1]);
        building.GetComponent<ConstructionController>().StartConstruction();
    }
    public void PlaceDeclined()
    {
        _currentBuilding.SetActive(false);
        Destroy(_currentBuilding);
        _currentBuilding = null;
        _currentState = BuildingState.Idle;
    }
    public void BuildingFinished(GameObject building)
    {
        if (BuildingsInProcess.Contains(building))
        {
            onBuildingFinished(building);
            BuildingsInProcess.Remove(building);
        }
        else
            Debug.LogError("BuildingSpawner: try to remove unexisting building");
    }
    private void onLevelCreated(GameObject level)
    {
        _level = level;
    }
    private void onBuildingFinished(GameObject building)
    {
        Destroy(building.GetComponent<BuildingPlaceAvailible>());
        Destroy(building.GetComponent<ConstructionController>());
        building.GetComponent<ProductSpawner>().enabled=true;
        building.GetComponent<SpriteRenderer>().sortingOrder = 0;
        
    }
}
