using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    [SerializeField] private GameObject _panelProduction;
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private Canvas _canvas;
    private Transform _queuePanel;
    private TextMeshProUGUI _currentProductName;
    private TextMeshProUGUI _currentProductTimeLeft;
    private TextMeshProUGUI _currentProductDescription;


    private Transform _unitsPlaced;
    private GuiController _guiController;

    public int BuildingTypeId { get; private set; }
    private bool _playerIn = false;
    private Collider2D _collision;
    public List<UnitData> ProductionQueue { get; private set; } = new List<UnitData>();
    private List<GameObject> _cancelButtons = new List<GameObject>();
    private float _currentProductionTime = 0;
    public void Init(int typeId)
    {
        BuildingTypeId = typeId;
    }
    private void Awake()
    {
        _guiController = GameObject.Find("ButtonController").GetComponent<GuiController>();
        _unitsPlaced = GameObject.Find("UnitsPlaced").transform;
        _queuePanel = _panelProduction.transform.Find("Queue");
        _currentProductName = _panelProduction.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        _currentProductTimeLeft = _panelProduction.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();
        _currentProductDescription = _panelProduction.transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        if (_playerIn)
            OnTriggerEnter2D(_collision);
    }
    public void createProduct(UnitData unitData)
    {
        if (ProductionQueue.Count == 0)
        {
            _currentProductionTime = unitData.ProductionTime;
            _panelProduction.SetActive(true);
            settingProductPanel(unitData);
        }
        ProductionQueue.Add(unitData);
        createCancelButton(unitData);
    }
    public void cancelProduct(GameObject cancelButton)
    {
        int index = _cancelButtons.IndexOf(cancelButton);
        ProductionQueue.RemoveAt(index);
        Destroy(_cancelButtons[index]);
        _cancelButtons.RemoveAt(index);
        if (index == 0 && ProductionQueue.Count > 0)
        {
            _currentProductionTime = ProductionQueue[0].ProductionTime;
            settingProductPanel(ProductionQueue[0]);
        }
        if (ProductionQueue.Count == 0)
            queueFinished();
    }
    private void Update()
    {
        if (ProductionQueue.Count > 0)
        {
            _currentProductionTime -= Time.deltaTime;
            _currentProductTimeLeft.SetText(_currentProductionTime.ToString("F1"));
            if (_currentProductionTime <= 0)
            {
                productReady(ProductionQueue[0]);
                ProductionQueue.RemoveAt(0);
                Destroy(_cancelButtons[0]);
                _cancelButtons.RemoveAt(0);
                if (ProductionQueue.Count > 0)
                {
                    settingProductPanel(ProductionQueue[0]);
                    _currentProductionTime = ProductionQueue[0].ProductionTime;
                }
                else
                    queueFinished();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log($"{collision.gameObject.GetComponent<Team>()}, {this.GetComponent<Team>()}");
            if (collision.gameObject.GetComponent<Team>().Number == this.GetComponent<Team>().Number)
            {
                _canvas.enabled = true;
                if (this.isActiveAndEnabled)
                    _guiController.BuildingEntered(this);
                else
                {
                    _playerIn = true;
                    _collision = collision;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") &&
            collision.GetComponent<Team>().Number == this.GetComponent<Team>().Number)
        {
            _canvas.enabled = false;
            _playerIn = false;
            if (this.isActiveAndEnabled)
                _guiController.BuildingExited();
        }
    }
    private void productReady(UnitData unitData)
    {
        var unit = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity, _unitsPlaced.transform);
        unit.transform.position = this.transform.position;
        unit.GetComponent<UnitMain>().Init(unitData);
        //unit.GetComponent<UnitBehavior>().SetCurrentPathPoint(PathPointsController.CurrentPathPoint);
        unit.GetComponent<Team>().Number = this.GetComponent<Team>().Number;
    }
    private void createCancelButton(UnitData unitData)
    {
        var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _queuePanel);
        var cancelButtonScript = button.AddComponent<ProductCancelButton>();
        cancelButtonScript.Init(this);
        Destroy(button.GetComponentInChildren<TextMeshProUGUI>());
        button.GetComponent<Image>().sprite = unitData.Icone;
        var rectTransform = button.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(60, 60);
        _cancelButtons.Add(button);
    }
    private void queueFinished()
    {
        _panelProduction.SetActive(false);
    }
    private void settingProductPanel(UnitData unitData)
    {
        _currentProductName.SetText(unitData.Name);
        _currentProductTimeLeft.SetText(_currentProductionTime.ToString("F1"));
        _currentProductDescription.SetText(unitData.Description);
    }
}
