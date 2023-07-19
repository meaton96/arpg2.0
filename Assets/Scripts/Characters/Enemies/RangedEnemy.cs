using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : Enemy {

    public override void Init(float health, Player player, string type) {
        base.Init(health, player, type);
        availableAbilities.Add(GameController.Instance.allSpells[400]);

    }
    protected override void AttackPlayer() {
        base.AttackPlayer();

    }
}
