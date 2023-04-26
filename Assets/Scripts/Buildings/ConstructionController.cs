using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
public class ConstructionController : MonoBehaviour
{
    [SerializeField] private GameObject _panelPlaced;
    [SerializeField] private GameObject _panelUnplaced;

    private TextMeshProUGUI _timeLeftText;
    private bool _constructionFinished = false;
    private bool _constructionStarted = false;
    private int _constructionPhasesQuantity;
    private float _currentConstructionTime;
    private float _oneConstructionPhaseDuration;
    private int _currentConstructionPhase;
    private SpriteRenderer _spriteRenderer;
    private List<GameObject> _workersInvolved = new List<GameObject>();
    private BuildingData _data;


    public delegate void ConstructionFinished(GameObject Building);
    public static event ConstructionFinished ConstructionIsFinished;

    public float Height { get; private set; }

    public void Init(BuildingData buildingData)
    {
        _data = buildingData;
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _data.SpriteFinished;
        var size = _spriteRenderer.sprite.bounds.size;
        this.GetComponent<BoxCollider2D>().size = size;
        Height = size.y;
        this.name = _data.Name;
        _panelUnplaced.transform.Find("NameText").GetComponent<TextMeshProUGUI>().SetText(_data.Name);
        _panelUnplaced.transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>().SetText(buildingData.Description);
        _panelUnplaced.transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>().SetText(buildingData.BuildingTime.ToString());
        _timeLeftText = _panelPlaced.transform.Find("TimeLeft").GetComponent<TextMeshProUGUI>();
    }
    public void NewWorkerInvolvedInConstruction(GameObject worker)
    {
        _workersInvolved.Add(worker);
    }
    public void WorkerLeftConstruction(GameObject worker)
    {
        if (_workersInvolved.Contains(worker))
            _workersInvolved.Remove(worker);
        else
            Debug.LogError("BuildingController: worker was't involved in construction but try to leave it");
    }
    public SpawnTypes GetSpawnType()
    {
        return _data.SpawnType;
    }
    private void Update()
    {
        if (_constructionStarted == true)
        {
            if (_constructionFinished == false && _workersInvolved.Count > 0)
            {
                _currentConstructionTime += _workersInvolved.Count * Time.deltaTime; //every worker increases building time for 1 per second
                _timeLeftText.SetText((_data.BuildingTime - _currentConstructionTime).ToString("F1"));
                if ((int)(_currentConstructionTime / _oneConstructionPhaseDuration) > _currentConstructionPhase)
                {
                    _currentConstructionPhase++;
                    if (_currentConstructionPhase == _constructionPhasesQuantity)
                    {
                        _constructionStarted = false;
                        _constructionFinished = true;
                        _spriteRenderer.sprite = _data.SpriteFinished;
                        _panelPlaced.SetActive(false);
                        ConstructionIsFinished?.Invoke(this.gameObject);
                    }
                    else
                    {
                        _spriteRenderer.sprite = _data.SpritesUnfinished[_currentConstructionPhase];
                    }
                }
            }
        }
    }
    public void StartConstruction()
    {
        _constructionPhasesQuantity = _data.SpritesUnfinishedCount;
        _oneConstructionPhaseDuration = _data.BuildingTime / _constructionPhasesQuantity;
        if (_constructionPhasesQuantity > 0)
        {
            _spriteRenderer.sprite = _data.SpritesUnfinished[0];
            _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
            _panelUnplaced.SetActive(false);
            _panelPlaced.SetActive(true);
        }
        _constructionStarted = true;
    }
    public void InstantConstruction()
    {
        _panelUnplaced.SetActive(false);
        _constructionStarted = false;
        _constructionFinished = true;
         _spriteRenderer.color = new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1);
        _spriteRenderer.sprite = _data.SpriteFinished;
        _panelPlaced.SetActive(false);
        ConstructionIsFinished?.Invoke(this.gameObject);
    }
}
