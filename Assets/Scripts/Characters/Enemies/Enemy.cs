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

    protected const float FORCE_MULTIPLIER = 1.5f;
    protected const float SEPERATE_MULTIPLIER = 1;
    protected const float MIN_SEP_RANGE = 0.1f, MAX_SEP_RANGE = 5;


    public void Init(float movementSpeed, float attackCooldown, float health, Player player, string type) {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        resourceManager.Init(health, 0, 0, 0);
        this.player = player;
        this.movementSpeed = movementSpeed;
        this.attackCooldown = attackCooldown;

        //temporary
        if (type == "basicGoblin") {
            attackRange = 1;
        }
        else if (type == "shooterGoblin") {
            attackRange = 5;
        }

    }
    protected void Update() {
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
            rb.AddForce(Seek() * FORCE_MULTIPLIER);


            rb.AddForce(Seperate() * FORCE_MULTIPLIER);
        }
        if (rb.velocity.magnitude > 0) {
            animationManager.SetState(CharacterState.Walk);
        }

    }
    protected virtual void AttackPlayer() {
        attackTimer = attackCooldown;
        animationManager.Attack();
    }
    protected Vector2 Seek() {
        var desiredVelocity = (player.transform.position - transform.position).normalized * movementSpeed;

        var velocity_2d = new Vector2(desiredVelocity.x, desiredVelocity.y);

        return velocity_2d - rb.velocity;

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

                vToEnemy = vToEnemy / distance * SEPERATE_MULTIPLIER;
                force += new Vector2(vToEnemy.x, vToEnemy.y);
            }
        });

        return force;


    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position + new Vector3(rb.velocity.x, rb.velocity.y), Seek());
        Gizmos.DrawLine(transform.position, rb.velocity);

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
