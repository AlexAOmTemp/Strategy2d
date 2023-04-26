using UnityEngine;

public class IsMainBuilding : MonoBehaviour
{
    private int _teamNumber;
    public void Start()
    {
       _teamNumber = this.GetComponent<Team>().Number;
    }   
    public void OnDestroy()
    {
        Debug.Log($"Team {_teamNumber} lose the game!!!") ;
    }
}
