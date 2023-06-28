using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public class Enemy : GameCharacter {
   // Character4D character4DScript;
    
    
    // Start is called before the first frame update
     protected override void Start() {

        base.Start();
        resourceManager.Init(100, 0, 0, 0);
    }

    
    
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.layer == GameController.PROJECTILE_LAYER)
            DamageHealth(other.GetComponent<ProjectileBehaviour>().CalculateDmage());
    }
}
