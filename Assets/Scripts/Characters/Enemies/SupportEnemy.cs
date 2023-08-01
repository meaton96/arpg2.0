using System.Collections;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SupportEnemy : RangedEnemy {
    private const float IDLE_RANGE = .2f;
    [SerializeField] protected float auraRangeSquared = 36;     //the radius squared of the range of the aura
    private const float pollNearbyEnemiesCooldown = 1f;         //how often to poll nearby enemies in seconds
    private float pollTimer;
    [SerializeField] protected GameObject auraVisualPrefab;     //The visual for the aura while it is on
    [SerializeField] Buff auraBuffToApply;                      //the buff that the aura applies
    [SerializeField] protected RangedAuraDetector AuraCollider; //aura collider
    GameObject auraVisual;
    Vector2 seekTarget;
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
        auraVisual = Instantiate(auraVisualPrefab, transform);   //create the aura visual and attach it
        //set the aura buff
        var buffCopy = AbilityCollectionSingleton.Instance.GetBuffCopy(auraBuffToApply);
        AuraCollider.Buff = buffCopy;
        auraBuffToApply = buffCopy;
        //set the aura collider radius
        AuraCollider.GetComponent<CircleCollider2D>().radius = Mathf.Sqrt(auraRangeSquared) / transform.localScale.x;
    }

    //do nothing instead of attacking
    protected override void AttackPlayer() { }

    protected override void Update() {
        if (!StatManager.IsAlive()) {
            animationManager.Die();
        }
        if (isActive) {
            base.Update();
            //poll nearby enemies by getting all units within the aura range
            if (pollTimer > pollNearbyEnemiesCooldown) {
                var nearbyEnemies = GetNearbyEnemies();
                //set target to null if there are nearby enemies
                //this is used to stop the Seek behaviour
                if (nearbyEnemies.Count > 0) {
                    Vector2 pos = new();
                    foreach (var nearby in nearbyEnemies) {
                        pos += (Vector2)nearby.transform.position;
                    }
                    pos /= nearbyEnemies.Count;
                    seekTarget = pos;
                }
                //if there are no nearby enemies then set target transform so the support will move toward the player
                else {
                    seekTarget = player.transform.position;
                }
                pollTimer = 0;
            }
            else {
                pollTimer += Time.deltaTime;
            }
        }
    }

    protected void FixedUpdate() {
        //die if not alive!!
        if (isActive && isAlive) {
            if (seekTarget == null) {
                seekTarget = player.transform.position;

            }
            if (GetDistanceSquared2D(seekTarget, transform.position) > IDLE_RANGE) {
                ApplySeek(seekTarget);
            }
            
            if (rb.velocity.sqrMagnitude > 0) {
                animationManager.SetState(CharacterState.Walk);
            }
            movementDirection = rb.velocity.normalized;
            //UpdateFunctionWrapper();
        }

    }
    protected override void ProcessDeath() {
        auraVisual.SetActive(false);
        var nearbyEnemies = GetNearbyEnemies();
        nearbyEnemies.ForEach(enemy => {
            enemy.BuffManager.RemoveBuff(auraBuffToApply);
        });
        base.ProcessDeath();

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
