using System.Collections;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SupportEnemy : RangedEnemy {
    [SerializeField] protected float auraRangeSquared = 36;     //the radius squared of the range of the aura
    private const float pollNearbyEnemiesCooldown = 1f;         //how often to poll nearby enemies in seconds
    private float pollTimer;
    [SerializeField] protected GameObject auraVisualPrefab;     //The visual for the aura while it is on
    [SerializeField] Buff auraBuffToApply;                      //the buff that the aura applies
    [SerializeField] protected RangedAuraDetector AuraCollider; //aura collider

    /// <summary>
    /// Initalize fields and visuals
    /// </summary>
    /// <param name="player">pointer to the player</param>
    /// <param name="allEnemies">list of all current enemies</param>
    /// <param name="health">max health</param>
    /// <param name="mana">max mana</param>
    /// <param name="isActive">enable or disable movement logic</param>
    public override void Init(Player player, List<Enemy> allEnemies,
        float health, float mana = 0, bool isActive = true) {
        base.Init(player, allEnemies, health, mana, isActive);
        Instantiate(auraVisualPrefab, transform);   //create the aura visual and attach it
        //set the aura buff
        AuraCollider.Buff = AbilityCollectionSingleton.Instance.GetBuffCopy(auraBuffToApply);
        //set the aura collider radius
        AuraCollider.GetComponent<CircleCollider2D>().radius = Mathf.Sqrt(auraRangeSquared) / transform.localScale.x;
    }

    //do nothing instead of attacking
    protected override void AttackPlayer() { }

    protected void FixedUpdate() {
        //die if not alive!!
        if (!resourceManager.IsAlive()) {
            animationManager.Die();
        }
        else {
            if (isActive) {
                //poll nearby enemies by getting all units within the aura range
                if (pollTimer > pollNearbyEnemiesCooldown) {
                    var nearbyEnemies = GetNearbyEnemies();
                    //set target to null if there are nearby enemies
                    //this is used to stop the Seek behaviour
                    if (nearbyEnemies.Count > 0) {
                        target = null;
                    }
                    //if there are no nearby enemies then set target transform so the support will move toward the player
                    else {
                        target = player.transform;
                    }
                    pollTimer = 0;
                }
                else {
                    pollTimer += Time.fixedDeltaTime;
                }
                //apply seek if there is a target (player)
                if (target != null) {
                    ApplySeek();
                }
                //always apply flock
                ApplyFlock();

                if (rb.velocity.sqrMagnitude > 0) {
                    animationManager.SetState(CharacterState.Walk);
                }
                movementDirection = rb.velocity.normalized;
                UpdateFunctionWrapper();
            }
        }
    }
    List<Enemy> GetNearbyEnemies() {
        if (allAgents.Count == 0) { return new List<Enemy>(); }
        List<Enemy> result = new();
        result = allAgents.FindAll(
            enemy =>
            GetDistanceSquared2D(enemy.transform.position, transform.position) <= auraRangeSquared);
        return result;
    }
}
