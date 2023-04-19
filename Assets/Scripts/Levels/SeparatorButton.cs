using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeparatorButton : MonoBehaviour
{
	private PathPointsController _pointsController;
	private ZoneSeparatorController _separatorController;
	private int _buttonNumber; //0<- 1.| 2|. 3->
	
	public void Init(int buttonNumber)
	{
		_buttonNumber = buttonNumber;
		Destroy(this.GetComponentInChildren<TextMeshProUGUI>());
		var levelSpawner = GameObject.Find("LevelSpawner").GetComponent<LevelSpawner>();
		var currentLevelScript = levelSpawner.CurrentLevel.GetComponent<LevelController>();
		switch (buttonNumber)
		{
			case 0:
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandBackIcon;
				break;
			case 1:
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandBehindWallIcon;
				break;
			case 2:
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandProtectWallIcon;
				break;
			case 3:
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandForwardIcon;
				break;
		
			default:
				Debug.LogError("SeparatorButton : unexpected number of button");
				break;
		}

		var rectTransform = this.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y, rectTransform.sizeDelta.y);
	}
	public void SetSeparator(ZoneSeparatorController separatorController)
	{
		_separatorController = separatorController;
		_pointsController = separatorController.transform.GetComponentInParent<PathPointsController>();
		if (_pointsController == null)
			Debug.Log("SeparatorButton: _pointsController not found");
		if ( _buttonNumber == 0)
		{
			if (_separatorController.Id == 0)
				this.GetComponent<Button>().interactable = false;
			else
				this.GetComponent<Button>().interactable = true;
		}
	}
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		_pointsController.ChangeCurrentPathPoint(_separatorController.Id, _buttonNumber);
	}
}
