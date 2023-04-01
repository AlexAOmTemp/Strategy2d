using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GuiController : MonoBehaviour
{
    [SerializeField] private GameObject _buttonPrefab;
    [SerializeField] private float _buttonHeight = 30;
    [SerializeField] private GameObject _buildingsPanel;
   
    private List<GameObject> _buttons = new List<GameObject>();
    private int _activeButtonsCounter = 0;

    private Vector3 _basePanelPosition;
    private void Start()
    {
        _basePanelPosition = _buildingsPanel.transform.position;
        CreateButtons();
    }
    public void TogglePanel()
    {
        if (_buildingsPanel == null)
        { 
            Debug.LogError("GuiController: Buildings Panel doesn't set");
            return;
        }

        if (_buildingsPanel.activeSelf == true)
            _buildingsPanel.SetActive ( false) ;
        else
        {
            
            _buildingsPanel.SetActive(true);
            _buildingsPanel.transform.position = _basePanelPosition;
            _buildingsPanel.transform.Translate(Vector3.up * (_buttonHeight + 2) * _activeButtonsCounter);
        }
    }
    private void CreateButtons()
    {
        var buttransf = _buttonPrefab.GetComponent<RectTransform>();
        
        if (_buttonPrefab == null)
        {
            Debug.LogError("GuiController: Button Prefab doesn't set");
            return;
        }
        for (int i =0; i<5; i++)
        {
            var button = Instantiate(_buttonPrefab, Vector3.zero, Quaternion.identity, _buildingsPanel.transform);
            button.GetComponentInChildren<TextMeshProUGUI>().SetText($"Button {i}");
            _activeButtonsCounter++;
            _buttons.Add(button);
        }   
    }

    /*
    void CreateButtons()
    {
        var button = Instantiate(RoomButton, Vector3.zero, Quaternion.identity, _buildingsPanel) as Button;
        var rectTransform = button.GetComponent<RectTransform>();
        //rectTransform.SetParent(Canvas.transform);
        //rectTransform.offsetMin = Vector2.zero;
        //rectTransform.offsetMax = Vector2.zero;
        //button.onClick.AddListener(SpawnPlayer);
    }*/
}
