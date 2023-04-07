using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class BuildingController : MonoBehaviour
{
    private bool _isFinished = false;
    private bool _buildStarted = false;
    private int _buildingPhasesQuantity;
    private float _currentBuildingTime;
    private float _oneBuildingPhaseTime;
    private int _currentBuildingPhase;
    private SpriteRenderer _spriteRenderer;
    private List<GameObject> _workersInvolved = new List<GameObject>();
    private BuildingData _data;

   
    public delegate void BuildingFinished(GameObject Building);
    public static event BuildingFinished BuildingIsFinished;

    public float Height { get; private set; }

    public void Init(BuildingData buildingData)
    {
        _data = buildingData;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _data.SpriteFinished;
        var size = _spriteRenderer.sprite.bounds.size;
        this.GetComponent<BoxCollider2D>().size = size;
        Height = size.y;
    }
    public void NewWorkerInvolvedInBuilding(GameObject worker)
    {
        _workersInvolved.Add(worker);
    }
    public void WorkerLivedBuilding(GameObject worker)
    {
        if (_workersInvolved.Contains(worker))
            _workersInvolved.Remove(worker);
        else
            Debug.LogError("BuildingController: worker was't involved in building but try to leave it");
    }
    private void Update()
    {
        if (_buildStarted == true)
        {
            if (_isFinished == false && _workersInvolved.Count > 0)
            {
                _currentBuildingTime += _workersInvolved.Count * Time.deltaTime; //every worker increases building time for 1 per second
                if ((int)(_currentBuildingTime / _oneBuildingPhaseTime) > _currentBuildingPhase)
                {
                    _currentBuildingPhase++;
                    if (_currentBuildingPhase == _buildingPhasesQuantity)
                    {
                        _buildStarted = false;
                        _isFinished = true;
                        _spriteRenderer.sprite = _data.SpriteFinished;
                        BuildingIsFinished?.Invoke(this.gameObject);
                    }
                    else
                    {
                        _spriteRenderer.sprite = _data.SpritesUnfinished[_currentBuildingPhase];
                    }
                }
            }
        }
    }
    public void StartBuilding()
    {
        _buildingPhasesQuantity = _data.SpritesUnfinishedCount;
        _oneBuildingPhaseTime = _data.BuildingTime / _buildingPhasesQuantity;
        if (_buildingPhasesQuantity > 0)
            _spriteRenderer.sprite = _data.SpritesUnfinished[0];
        _buildStarted = true;
    }
 
}
