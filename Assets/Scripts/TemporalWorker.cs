using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalWorker : MonoBehaviour
{
    private BuildingSpawner _buildingSpawner;
     
    void Start()
    {
        _buildingSpawner = GameObject.Find("BuildingProcessor").GetComponent< BuildingSpawner>();
        if (_buildingSpawner == null)
            Debug.LogError("TemporalWorker: buildingSpawner not finded");
        BuildingSpawner.NewBuildingIsInProcess += onNewBuildingStarted;
        BuildingParameters.BuildingIsFinished += onBuildingFinished;
    }
    void onNewBuildingStarted(GameObject building)
    {
        var bp = building.GetComponent<BuildingParameters>(); 
        bp.NewWorkerInvolvedInBuilding(this.gameObject);
    }
    void onBuildingFinished(GameObject building)
    {
        var bp = building.GetComponent<BuildingParameters>();
        bp.WorkerLivedBuilding(this.gameObject);
        _buildingSpawner.BuildingFinished(building);
    }

}
