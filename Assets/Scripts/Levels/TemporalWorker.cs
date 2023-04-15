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
        ConstructionController.ConstructionIsFinished += onBuildingFinished;
    }
    void onNewBuildingStarted(GameObject building)
    {
        var bp = building.GetComponent<ConstructionController>(); 
        bp.NewWorkerInvolvedInConstruction(this.gameObject);
    }
    void onBuildingFinished(GameObject building)
    {
        var bp = building.GetComponent<ConstructionController>();
        bp.WorkerLeftConstruction(this.gameObject);
        _buildingSpawner.BuildingFinished(building);
    }

}
