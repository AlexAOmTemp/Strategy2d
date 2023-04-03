using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unconstructBuildingPrefab;
    [SerializeField] private GameObject _level;
    private Transform _buildingPlacePoint;
    public List <GameObject> BuildingsInProcess = new List<GameObject>();
    private GameObject _currentBuilding;
    enum BuildingState 
    {
        Idle,
        HeroAttached,
        Placed
    }
    private BuildingState _currentState = BuildingState.Idle;

    private void Start()
    {
        _buildingPlacePoint = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
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

        Sprite sprite = Resources.Load<Sprite>($"Sprites/Buildings/{DataLoader.Buildings[id].SpriteName}");
        if (sprite == null)
            Debug.LogError($"BuildingSpawner: Sprite Sprites/Buildings/{DataLoader.Buildings[id].SpriteName} doesn't exist");

        _currentBuilding = Instantiate(_unconstructBuildingPrefab, Vector3.zero, Quaternion.identity, _buildingPlacePoint);
       
        _currentBuilding.transform.position = _buildingPlacePoint.position;
        var data = _currentBuilding.GetComponent<BuildingData>();
        data.SetSprite(sprite);
        var size = _currentBuilding.GetComponent<SpriteRenderer>().bounds.size;
        Debug.Log($"Size is {size}");
        _currentBuilding.transform.Translate(Vector3.up* size.y/2);
        data.name = DataLoader.Buildings[id].Name;
        _currentState = BuildingState.HeroAttached;
    }
    public void PlaceAssepted()
    {
        _currentBuilding.transform.SetParent(_level.transform);
        foreach (Transform child in _currentBuilding.transform)
            GameObject.Destroy(child.gameObject);
        
        _currentState = BuildingState.Idle;
        BuildingsInProcess.Add(_currentBuilding);
        _currentBuilding = null;
    }
    public void PlaceDeclined()
    {
        _currentBuilding.SetActive(false);
        Destroy(_currentBuilding);
        _currentBuilding = null;
        _currentState = BuildingState.Idle;
    }
}
