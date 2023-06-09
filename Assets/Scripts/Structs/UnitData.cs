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
    public string[] ProductsIn { get; set; }
    public float ProductionTime { get; set; }
    public int TypeId { get; set; }
    public string Name { get; set; }
    public string Race { get; set; }
    public float RunSpeed { get; set; }
    public string Owner { get; set; }
    public bool CanFly { get; set; }
    public bool CanMove { get; set; }
    public bool CanAct { get; set; }
    public string BehaviorModel { get; set; }
    public float? Health { get; set; }
    public float? MeleeAttackSpeed { get; set; }
    public float? MeleeAttackDamage { get; set; }
    public float? MeleeAttackDistance { get; set; }
    public float? RangeAttackSpeed { get; set; }
    public float? RangeAttackDamage { get; set; }
    public float? RangeAttackDistance { get; set; }
    public float? ConstructionEfficiency { get; set; }
    public Sprite SpriteAlive { get; set; }
    public Sprite SpriteDead { get; set; }
    public Sprite Icone { get; set; }
    public string Description { get; set; }
}