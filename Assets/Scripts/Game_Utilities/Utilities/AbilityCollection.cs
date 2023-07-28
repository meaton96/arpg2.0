using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AbilityCollection", menuName = "Custom Assets/Utilities/Ability Collection")]
public class AbilityCollection : ScriptableObject
{
    [SerializeField]
    SerializableDictionary<int, Ability> allAbilities;
    [SerializeField]
    SerializableDictionary<int, Buff> allBuffsAndDebuffs;

    //TODO:
    //change copyInstance()
    public Ability GetAbilityByID(int id) {
        Ability ability = null;
        try {
            ability = allAbilities[id];
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return ability;
    }
    public Buff GetBuffByID(int id) {
        Buff buff = null;
        try {
            buff = allBuffsAndDebuffs[id];
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return buff;
    }
}
