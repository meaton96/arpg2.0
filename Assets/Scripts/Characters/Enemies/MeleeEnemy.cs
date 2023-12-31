using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy {

    public float basicAttackDamage;
    public float attackTime;
    public bool twoHanded = false;
    //public override void Init(Player player, List<Enemy> allEnemies, float health, float mana = 0, bool isActive = true) {
    //    base.Init(player, allEnemies, health, mana, isActive);
    //    //availableAbilities.Add(GameController.Instance.allSpells[400]);
    //    //basicAttackDamage = availableAbilities[0].CalculateDamage(player);
    //    //character4DScript.WeaponType = Assets.HeroEditor4D.Common.Scripts.Enums.WeaponType.Melee1H;
    //    //attackCooldown = .25f * animationManager.animationSpeed;
        
    //}
    protected override void Start() {
        base.Start();
        animationManager.SetTwoHanded(twoHanded);
    }
    //protected override void AttackPlayer() {
        
    //    base.AttackPlayer();

    //}
    public override void DealMeleeDamage() {
        //attack will always hit as long as the player is still in range at the time of the sword hit
        //aprox 0.18 seconds after starting attack animation
      //  if (GetDistanceSquared2D(player.transform.position, transform.position) <= attackRange) {
            player.DamageHealth(Random.Range(DAMAGE_MIN, DAMAGE_MAX));
     //   }
    }
}
