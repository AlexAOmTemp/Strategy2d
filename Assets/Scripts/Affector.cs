using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Affector : MonoBehaviour
{
    public delegate void ValueChanged();
    public event ValueChanged ValueIsChanged;

    public float TotalIncrease { get; private set; } = 1;
    public float TotalMultiply { get; private set; } = 1;
    public float TotalAdd { get; private set; } = 0;
    public float TotalBaseAdd { get; private set; } = 0;

    public List<List<Affect>> AllAffects { get; private set; } = new List<List<Affect>>();
    public List<Affect> Increasers { get; private set; } = new List<Affect>();
    public List<Affect> Decraisers { get; private set; } = new List<Affect>();
    public List<Affect> Multipliers { get; private set; } = new List<Affect>();
    public List<Affect> Dividers { get; private set; } = new List<Affect>();
    public List<Affect> Additors { get; private set; } = new List<Affect>();
    public List<Affect> Substractors { get; private set; } = new List<Affect>();
    public List<Affect> BaseAdditors { get; private set; } = new List<Affect>();
    public List<Affect> BaseSubstractors { get; private set; } = new List<Affect>();
    private void Awake()
    {
        AllAffects.Add(Increasers);
        AllAffects.Add(Decraisers);
        AllAffects.Add(Multipliers);
        AllAffects.Add(Dividers);
        AllAffects.Add(Additors);
        AllAffects.Add(Substractors);
        AllAffects.Add(BaseAdditors);
        AllAffects.Add(BaseSubstractors);
    }
    public float ApplyAffector(float baseValue)
    {
        return (((baseValue + TotalBaseAdd) * TotalIncrease + TotalAdd) * TotalMultiply);
    }
    public void AddIncreaser(Affect affect)
    {
        Increasers.Add(affect);
        TotalIncrease += affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void RemoveIncreaser(Affect affect)
    {
        if (Increasers.Contains(affect))
            Increasers.Remove(affect);
        else
            Debug.LogError($"{this.gameObject} Affector: try to remove unexisted Increaser");
        TotalIncrease -= affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void AddDecraiser(Affect affect)
    {
        Decraisers.Add(affect);
        TotalIncrease -= affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void RemoveDecraiser(Affect affect)
    {
        if (Decraisers.Contains(affect))
            Decraisers.Remove(affect);
        else
            Debug.LogError($"{this.gameObject} Affector: try to remove unexisted Decraiser");
        TotalIncrease += affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void AddMultiplier(Affect affect)
    {
        Multipliers.Add(affect);
        TotalMultiply += affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void RemoveMultiplier(Affect affect)
    {
        if (Multipliers.Contains(affect))
            Multipliers.Remove(affect);
        else
            Debug.LogError($"{this.gameObject} Affector: try to remove unexisted Multiplier");
        TotalMultiply -= affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void AddDivider(Affect affect)
    {
        Dividers.Add(affect);
        TotalMultiply -= affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }
    public void RemoveDivider(Affect affect)
    {
        if (Dividers.Contains(affect))
            Dividers.Remove(affect);
        else
            Debug.LogError($"{this.gameObject} Affector: try to remove unexisted Divider");
        TotalMultiply += affect.AffectData.Value;
        ValueIsChanged?.Invoke();
    }

}
