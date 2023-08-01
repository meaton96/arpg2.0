using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Codice.CM.SEIDInfo;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;

public abstract class Enemy : GameCharacter {
    // Character4D character4DScript;



    public float attackRange;
    public float attackCooldown;
    protected float attackTimer;
    protected Player player;

    protected Collider2D enemyCollider;

    public Transform target;
    // public Transform player;

    [HideInInspector] public float separationWeight = 1f;
    [HideInInspector] public float alignmentWeight = 1f;
    [HideInInspector] public float cohesionWeight = 1f;

    protected const float FORCE_MULTIPLIER = 500f;
    protected const float SEPERATE_MULTIPLIER = 1;
    protected const float SEEK_MULTIPLIER = 1;
    protected const float MIN_SEP_RANGE = 0.1f, MAX_SEP_RANGE = 5;
    protected const float MAXIMUM_SPEED = 2f;
    protected const float FLOCKING_RANGE = 16f; //distance squared
    protected bool isActive;

    [SerializeField] protected List<Ability> availableAbilities;
    protected List<Enemy> allAgents;

    [SerializeField] WeaponType weaponType;

    public virtual void Init(Player player, List<Enemy> allEnemies, float health, float mana = 0, bool isActive = true) {
        _CHARACTER_HALF_HEIGHT_ *= transform.localScale.x;
        allAgents = allEnemies;


        resourceManager.Init(health, mana, 0, mana == 0 ? 0 : 5);
        this.player = player;
        target = player.transform;
        availableAbilities = new();
        enemyCollider = GetComponent<Collider2D>();
        this.isActive = isActive;

        
    }
    public virtual void Init(int num, Player player, List<Enemy> allEnemies, float health, float mana = 0, bool isActive = true) {
        Init(player, allEnemies, health, mana, isActive);
        name += num;
    }
    protected override void Start() {
        base.Start();
        character4DScript.WeaponType = weaponType;
        animationManager.SetWeaponType(weaponType);
    }
    public virtual void DealMeleeDamage() { }
    private void FixedUpdate() {
        if (isAlive) {
            if (isActive) {
                if (InAttackRange()) {

                    if (attackTimer >= attackCooldown) {

                        AttackPlayer();
                        attackTimer = 0;
                    }
                    else {
                        attackTimer += Time.fixedDeltaTime;
                    }
                }
                else {
                    ApplySeekAndFlock();
                }

                if (rb.velocity.sqrMagnitude > 0) {
                    animationManager.SetState(CharacterState.Walk);
                }
                movementDirection = rb.velocity.normalized;
                // Debug.Log(rb.velocity.magnitude);
                if (rb.velocity.sqrMagnitude > Mathf.Pow(GetMovementSpeed(), 2))
                    rb.velocity = movementDirection * GetMovementSpeed();
            }


        }
    }
    public override void RemoveOnDeath() {
        Destroy(gameObject, 1);
    }
    protected virtual void AttackPlayer() {
        PlayCastAnimation();
    }
    protected override void StopMove() {
        rb.velocity = Vector2.zero;
    }
    protected void ApplySeekAndFlock() {
        ApplySeek();
        ApplyFlock();
    }
    protected void ApplySeek() {
        rb.AddForce(FORCE_MULTIPLIER * SEEK_MULTIPLIER * Seek(target.position));
    }
    private Vector2 Flocking() {

        Vector2 separationForce = Vector2.zero;
        Vector2 alignmentForce = Vector2.zero;
        Vector2 cohesionForce = Vector2.zero;

        int neighborCount = 0;

        foreach (var enemy in allAgents) {
            if (enemy == this) continue;
            if (enemy == null) continue;

            float distance = GetDistanceSquared2D(transform.position, enemy.transform.position);

            if (distance < FLOCKING_RANGE) {
                separationForce += (Vector2)transform.position - (Vector2)enemy.transform.position;
                alignmentForce += enemy.rb.velocity;
                cohesionForce += (Vector2)enemy.transform.position;
                neighborCount++;
            }
        }

        if (neighborCount > 0) {
            separationForce /= neighborCount;
            alignmentForce /= neighborCount;
            cohesionForce /= neighborCount;

            separationForce = separationForce.normalized * separationWeight;
            alignmentForce = alignmentForce.normalized * alignmentWeight;
            cohesionForce = (cohesionForce - (Vector2)transform.position).normalized * cohesionWeight;
        }

        return separationForce + alignmentForce + cohesionForce;
    }

    private Vector2 Seek(Vector2 targetPosition) {
        Vector2 desiredVelocity = (targetPosition - (Vector2)transform.position).normalized * baseMovementSpeed;
        return desiredVelocity - rb.velocity;
    }
    protected void ApplyFlock() {
        rb.AddForce(FORCE_MULTIPLIER * Flocking());
    }
    protected Vector2 Seperate() {
        var enemies = GameController.Instance.GetAllEnemies().FindAll(enemy => {
            return GetDistanceSquared2D(enemy.transform.position, transform.position) < MAX_SEP_RANGE;
        });

        Vector2 force = new();

        enemies.ForEach(enemy => {
            if (enemy != this) {
                var vToEnemy = enemy.transform.position - transform.position;
                var distance = GetDistanceSquared2D(vToEnemy);
                //var ratio = (distance) / (MAX_SEP_RANGE - MIN_SEP_RANGE);

                vToEnemy /= distance;
                force += new Vector2(vToEnemy.x, vToEnemy.y);
            }
        });

        return force;


    }


    private void OnDrawGizmos() {
        //Gizmos.DrawLine(transform.position, Seek(player.transform.position));
        //Gizmos.DrawLine(transform.position, rb.velocity);

    }
    protected override void ProcessDeath() {
        allAgents.Remove(this);
        base.ProcessDeath();

    }

    protected bool InAttackRange() {

        return GetDistanceSquared2D(player.transform.position, transform.position) <= attackRange;
    }
    protected bool InAttackCooldown() {
        if (attackTimer >= attackCooldown) return true;
        attackTimer += Time.deltaTime;
        return false;
    }



}
