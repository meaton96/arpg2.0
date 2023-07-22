using System.Collections;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SupportEnemy : RangedEnemy
{
    [SerializeField] protected float auraRangeSquared = 36;
    private const float pollNearbyEnemiesCooldown = .5f;
    private float pollTimer;
    [SerializeField] protected GameObject auraVisualPrefab;

    HashSet<Enemy> nearbyEnemies;

    public override void Init(float health, Player player, string type) {
        base.Init(health, player, type);
        availableAbilities.Add(GameController.Instance.allSpells[spellID]);
        Instantiate(auraVisualPrefab, transform);
    }

    protected override void AttackPlayer() {
        
    }
    protected override void Update() {
        if (!resourceManager.IsAlive()) {
            animationManager.Die();
        }
        else {
            if (pollTimer > pollNearbyEnemiesCooldown) {
                UpdateNearbyEnemies();
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
        
        if (nearbyEnemies.Count == 0) {
            target = player.transform;
        }
        else {
            if (target == null) {
                //target died or never existed
                //  target = nearbyEnemies.g
                target = nearbyEnemies.ElementAt(Random.Range(0, nearbyEnemies.Count)).transform;
            }


        }
    }
    void UpdateNearbyEnemies() {
        HashSet<Enemy> currentEnemySet = nearbyEnemies;
        nearbyEnemies.Clear();
        GameController.Instance.enemyList.ForEach(enemy => {
            if (GetDistanceSquared2D(enemy.transform.position, transform.position) < attackRange) {
                nearbyEnemies.Add(enemy);
            }
        });
        var enemiesMovedOutOfRangeOrDied = currentEnemySet.Except(nearbyEnemies);
        //need to remove the effect on these 
        //if they had died they might be destroyed here which will cause an error
        //not sure how to fix
        

    }
    
}
