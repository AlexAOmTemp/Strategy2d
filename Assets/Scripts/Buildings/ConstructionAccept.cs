using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionAccept : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	void Start()
	{
		_buildingSpawner = GameObject.Find("BuildingProcessor").GetComponent<BuildingSpawner>(); 
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		_buildingSpawner.PlaceAssepted();
	}
}
