using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scientist : BaseCharacter
{
    public bool detectedPlayer;
    public Scientist() 
    {
        AddBehavior(BehaviorType.Health, new HealthState());
        AddBehavior(BehaviorType.Move, new Move());
    }
}
