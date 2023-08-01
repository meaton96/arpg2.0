using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    protected const float MANA_TICKS_PER_SECOND = 4f;
    protected const float HEALTH_TICKS_PER_SECOND = 4f;
    protected float healthTimer = 0;
    protected float manaTimer = 0;
 //   [HideInInspector]public float currentHealth;
  //  [HideInInspector] public float maxHealth;
    [SerializeField] private StatManager statManager;
   // [HideInInspector] public float currentMana;
   // [HideInInspector] public float maxMana;

 


    public void Init(float maxHealth, float maxMana, float baseHealthRegen = 5, float baseManaRegen = 5) {
        //  this.maxMana = currentMana = maxMana;
        //statManager.IncreaseHealthRegeneration( baseHealthRegen);
        //statManager.IncreaseManaRegeneration(baseManaRegen);
        //currentHealth = this.maxHealth = maxHealth;
        statManager.Init(maxHealth, maxMana, baseHealthRegen, baseManaRegen);
    }

    //protected void Update() {
    //    RegenerateMana();
    //    RegenerateHealth();
    //}

    

}
