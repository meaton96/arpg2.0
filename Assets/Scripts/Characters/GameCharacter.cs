using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour {
    #region Vars - Projectile Spawning
    [HideInInspector] public const float _PROJECTILE_SPAWN_RADIUS_ = 1f;
    [HideInInspector] public static readonly Vector3 _CHARACTER_HALF_HEIGHT_ = new(0, 0.7f, 0);
    #endregion

    [HideInInspector] public ResourceManager resourceManager;

    protected AnimationManager animationManager;

    //reference to the character script to change character direction
    public Character4D character4DScript;

    protected virtual void Start() {
        character4DScript = GetComponent<Character4D>();
        character4DScript.SetDirection(Vector2.right);
        animationManager = character4DScript.AnimationManager;
        resourceManager = GetComponent<ResourceManager>();
    }

    public void DamageHealth(float amount) {
       // Debug.Log(resourceManager.currentHealth + "/" + resourceManager.maxHealth);
        resourceManager.DamageHealth(amount);

    }

    //replace with calculation from weapon damage
    public virtual float GetAttackDamage() {
        return 5;
    }
}
