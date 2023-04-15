using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructButton : MonoBehaviour
{
	private BuildingSpawner _buildingSpawner;
	public int? Id { set; get; }
	// Start is called before the first frame update
	void Start()
	{
		_buildingSpawner = GameObject.Find("BuildingProcessor").GetComponent<BuildingSpawner>(); ;
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
