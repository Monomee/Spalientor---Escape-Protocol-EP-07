using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialForce : BaseCharacter
{
    public bool detectedPlayer;
    public SpecialForce()
    {
        AddBehavior(BehaviorType.Health, new HealthState());
        AddBehavior(BehaviorType.Move, new Move());
        AddBehavior(BehaviorType.Attack, new Attack());
    }
}
