using Assets.HeroEditor4D.Common.Scripts.CharacterScripts;
using Assets.HeroEditor4D.Common.Scripts.Enums;
using Assets.HeroEditor4D.InventorySystem.Scripts.Data;
using Assets.HeroEditor4D.InventorySystem.Scripts.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : GameCharacter {

    //toggle all logic
    private bool isActive = true;
    private const float _MAX_HEALTH_ = 1000;
    private const float _MAX_MANA_ = 1000;

    #region Vars - movement
    private const float MOVEMENT_TOLERANCE = 0.01f;

    //movement speed multiplier for buffing
    [HideInInspector] public float movementSpeedMultiplier = 1;
    //position for the player to move to when user clicks
    private Vector3 moveToPosition;
    private const float CLICK_RADIUS = .25f;

    #endregion
    #region Vars - controls
    //control settings - placeholder before moving to a more robust settings far in the future
    public static int FORCE_MOVEMENT_BUTTON;// = 2; //what is middle click
    public static KeyCode KEY_CODE_DODGE;// = KeyCode.Space;
    public static KeyCode KEY_CODE_ABILITY_0;// = KeyCode.Q;
    public static KeyCode KEY_CODE_ABILITY_1;// = KeyCode.W;
    public static KeyCode KEY_CODE_ABILITY_2;// = KeyCode.E;
    public static KeyCode KEY_CODE_ABILITY_3;// = KeyCode.R;
    public static KeyCode KEY_CODE_CHAR_PANEL;// = KeyCode.C;

    public const int KEY_CODE_ABILITY_4 = 0; //left click
    public const int KEY_CODE_ABILITY_5 = 1; //right click

    public int DISPLAY_FLOATING_COMBAT_TEXT;

    #endregion
    #region Vars - Helper Objects
    public SpellBar spellBar;

    //reference to interface script
    [HideInInspector] public UIBehaviour HUD;
    [HideInInspector] public Collider2D playerCollider;

    //[SerializeField] private Ability[] abilities;

    //public PlayerResourceManager resourceManager;


    //reference to animation manager script to control animations
    // public AnimationManager animationManager;


    #endregion
    #region Vars - combat stats
    //tracked as a float from 0 - 1 as a % cooldown reduction, .2 = 20% reduced cooldown
    [HideInInspector] public float cooldownReduction = 0;


    #endregion
    #region Vars - character states
    //control player logic states
    public enum State {
        walking,
        idle,
        dodging
    }
    private State state;
    #endregion
    #region Vars - dodge
    //dodge control vars
    [SerializeField] private float DODGE_TIME;
    private float dodgeTimer = 0f;
    [SerializeField] private float dodgeSpeed;
    #endregion


    #region Start + Update
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();

        baseMovementSpeed = 3f;

        InitControls();


        Camera.main.transform.SetParent(transform, false);

        playerCollider = GetComponent<Collider2D>();
        StatManager.Init(_MAX_HEALTH_, _MAX_MANA_, 5, 5);
        //placeholder
        //animationManager.SetWeaponType(WeaponType.Melee2H);


        spellBar.EquipAbility(4, 405);   //iceshot
        spellBar.EquipAbility(5, 403);   //barage
        spellBar.EquipAbility(0, 800); //teleport
        spellBar.EquipAbility(1, 401); //piercing shot
        spellBar.EquipAbility(2, 100); //flamestrike
        spellBar.EquipAbility(3, 200); //haste





    }


    // Update is called once per frame
    protected override void Update() {

        if (isActive && isAlive) {

            if (state != State.dodging) {


                base.Update();
                HandleAnimationLockedInput();
                if (!animationManager.IsAction) //animation lock
                 {

                    HandleInput();
                    //if (state != State.dodging) {
                    //    HandleMovement();
                    //}
                }
            }

        }

    }
    private void FixedUpdate() {
        animationManager.animationSpeed = StatManager.GetActionSpeed();
        if (!animationManager.IsAction && state != State.dodging) {
            HandleMovement();
        }
        else if (state == State.dodging) {
            HandleDodge();
        }
    }
    #endregion
    #region Dodge
    //begins the dodge animation
    void StartDodge() {
        //grab movement direction in the direction of the mouse
        movementDirection = GameController.CameraToWorldPointMousePos() - transform.position;

        movementDirection.z = 0;
        movementDirection.Normalize();

        //set state
        state = State.dodging;

    }
    //proces the dodge animation 
    void HandleDodge() {
        //dodge ended
        if (dodgeTimer >= (DODGE_TIME/* / StatManager.GetMovementSpeed()*/)) {
            dodgeTimer = 0f;
            state = State.idle;
            return;
        }
        //dodge not correct direction
        dodgeTimer += Time.deltaTime;
        rb.MovePosition(
            Vector3.MoveTowards(
                transform.position, 
                movementDirection * DODGE_TIME, 
                dodgeSpeed * Time.fixedDeltaTime));


    }
    #endregion
    #region Animation
    protected override void UpdateAnimation() {
        //change character animation state
        if (state == State.walking) {
            animationManager.SetState(CharacterState.Walk);
        }
        else if (state == State.dodging) {
            animationManager.SetState(CharacterState.Evasion);
        }
        else if (state == State.idle) {
            animationManager.SetState(CharacterState.Idle);
        }
        base.UpdateAnimation();
    }



    public void FaceDirection(Vector3 direction) {
        movementDirection = (direction - transform.position).normalized;
    }

    #endregion
    #region Player Input
    private void InitControls() {
        PlayerSettingsHelper.InitObjectSettings(this, "Player");
    }


    //handle player walking to a location specified by mouse click
    void HandleMovement() {
        if (ReachedDestination()) {
            state = State.idle;
            return;
        }
        //move towards the moveToPosition vector
        if (state == State.walking) {
            //transform.position = Vector3.MoveTowards(
            //    transform.position,
            //    moveToPosition,
            //    GetMovementSpeed() * Time.deltaTime);

            rb.MovePosition(Vector3.MoveTowards(
                transform.position,
                moveToPosition,
                GetMovementSpeed() * Time.fixedDeltaTime));

            movementDirection = (moveToPosition - transform.position).normalized;
        }
    }
    //handle all input from the player
    void HandleInput() {
        if (state == State.dodging) return;
        //force player movement not curently working
        if (Input.GetMouseButton(FORCE_MOVEMENT_BUTTON)) {
            moveToPosition = GameController.CameraToWorldPointMousePos();
            state = State.walking;
            moveToPosition.z = transform.position.z;
        }

        if (Input.GetKey(KEY_CODE_ABILITY_0)) {
            if (spellBar.spellWrappers[0] != null) {
                spellBar.Cast(0);

            }
        }
        if (Input.GetKey(KEY_CODE_ABILITY_1)) {
            if (spellBar.spellWrappers[1] != null) {
                spellBar.Cast(1);
            }
        }
        if (Input.GetKey(KEY_CODE_ABILITY_2)) {
            if (spellBar.spellWrappers[2] != null) {
                spellBar.Cast(2);
            }
        }
        if (Input.GetKey(KEY_CODE_ABILITY_3)) {
            if (spellBar.spellWrappers[3]) {
                spellBar.Cast(3);
            }
        }

        //this slot is locked to left click

        if (Input.GetMouseButton(0)) {
            //false to be replaced with check for clicking on enemy
            if (ClickedOnEnemy() || Input.GetKey(KeyCode.LeftShift)) { //clicked on enemy or player is holding shift so cast spell
                if (spellBar.spellWrappers[4] != null) {

                    spellBar.Cast(4);
                }
            }
            else { //otherwise move
                moveToPosition = GameController.CameraToWorldPointMousePos();
                state = State.walking;
                moveToPosition.z = transform.position.z;
            }

        }
        //mouse right 
        if (Input.GetMouseButton(KEY_CODE_ABILITY_5)) {
            if (spellBar.spellWrappers[5] != null) {
                // Debug.Log("spell 5 casting");
                spellBar.Cast(5);
            }
        }


    }
    private bool ClickedOnEnemy() {
        var hit = Physics2D.CircleCast(GameController.CameraToWorldPointMousePos(), CLICK_RADIUS, Vector3.zero);
        return hit && hit.collider.gameObject.layer == GameController.ENEMY_LAYER;

    }
    //handle any input that is available to the player during animation lock
    //menu stuff/pause/play/dodge
    private void HandleAnimationLockedInput() {
        if (Input.GetKeyDown(KEY_CODE_DODGE)) {
            StartDodge();
        }
    }

    #endregion
    #region Add/Removing Buffs/Auras

    //public override void ApplyBuff(Buff buff) {
    //    //Debug.Log("Applying Buff: " +  buff.ToString());
    //    HUD.DisplayNewBuff(buff);
    //    base.ApplyBuff(buff);   
    //}
    //public override bool RemoveBuff(Buff buff) {

    //    HUD.ForceRemoveBuff(buff);
    //    return base.RemoveBuff(buff);
    //}
    #endregion
    #region Getters/Setters/ToString

    public override string ToString() {
        return "Health: " + StatManager.GetCurrentHealth() + "/" + StatManager.GetMaxHealth() + "\n" +
            "Mana:" + StatManager.GetCurrentMana() + "/" + StatManager.GetMaxMana() + "\n" +
            "HP Regen: " + StatManager.GetTotalHealthRegen() + "\n" +
            "Mana Regen: " + StatManager.GetTotalManaRegen() + "\n" +
            "Movespeed:" + GetMovementSpeed() + "\n" +
            "AttackSpeed:" + StatManager.GetAttackSpeed() * 100 + "%\n" +
            "Cooldown Reduction: " + (cooldownReduction * 100) + "%\n" +
            "Action Speed: " + StatManager.GetActionSpeed() * 100 + "%";
    }
    protected override void StopMove() {
        moveToPosition = transform.position;
    }

    //public float GetMovementSpeed() {
    //    return baseMovementSpeed * movementSpeedMultiplier * StatManager.GetActionSpeed();
    //}
    public float GetCooldownReduction() {
        return cooldownReduction;
    }

    //returns true if the distance squared to the given position is less than the movement tolerance constant
    //used to end player movement and avoid floating point errors
    bool ReachedDestination() {
        return GetDistanceSquared2D(transform.position, moveToPosition) < MOVEMENT_TOLERANCE;
    }

    public void EquipItem(Item item) {
        character4DScript.Equip(item);
    }


    #endregion

    public override void RemoveOnDeath() {
        Debug.Log("Player death");
    }

}
