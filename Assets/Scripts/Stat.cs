using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    // Start is called before the first frame update
    public string Name { get; private set; }
    public float CurrentValue { get; private set; }
    public float BaseValue { get; private set; }
    public Affector Affector { get; private set; } = new Affector();


}
