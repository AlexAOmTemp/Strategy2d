using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneSeparatorController : MonoBehaviour
{
    public int ZoneId { get; set; }
    public static int CurrentZone { get; private set; } = 0;
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.transform.position.x > transform.position.x)
                CurrentZone = ZoneId + 1;
            else
                CurrentZone = ZoneId;
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 128, 32), $"Zone = {CurrentZone} ");
    }
}
