using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy {

    public float basicAttackDamage;
    public override void Init(float health, Player player, string type) {
        base.Init(health, player, type);
        availableAbilities.Add(GameController.Instance.allSpells[400]);
        basicAttackDamage = availableAbilities[0].CalculateDamage(player);
    }
    protected override void AttackPlayer() {
        attackTimer = attackCooldown;
        
        PlayCastAnimation();

    }
}
