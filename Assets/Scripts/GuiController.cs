using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private GameObject _buildingsPanel;

    private List<GameObject> _activeButtonsSet;
    private List<List<GameObject>> _constructionButtonsSets = new List<List<GameObject>>();
    private List<List<GameObject>> _productButtonsSets = new List<List<GameObject>>();
    private int _currentZone = 0;

    private RectTransform _buttonRectTransform;
    private Vector3 _basePanelPosition;
    private void Start()
    {
        _basePanelPosition = _buildingsPanel.transform.position;
        ZoneSeparatorController.ZoneIsChangedTo += onZoneChanged;
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
        _activeButtonsSet = _constructionButtonsSets[0];
        onZoneChanged(_currentZone);
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

            foreach (int zoneId in building.BuildInZones)
            {
                while (zoneId > _constructionButtonsSets.Count - 1)
                    _constructionButtonsSets.Add(new List<GameObject>());
                _constructionButtonsSets[zoneId].Add(button);
            }
            button.SetActive(false);
        }
    }
    private void createUnitButtons()
    {
        _productButtonsSets.Clear();
        foreach (UnitData unit in DataLoader.Units)
        {
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(unit.Name);
            var productButton = button.AddComponent<ProductButton>();
            productButton.Init(unit);
            foreach (string buildingName in unit.ProductsIn)
            {
                BuildingData building = DataLoader.Buildings.Find(i => i.Name == buildingName);
                while (building.TypeId > _productButtonsSets.Count - 1)
                    _productButtonsSets.Add(new List<GameObject>());
                _productButtonsSets[building.TypeId].Add(button);
            }
            button.SetActive(false);
        }
    }
    private void onZoneChanged(int currentZone)
    {
        _currentZone = currentZone;
        foreach (var button in _activeButtonsSet)
            button.SetActive(false);

        if (currentZone < _constructionButtonsSets.Count)
        {
            _activeButtonsSet = _constructionButtonsSets[currentZone];
            foreach (var button in _activeButtonsSet)
                button.SetActive(true);
        }
    }
    public void BuildingEntered(ProductSpawner spawner)
    {
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
        onZoneChanged(_currentZone);
    }
}
