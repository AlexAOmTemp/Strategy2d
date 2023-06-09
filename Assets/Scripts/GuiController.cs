using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiController : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _buildingsPanel;
    private List<GameObject> _activeButtonsSet;
    private List<List<GameObject>> _constructionButtonsSets = new List<List<GameObject>>();
    private List<List<GameObject>> _productButtonsSets = new List<List<GameObject>>();
    private List<GameObject> _separatorButtonsSet = new List<GameObject>();
    private int _currentZone = 0;
    private int _countOfBuildingVisited;
    private Vector3 _basePanelPosition;
    private void Start()
    {
        _basePanelPosition = _buildingsPanel.transform.position;
        InitGuiForLevel();
    }
    public void InitGuiForLevel()
    {
        if (_buttonPrefab == null)
        {
            Debug.LogError("GuiController: Button Prefab doesn't set");
            return;
        }
        createConstructionButtons();
        createUnitButtons();
        createSeparatorButtons();
        _activeButtonsSet = _constructionButtonsSets[0];
        allBuildingsExited();
    }
    private void createConstructionButtons()
    {
        _constructionButtonsSets.Clear();
        foreach (BuildingData building in DataLoader.Buildings)
        {
            if (building.CanBeBuild == false)
                continue;
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            button.AddComponent<ConstructButton>();
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(building.Name);
            button.GetComponent<ConstructButton>().Id = building.TypeId;

            foreach (int zoneId in building.BuildInZonesWithId)
            {
                while (zoneId > _constructionButtonsSets.Count - 1)
                    _constructionButtonsSets.Add(new List<GameObject>());
                _constructionButtonsSets[zoneId].Add(button);
            }
            button.SetActive(false);
        }
    }
    private void createSeparatorButtons()
    {
        _separatorButtonsSet.Clear();
        for (int i = 0; i < 4; i++)
        {
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            var separatorButton = button.AddComponent<SeparatorButton>();
            separatorButton.Init(i);
            button.SetActive(false);
            _separatorButtonsSet.Add(button);
        }
    }
    private void createUnitButtons()
    {
        _productButtonsSets.Clear();
        foreach (UnitData unit in DataLoader.Units)
        {
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            var productButton = button.AddComponent<ProductButton>();
            productButton.Init(unit);
            foreach (string buildingName in unit.ProductsIn)
            {
                BuildingData building = DataLoader.Buildings.Find(i => i.Name == buildingName);
                if (building.Name == buildingName)
                {
                    //Debug.Log($"Search {buildingName} found {building.Name}");
                    while (building.TypeId > _productButtonsSets.Count - 1)
                        _productButtonsSets.Add(new List<GameObject>());
                    _productButtonsSets[building.TypeId].Add(button);
                }
                else
                    Debug.LogError($"GuiController: building {buildingName} doesn't exist");
            }
            button.SetActive(false);
        }
    }
    public void SeparatorEntered(GameObject separator)
    {
        _countOfBuildingVisited++;
        foreach (var button in _activeButtonsSet)
            button.SetActive(false);
        _activeButtonsSet = _separatorButtonsSet;
        foreach (var button in _activeButtonsSet)
        {
            button.GetComponent<SeparatorButton>().SetSeparator(separator.GetComponent<SeparatorController>());
            button.SetActive(true);
        }
    }
    public void SeparatorExited(int currentZone)
    {
        _countOfBuildingVisited--;
        _currentZone = currentZone;
           Debug.Log($"{_currentZone}");
        if (_countOfBuildingVisited == 0)
            allBuildingsExited();
    }
    public void BuildingEntered(ProductSpawner spawner)
    {
        _countOfBuildingVisited++;
        foreach (var button in _activeButtonsSet)
            button.SetActive(false);
        if (spawner.BuildingTypeId < _productButtonsSets.Count)
        {
            _activeButtonsSet = _productButtonsSets[spawner.BuildingTypeId];
            foreach (var button in _activeButtonsSet)
            {
                button.GetComponent<ProductButton>().SetProductSpawner(spawner);
                button.SetActive(true);
            }
        }
    }
    public void BuildingExited()
    {
        _countOfBuildingVisited--;
        if (_countOfBuildingVisited == 0)
            allBuildingsExited();
    }
    private void allBuildingsExited()
    {
        foreach (var button in _activeButtonsSet)
            button.SetActive(false);
        if (_currentZone < _constructionButtonsSets.Count)
        {
            _activeButtonsSet = _constructionButtonsSets[_currentZone];
            foreach (var button in _activeButtonsSet)
                button.SetActive(true);
        }
    }
}
