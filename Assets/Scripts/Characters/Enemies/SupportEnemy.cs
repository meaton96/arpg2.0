using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportEnemy : RangedEnemy
{
    [SerializeField] protected float auraRangeSquared = 36;
    private const float pollNearbyEnemiesCooldown = .5f;
    private float pollTimer;

    public override void Init(float health, Player player, string type) {
        base.Init(health, player, type);
        availableAbilities.Add(GameController.Instance.allSpells[spellID]);
        
    }
    protected override void AttackPlayer() {
        
    }
    protected override void Update() {
        if (!resourceManager.IsAlive()) {
            animationManager.Die();
        }
        else {
            if (pollTimer > pollNearbyEnemiesCooldown) {
                GetTargetFromNearbyEnemies();
                pollTimer = 0;
            }
            else {
                pollTimer += Time.deltaTime;
            }
            if (target != null) {
                ApplySeek();
            }
            if (rb.velocity.magnitude > 0) {
                animationManager.SetState(CharacterState.Walk);
            }
            movementDirection = rb.velocity.normalized;
            UpdateAnimation();
        }
        

    }
    void GetTargetFromNearbyEnemies() {
        var nearbyEnemies = GetNearbyEnemies();
        
        if (nearbyEnemies.Count == 0) {
            target = player.transform;
        }
        else {
            if (target == null) {
                //target died or never existed
                target = nearbyEnemies[Random.Range(0, nearbyEnemies.Count)].transform;

            }


        }
    }
    List<Enemy> GetNearbyEnemies() {
        List<Enemy> result = GameController.Instance.enemyList.
            FindAll(enemy => GetDistanceSquared2D(enemy.transform.position, transform.position) < attackRange);

        return result;

    }

}
