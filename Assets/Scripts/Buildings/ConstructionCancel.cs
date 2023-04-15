using UnityEngine;
using UnityEngine.UI;

public class ConstructionCancel : MonoBehaviour
{
    [SerializeField] private GameObject _building;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    private void TaskOnClick()
    {
        GameObject.Destroy(_building);
    }
}
