using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCharacter : MonoBehaviour
{
    public CharacterType type;
    public bool isAlive;
    public Dictionary<BehaviorType, IBehavior> behaviorDictionary = new Dictionary<BehaviorType, IBehavior>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (KeyValuePair<BehaviorType, IBehavior> pair in behaviorDictionary)
        {
            Debug.Log($"Type: {pair.Key}, Behavior: {pair.Value}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public T GetBehavior<T>(BehaviorType type) where T : class, IBehavior
    {
        if (behaviorDictionary.TryGetValue(type, out var behavior))
            return behavior as T;
        return null;
    }
    public void AddBehavior(BehaviorType type, IBehavior behavior)
    {
        if (!behaviorDictionary.ContainsKey(type))
            behaviorDictionary.Add(type, behavior);
    }
}
public enum CharacterType
{
    Player,
    Scientist,
    SpecialForce
}
public enum BehaviorType
{
    Health,
    Attack,
    Move
}
