using UnityEngine;
using UnityEngine.UI;

public class ConstructionPlaserCancel : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	private void Start()
	{
		_buildingSpawner = GameObject.Find("BuildingProcessor").GetComponent<BuildingSpawner>(); 
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	private void TaskOnClick()
	{
		_buildingSpawner.PlaceDeclined();
	}
}
