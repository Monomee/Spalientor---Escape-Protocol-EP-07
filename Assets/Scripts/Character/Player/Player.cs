using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    public List<GameObject> inventory;
    public bool isStealth;
    public Player()
    {
        AddBehavior(BehaviorType.Attack, new Attack());
        AddBehavior(BehaviorType.Health, new HealthState());
    }
}
