using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructButton : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	public int? Id { set; get; }
	void Start()
	{
		var level = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().PlayerLevel;
		if (level == null)
			Debug.LogError("ConstructButton: level not found"); 
		_buildingSpawner = level.GetComponent<BuildingSpawner>();
		if (Id == null)
		{
			Debug.LogError("ConstructButton: Id doesn't set");
		}
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		if (Id == null)
		{
			Debug.LogError("ConstructButton: Id doesn't set");
			return;
		}
		_buildingSpawner.CreateBuildingPlacer((int)Id);
	}
}
