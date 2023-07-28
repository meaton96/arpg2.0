using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMinionAbility", menuName = "Custom Assets/Summon/Minion")]
public class Minion : Ability {


    public override Ability CopyInstance() {
        Ability ability = CreateInstance<Minion>(); 
        CopyTo(ability);
        return ability;
    }
}
