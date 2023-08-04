using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffApplicationAura : Aura
{
    [SerializeField] protected RangedAuraDetector AuraCollider; //aura collider

    public override void Init(GameCharacter caster) {
        base.Init(caster);
        AuraCollider.Buff = buff;
        
    }
}
