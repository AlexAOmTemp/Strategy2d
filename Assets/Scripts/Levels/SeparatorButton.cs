using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeparatorButton : MonoBehaviour
{
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
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandForwardIcon;
				break;
			case 3:
				GetComponent<Image>().sprite = currentLevelScript.Data.SeparatorCommandProtectWallIcon;
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
	}
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		switch (_buttonNumber)
		{
			case 0:
				_separatorController.MoveBack();
				break;
			case 1:
				_separatorController.MoveLeftPoint();
				break;
			case 2:
				_separatorController.MoveRightPoint();
				break;
			case 3:
				_separatorController.MoveForward();
				break;
			default:
				Debug.LogError("SeparatorButton : no command for this number");
				break;
		}
	}
}
