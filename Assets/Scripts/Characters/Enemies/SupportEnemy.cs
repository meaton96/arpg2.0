using System.Collections;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SupportEnemy : RangedEnemy {
    [SerializeField] protected float auraRangeSquared = 36;
    private const float pollNearbyEnemiesCooldown = 1f;
    private float pollTimer;
    [SerializeField] protected GameObject auraVisualPrefab;
    [SerializeField] protected GameObject auraAttachPrefab;
    [SerializeField] Buff auraBuffToApply;
    //[SerializeField] private int buffId;
    [SerializeField] protected RangedAuraDetector AuraCollider;
    // HashSet<Enemy> nearbyEnemies;
    //[HideInInspector] new Vector3 target;

    public override void Init(Player player, List<Enemy> allEnemies, 
        float health, float mana = 0, bool isActive = true) {
        base.Init(player, allEnemies, health, mana, isActive);
        //availableAbilities.Add(GameController.Instance.allSpells[spellID]);
        //auraBuffToApply = (AbilityCollectionSingleton.Instance.GetAbilityByID(spellID) as Aura).buff;
        Instantiate(auraVisualPrefab, transform);
        AuraCollider.Buff = AbilityCollectionSingleton.Instance.GetBuffCopy(auraBuffToApply);
        AuraCollider.GetComponent<CircleCollider2D>().radius = Mathf.Sqrt(auraRangeSquared);
    }

    protected override void AttackPlayer() {

    }
    protected override void Update() {
        
        if (!resourceManager.IsAlive()) {
            animationManager.Die();
        }
        else {
            if (isActive) {
                if (pollTimer > pollNearbyEnemiesCooldown) {
                    var nearbyEnemies = GetNearbyEnemies();
                    if (nearbyEnemies.Count >0) {
                        target = null;
                    }
                    else {
                        target = player.transform;
                    }
                    //   GetTargetFromNearbyEnemies(nearbyEnemies);
                    //  ApplyAuraBuffToNearbyEnemies(nearbyEnemies);
                    pollTimer = 0;
                }
                else {
                    pollTimer += Time.deltaTime;
                }
                if (target != null) {
                    ApplySeek();
                }
                ApplyFlock();
                if (rb.velocity.magnitude > 0) {
                    animationManager.SetState(CharacterState.Walk);
                }
                movementDirection = rb.velocity.normalized;
                UpdateFunctionWrapper();
            }
        }


    }
    //void ApplyAuraBuffToNearbyEnemies(List<Enemy> nearbyEnemies) {
    //    foreach (Enemy enemy in nearbyEnemies) { 
    //        if (enemy.BuffManager.BuffAlreadyApplied(auraBuffToApply)) continue;
    //        var tempBuffWrap = Instantiate(auraAttachPrefab, enemy.transform).
    //            GetComponent<TempBuffWrapper_Distance>();
    //        tempBuffWrap.Attach(
    //            buff: auraBuffToApply,
    //            caster: this,
    //            attachedCharacter: enemy,
    //            range: auraRangeSquared,
    //            pollTimer: pollTimer
    //            );

    //    }
    //}
    List<Enemy> GetNearbyEnemies() {
        if (allAgents.Count == 0) { return new List<Enemy>(); }
        List<Enemy> result = new();
        result = allAgents.FindAll(
            enemy =>
            GetDistanceSquared2D(enemy.transform.position, transform.position) <= auraRangeSquared);
        return result;
    }
    ////void GetTargetFromNearbyEnemies(List<Enemy> nearbyEnemies) {

    //    if (nearbyEnemies.Count == 0) {
    //        target = player.transform;
    //    }
    //    else {
    //        Vector3 sum = new();
    //        foreach (Enemy e in nearbyEnemies) {
    //            sum += e.transform.position;
    //        }
    //        target = sum / nearbyEnemies.Count;
    //    }
    //}
    //void UpdateNearbyEnemies() {
    //    HashSet<Enemy> currentEnemySet = nearbyEnemies;
    //    nearbyEnemies.Clear();
    //    GameController.Instance.enemyList.ForEach(enemy => {
    //        if (GetDistanceSquared2D(enemy.transform.position, transform.position) < attackRange) {
    //            nearbyEnemies.Add(enemy);
    //        }
    //    });
    //    var enemiesMovedOutOfRangeOrDied = currentEnemySet.Except(nearbyEnemies);
    //    //need to remove the effect on these 
    //    //if they had died they might be destroyed here which will cause an error
    //    //not sure how to fix


    //}

}
