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

        if (InAttackRange()) {
            if (!InAttackCooldown() && !animationManager.IsAction) {
                AttackPlayer();
            }
            else {
                //shuffle around or something a bit 
                
            }

        }
        else {
            MoveTowardsPlayer();
        }
        if (rb.velocity.magnitude > 0) {
            animationManager.SetState(CharacterState.Walk);
        }

    }
    protected virtual void AttackPlayer() {
        attackTimer = attackCooldown;
        animationManager.Attack();
    }
    protected void MoveTowardsPlayer() {
        var vToPlayer = player.transform.position - transform.position;
        rb.velocity = vToPlayer.normalized * movementSpeed;
    }

    protected bool InAttackRange() {
        return GetDistanceSquared2D(player.transform.position, transform.position) <= attackRange;
    }
    protected bool InAttackCooldown() {
        if (attackTimer <= 0) return true;
        attackTimer -= Time.deltaTime;
        return false;
    }



}
