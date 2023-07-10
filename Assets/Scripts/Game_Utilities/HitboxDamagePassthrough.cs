using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDamagePassthrough : MonoBehaviour
{

    public virtual float CalculateDamage() {
        //Debug.Log(ability.CalculateDamage());
        //Debug.Log(ability.CalculateDamage(caster));
        var parent = transform.parent.gameObject;
        var dam = parent.GetComponent<SpawnedSpellAnimation>().CalculateDamage();
        Debug.Log(dam);
        return dam;


    }

}
