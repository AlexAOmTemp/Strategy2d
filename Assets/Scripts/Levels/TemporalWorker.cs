using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalWorker : MonoBehaviour
{
    [SerializeField] private BuildingSpawner _buildingSpawner;
     
    void Start()
    {
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
    }

}
