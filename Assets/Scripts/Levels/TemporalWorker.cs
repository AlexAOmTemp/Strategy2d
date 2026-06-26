using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalWorker : MonoBehaviour
{
    [SerializeField] private BuildingSpawner _buildingSpawner;

    private void Start()
    {
        BuildingSpawner.NewBuildingIsInProcess += onNewBuildingStarted;
        ConstructionController.ConstructionIsFinished += onBuildingFinished;
    }

    private void onNewBuildingStarted(GameObject building)
    {
        var bp = building.GetComponent<ConstructionController>();
        bp.NewWorkerInvolvedInConstruction(this.gameObject);
    }

    private void onBuildingFinished(GameObject building)
    {
        var bp = building.GetComponent<ConstructionController>();
        bp.WorkerLeftConstruction(this.gameObject);
    }
}