using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
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
    protected Rigidbody2D rb;
    protected Collider2D enemyCollider;

    public Transform target;
   // public Transform player;
    public float flockRadius = 3f;
    public float separationWeight = 1f;
    public float alignmentWeight = 1f;
    public float cohesionWeight = 1f;

    protected const float FORCE_MULTIPLIER = 3f;
    protected const float SEPERATE_MULTIPLIER = 1;
    protected const float SEEK_MULTIPLIER = 1;
    protected const float MIN_SEP_RANGE = 0.1f, MAX_SEP_RANGE = 5;
    protected const float MAXIMUM_SPEED = 2f;

    protected List<Ability> availableAbilities;

    public virtual void Init(float health, Player player, string type) {
        _CHARACTER_HALF_HEIGHT_ *= transform.localScale.x;  
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        resourceManager.Init(health, 0, 0, 0);
        this.player = player;
        target = player.transform;
        availableAbilities = new();
        enemyCollider = GetComponent<Collider2D>();

    }
    protected override void Update() {
        //Debug.Log(GetDistanceSquared2D(transform.position, player.transform.position));
        if (!resourceManager.IsAlive()) {
            Destroy(gameObject);
        }

        if (InAttackRange()) {
            if (/*!InAttackCooldown() && */!animationManager.IsAction) {
                AttackPlayer();
            }
            else {
                //shuffle around or something a bit 

            }

        }
        else {
            Vector2 forces = new();
            forces += Seek(target.position) * SEEK_MULTIPLIER;
            forces += Flocking();
            //Debug.Log(forces);
            //forces += Seperate() * SEPERATE_MULTIPLIER; 
            rb.AddForce(forces * FORCE_MULTIPLIER);
        }
        if (rb.velocity.magnitude > 0) {
            animationManager.SetState(CharacterState.Walk);
        }
        movementDirection = rb.velocity.normalized;
       // if (rb.velocity.magnitude > MAXIMUM_SPEED)
       //     rb.velocity = movementDirection * MAXIMUM_SPEED;
        base.Update();

    }
    protected virtual void AttackPlayer() {
        attackTimer = attackCooldown;
        ProjectileAbility ability = availableAbilities[Random.Range(0, availableAbilities.Count)] as ProjectileAbility;
        ability.Cast(transform.position, target.position, Vector3.zero, GetComponent<Collider2D>());
        PlayCastAnimation();
    }
    protected override void StopMove() {
        rb.velocity = Vector2.zero;
    }
    private Vector2 Flocking() {
        List<Enemy> enemies = GameController.Instance.enemyList;
        Vector2 separationForce = Vector2.zero;
        Vector2 alignmentForce = Vector2.zero;
        Vector2 cohesionForce = Vector2.zero;

        int neighborCount = 0;

        foreach (var enemy in enemies) {
            if (enemy == this)
                continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < flockRadius) {
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
        Vector2 desiredVelocity = (targetPosition - (Vector2)transform.position).normalized * movementSpeed;
        return desiredVelocity - rb.velocity;
    }
    protected Vector2 Seperate() {
        var enemies = GameController.Instance.enemyList.FindAll(enemy => {
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


    protected bool InAttackRange() {
        return GetDistanceSquared2D(player.transform.position, transform.position) <= Mathf.Pow(attackRange, 2);
    }
    protected bool InAttackCooldown() {
        if (attackTimer <= 0) return true;
        attackTimer -= Time.deltaTime;
        return false;
    }



}
