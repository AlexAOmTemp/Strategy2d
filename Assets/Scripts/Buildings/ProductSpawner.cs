using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unitPrefab;
    private Transform _unitsPlaced;
    private GuiController _guiController;
    public int BuildingTypeId { get; private set; }
    public int Id { get; private set; }

    public List<UnitData> ProductionQueue { get; private set; } = new List<UnitData>();
    private List<Button> _cancelButtons = new List<Button>();
    private List<Button> _buildButtons = new List<Button>();
    private float _currentProductionTime = 0;
    public void Init(int typeId, int id)
    {
        BuildingTypeId = typeId;
        Id = id;
    }
    private void Awake()
    {
        _guiController = GameObject.Find("ButtonController").GetComponent<GuiController>();
        _unitsPlaced = GameObject.Find("UnitsPlaced").transform;
    }
    public void createProduct(UnitData unitData)
    {
        if (ProductionQueue.Count == 0)
            _currentProductionTime = unitData.BaseProductionTime;
        ProductionQueue.Add(unitData);
    }
    public void cancelProduct()
    { 
    }
    private void Update()
    {
        if (ProductionQueue.Count > 0)
        {
            _currentProductionTime -= Time.deltaTime;
            if (_currentProductionTime <= 0)
            {
                productReady(ProductionQueue[0]);
                ProductionQueue.RemoveAt(0);
                if (ProductionQueue.Count > 0)
                    _currentProductionTime = ProductionQueue[0].BaseProductionTime;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.isActiveAndEnabled && collision.CompareTag("Player"))
        {
            _guiController.BuildingEntered(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (this.isActiveAndEnabled && collision.CompareTag("Player"))
        {
            _guiController.BuildingExited();
        }
    }
    private void productReady(UnitData unitData)
    {
        var unit = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity, _unitsPlaced.transform);
        unit.transform.position = this.transform.position;
        unit.GetComponent<UnitMain>().Init(unitData);
    }

}
