using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionAccept : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	void Start()
	{
		var level = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().CurrentLevel;
		if (level == null)
			Debug.LogError("ConstructButton: level not found"); 
		_buildingSpawner = level.GetComponent<BuildingSpawner>();
		
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		_buildingSpawner.PlaceAssepted();
	}
}
