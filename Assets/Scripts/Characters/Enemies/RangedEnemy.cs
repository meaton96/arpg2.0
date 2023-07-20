using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {

    public override void Init(float health, Player player, string type) {
        base.Init(health, player, type);
        availableAbilities.Add(GameController.Instance.allSpells[400]);

    }
    protected override void AttackPlayer() {
        ProjectileAbility ability = availableAbilities[Random.Range(0, availableAbilities.Count)] as ProjectileAbility;
        var proj = ability.Cast(transform.position, target.position, Vector3.zero, enemyCollider);

        //workaround
        //passing collider into spell wasnt working like it does for player for some reason
        proj.layer = LayerMask.NameToLayer("EnemyProjectiles");
        base.AttackPlayer();

    }
}
