using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCollectionSingleton : MonoBehaviour {
    public static AbilityCollectionSingleton Instance;
    [SerializeField] private AbilityCollection abilityCollection;
    // Start is called before the first frame update
    void Start() {
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
        ability.Init();
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
        buff.Init();
        return buff;
    }
    public Ability GetAbilityCopyByID(int id) {
        Ability b = GetAbilityByID(id).CopyInstance();
        b.Init();
        return b;
    }
    public Buff GetBuffCopyByID(int id) {
        Buff b = GetBuffByID(id).CopyInstance();
        b.Init();
        return b;
    }
    public Buff GetBuffCopy(Buff buff) {
        if (buff == null) throw new System.ArgumentNullException("buff is null");
        Buff b = buff.CopyInstance();
        b.Init();
        return b;
    }
    public Ability GetAbilityCopy(Ability ability) {
        if (ability == null) throw new System.ArgumentNullException("ability is null");
        Ability a = ability.CopyInstance();
        a.Init();
        return a;
    }
}
