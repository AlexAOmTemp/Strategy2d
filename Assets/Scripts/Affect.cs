using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affect 
{
    private static int IdCounter = 0;
    public int Id { get; private set; }
    public float Value { get; private set; }
    public Affect(float value)
    {
        Value = value;
        Id = IdCounter;
        IdCounter++;
    }
}
