using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSelfBuff", menuName = "Custom Assets/Self Buff")]
public class SelfBuffAbility : Ability {
    [HideInInspector] public Buff buff;           //buff to apply
    public float duration;      //how long to apply the buff for
    public int effectID;   //effect name that corresponds to the line in player JSON
    public float effectAmount;  //how much of the effect to apply
  //  [SerializeField] private int buffId;

    public override string ToString() {
        return base.ToString() +
            "Duration: " + duration + "\n" +
            "Buff: " + buff.ToString();
    }
    //casts the ability by applying the buff to the player
    public override List<GameObject> Cast(Vector3 instantiatePosition, Vector3 mousePos, Vector3 offset, Collider2D casterCollider) {
        var player = casterCollider.GetComponent<GameCharacter>();
        buff.SetDuration(duration);
        player.BuffManager.ApplyBuff(buff);
        return null;
    }

   // public override void Init(GameCharacter caster) {
   //     base.Init(caster);
   //     buff = AbilityCollectionSingleton.Instance.GetBuffCopyByID(effectID);
   //     buff.SetDuration(duration);
   ////     buff.Init(caster);
        
   // }

    public override Ability CopyInstance() {

        var ability = CreateInstance<SelfBuffAbility>();
        ability.buff = buff;    
        CopyTo(ability);

        return ability;
    }
}
