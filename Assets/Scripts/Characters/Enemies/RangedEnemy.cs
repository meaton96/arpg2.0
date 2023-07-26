using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {

    [SerializeField] protected int spellID;
    public override void Init(Player player, List<Enemy> allEnemies, float health, float mana = 0, bool isActive = true) {
        base.Init(player, allEnemies,  health, mana, isActive);
        availableAbilities.Add(GameController.Instance.allSpells[spellID]);

    }
    protected override void AttackPlayer() {
        ProjectileAbility ability = availableAbilities[Random.Range(0, availableAbilities.Count)] as ProjectileAbility;
        var proj = ability.Cast(transform.position, target.position, Vector3.zero, enemyCollider);
        if (proj != null || proj.Count != 0) {
            //workaround
            //passing collider into spell wasnt working like it does for player for some reason
            for (int x = 0; x < proj.Count; x++) {
                proj[x].layer = LayerMask.NameToLayer("EnemyProjectiles");
            }
        }
        base.AttackPlayer();

    }
}
