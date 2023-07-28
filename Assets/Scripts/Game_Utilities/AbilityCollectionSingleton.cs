using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollectionSingleton : MonoBehaviour
{
    public static AbilityCollectionSingleton Instance;
    [SerializeField] private AbilityCollection abilityCollection;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        else {
            Instance = this;
        }
    }
    public Ability GetAbilityByID(int id) {
        Ability ability = null;
        try {
            ability = abilityCollection.GetAbilityByID(id); 
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return ability;
    }
    public Buff GetBuffByID(int id) {
        Buff buff = null;
        try {
            buff = abilityCollection.GetBuffByID(id);
        }
        catch (KeyNotFoundException e) {
            Debug.Log(e);
        }
        return buff;
    }
    public Ability GetAbilityCopyByID(int id) {
        return GetAbilityByID(id).CopyInstance();
    }
    public Buff GetBuffCopyByID(int id) {
        return GetBuffByID(id).CopyInstance();
    }
}
