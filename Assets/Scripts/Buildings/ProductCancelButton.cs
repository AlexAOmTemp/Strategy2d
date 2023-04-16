using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductCancelButton : MonoBehaviour
{
    private ProductSpawner _spawner;
    public void Init(ProductSpawner parent)
    {
        _spawner = parent;
    }
	void Start()
	{
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
		_spawner.cancelProduct(this.gameObject);
	}
}
