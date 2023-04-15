using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
	private ProductSpawner _productSpawner;
	private UnitData _unitData;
	
	public void Init(UnitData unitData)
	{
		_unitData = unitData;
	}
	public void SetProductSpawner(ProductSpawner productSpawner)
	{
		_productSpawner = productSpawner;
	}
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		_productSpawner.createProduct(_unitData);
	}
}
