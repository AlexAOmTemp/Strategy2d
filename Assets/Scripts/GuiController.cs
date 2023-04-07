using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    //[SerializeField] private float _buttonHeight = 30;
    [SerializeField] private GameObject _buildingsPanel;
    [SerializeField] private GameObject _zoneSeparatorController;

    private List<GameObject> _buttons = new List<GameObject>();
    private List<List<GameObject>> _buttonsSets = new List<List<GameObject>>();

    private Vector3 _basePanelPosition;
    private void Start()
    {
        _basePanelPosition = _buildingsPanel.transform.position;
        ZoneSeparatorController.ZoneIsChangedTo += onZoneChanged;
        InitGuiForLevel();
    }
    public void InitGuiForLevel()
    {
        createButtons();
    }
    private void createButtons()
    {
        var buttransf = _buttonPrefab.GetComponent<RectTransform>();
        _buttonsSets.Clear();
        if (_buttonPrefab == null)
        {
            Debug.LogError("GuiController: Button Prefab doesn't set");
            return;
        }
        foreach (BuildingData building in DataLoader.Buildings)
        {
            if (building.CanBeBuild == false)
                continue;
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().SetText(building.Name);
            button.GetComponent<BuildingButtons>().Id = building.Id;
         
            foreach (int zoneId in building.BuildInZones)
            {
                while (zoneId > _buttonsSets.Count-1)
                    _buttonsSets.Add(new List<GameObject>());
                _buttonsSets[zoneId].Add(button);
            }
            _buttons.Add(button);
            button.SetActive(false);


        }
        foreach (var button in  _buttonsSets[0])
        {
            button.SetActive( true);
        }
    }
  

    private void onZoneChanged(int currentZone)
    {
        foreach (var button in _buttons)
            button.SetActive(false);
        foreach (var button in _buttonsSets[currentZone])
            button.SetActive(true);
    }
}
