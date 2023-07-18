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

    protected const float FORCE_MULTIPLIER = 3f;
    protected const float SEPERATE_MULTIPLIER = 1;
    protected const float SEEK_MULTIPLIER = 1;
    protected const float MIN_SEP_RANGE = 0.1f, MAX_SEP_RANGE = 5;
    protected const float MAXIMUM_SPEED = 2f;


    public void Init(float movementSpeed, float attackCooldown, float health, Player player, string type) {
        _CHARACTER_HALF_HEIGHT_ = new(0, 0.35f, 0);
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
    protected override void Update() {
        Debug.Log(GetDistanceSquared2D(transform.position, player.transform.position));
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
            forces += Seek() * SEEK_MULTIPLIER;
            Debug.Log(forces);
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
        PlayCastAnimation();
    }
    protected override void StopMove() {
        rb.velocity = Vector2.zero;
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

                vToEnemy /= distance;
                force += new Vector2(vToEnemy.x, vToEnemy.y);
            }
        });

        return force;


    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position, Seek());
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
