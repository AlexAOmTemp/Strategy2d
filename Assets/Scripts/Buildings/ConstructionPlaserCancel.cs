using UnityEngine;
using UnityEngine.UI;

public class ConstructionPlaserCancel : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	private void Start()
	{
		var level = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>().PlayerLevel;
		_buildingSpawner = level.GetComponentInChildren<BuildingSpawner>(); 
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	private void TaskOnClick()
	{
		_buildingSpawner.PlaceDeclined();
	}
}
