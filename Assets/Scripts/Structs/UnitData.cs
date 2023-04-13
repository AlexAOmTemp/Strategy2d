using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitBehaviorModel
{
    Worker,
    Warrior,
    RangeWarrior
}
public struct UnitData
{
    public int Id { get; set; }
    public int TypeId { get; set; }
    public string Name { get; set; }
    public float RunSpeed { get; set; }
    public string Owner { get; set; }
    public bool CanFly { get; set; }
    public UnitBehaviorModel BehaviorModel { get; set; }
    public float? MeleeAttackSpeed { get; set; }
    public float? MeleeAttackDamage { get; set; }
    public float? MeleeAttackRange { get; set; }
    public float? RangeAttackSpeed { get; set; }
    public float? RangeAttackDamage { get; set; }
    public float? RangeAttackRange { get; set; }
    public float? ConstructionEfficiency { get; set; }
    public Sprite SpriteAlife { get; set; }
    public Sprite SpriteDead { get; set; }
}