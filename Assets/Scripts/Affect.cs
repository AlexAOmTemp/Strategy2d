using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AffectDurationType
{
    timed,
    WhileSourceExist,
    Permantent
}
public struct AffectData
{
    public int TypeId { get; set; }
    public float Value { get; set; }
    public string Name { get; set; }
    public AffectDurationType DurationType { get; set; }
    public Stat Duration { get; set; }
    public bool CanBeDispelled { get; set; }
}
public class Affect : MonoBehaviour
{
    private static int IdCounter=0;
    public int Id { get; private set; }
    public float TimeLeft { get; private set; }
    public AffectData AffectData { get; private set; }

    private void Awake()
    {
        Id = IdCounter;
        IdCounter++;
        TimeLeft = AffectData.Duration.CurrentValue;
    }
    public void Init(AffectData affectData)
    {
        AffectData = affectData;
    }
    private void Update()
    {
        switch (AffectData.DurationType)
        {
            case AffectDurationType.timed:
                TimeLeft -= Time.deltaTime;
                if (TimeLeft <= 0)
                {
                    
                }
                break;
            case AffectDurationType.WhileSourceExist:
                break;
            case AffectDurationType.Permantent:
                break;
        }
           

    }
}
