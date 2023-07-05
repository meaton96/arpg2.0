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
    private const float _MAX_HEALTH_ = 100;
    private const float _MAX_MANA_ = 100;

    #region Vars - movement
    private const float MOVEMENT_TOLERANCE = 0.01f;
    [SerializeField] private float movementSpeed;
    //movement speed multiplier for buffing
    [HideInInspector] public float movementSpeedMultiplier = 1;
    //position for the player to move to when user clicks
    private Vector3 moveToPosition;
    //direction the character is currently moving
    private Vector3 movementDirection;
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

    #endregion
    #region Vars - Helper Objects
    public SpellBar spellBar;

    //reference to interface script
    [HideInInspector] public UIBehaviour HUD;
    

    //public PlayerResourceManager resourceManager;

    
    //reference to animation manager script to control animations
   // public AnimationManager animationManager;

    
    #endregion
    #region Vars - combat stats
    //tracked as a float from 0 - 1 as a % cooldown reduction, .2 = 20% reduced cooldown
    [HideInInspector] public float cooldownReduction = 0;
    [HideInInspector] public float actionSpeed = 1;
    //NYI
    [HideInInspector] public float castSpeed;
    [HideInInspector] public float attackSpeed;
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
    #region Vars - Buff/Debuff tracking
    //track current debuffs and buffs and timers
    private List<Buff> currentBuffsDebuffs;
    private List<float> currentBuffsDebuffsTimers;
    #endregion
    
    #region Start + Update
    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
        InitControls();
        currentBuffsDebuffs = new();
        currentBuffsDebuffsTimers = new();
        GameController.Instance.player = this;
        Camera.main.transform.SetParent(transform, false);
        HUD = GameObject.FindWithTag("HUD").GetComponent<UIBehaviour>();
        
        resourceManager.Init(_MAX_HEALTH_, _MAX_MANA_);
        //placeholder
        //animationManager.SetWeaponType(WeaponType.Melee2H);


        spellBar.EquipAbility(4, 400);   //arrow shot
        spellBar.EquipAbility(5, 1);   //ice lance
        spellBar.EquipAbility(0, 800); //teleport
        spellBar.EquipAbility(1, 401); //piercing shot
        spellBar.EquipAura(0, GameController.Instance.allSpells[900] as Aura);
        spellBar.EquipAura(1, GameController.Instance.allSpells[901] as Aura);

    }
   

    // Update is called once per frame
    void Update() {

        if (isActive) {

            if (state == State.dodging) {
                HandleDodge();
            }
            else {
                UpdateAnimation();
                if (!animationManager.IsAction) //animation lock
                 {
                    HandleAnimationLockedInput();
                    HandleInput();
                    if (state != State.dodging) {
                        HandleMovement();
                    }
                }
            }
        }

    }
    private void FixedUpdate() {    
        animationManager.animationSpeed = actionSpeed;
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
        if (dodgeTimer >= DODGE_TIME) {
            dodgeTimer = 0f;
            state = State.idle;
            return;
        }
        dodgeTimer += Time.deltaTime;
        transform.position += Time.deltaTime * dodgeSpeed * movementDirection;


    }
    #endregion
    #region Animation
    void UpdateAnimation() {
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
        //change character direction
        if (Mathf.Abs(movementDirection.x) > Mathf.Abs(movementDirection.y)) {
            character4DScript.SetDirection(movementDirection.x < 0 ? Vector2.left : Vector2.right);
        }
        else
            character4DScript.SetDirection(movementDirection.y > 0 ? Vector2.up : Vector2.down);

    }
    public void PlayAttackAnimation() {
        switch (character4DScript.WeaponType) {
            case WeaponType.Melee1H:
                animationManager.Slash(twoHanded: false);
                break;
            case WeaponType.Melee2H:
            case WeaponType.Paired:
                animationManager.Slash(twoHanded: true);
                break;
            case WeaponType.Bow:
                animationManager.ShotBow();
                break;
            case WeaponType.Crossbow:
                animationManager.CrossbowShot();
                break;
        }
    }
    public void PlayCastAnimation() {
        StopMove();
        PlayAttackAnimation();
    }

    public void FaceDirection(Vector3 direction) {
        movementDirection = (direction - transform.position).normalized;
    }

    #endregion
    #region Player Input
    private void InitControls() {
        PlayerSettingsHelper.InitPlayerControls(this);
    }


    //handle player walking to a location specified by mouse click
    void HandleMovement() {
        if (ReachedDestination()) {
            state = State.idle;
            return;
        }
        //move towards the moveToPosition vector
        if (state == State.walking) {
            transform.position = Vector3.MoveTowards(
                transform.position,
                moveToPosition,
                GetMovementSpeed() * Time.deltaTime);

            movementDirection = (moveToPosition - transform.position).normalized;
        }
    }
    //handle all input from the player
    void HandleInput() {
        //force player movement not curently working
        if (Input.GetMouseButton(FORCE_MOVEMENT_BUTTON)) {
            moveToPosition = GameController.CameraToWorldPointMousePos();
            state = State.walking;
            moveToPosition.z = transform.position.z;
        }

        if (Input.GetKeyDown(KEY_CODE_ABILITY_0)) {
            if (spellBar.spellWrappers[0] != null) {
                spellBar.Cast(0);

            }
        }
        if (Input.GetKeyDown(KEY_CODE_ABILITY_1)) {
            if (spellBar.spellWrappers[1] != null) {
                spellBar.Cast(1);
            }
        }
        if (Input.GetKeyDown(KEY_CODE_ABILITY_2)) {
            if (spellBar.spellWrappers[2] != null) {
                spellBar.Cast(2);
            }
        }
        if (Input.GetKeyDown(KEY_CODE_ABILITY_3)) {
            if (spellBar.spellWrappers[3]) {
                spellBar.Cast(3);
            }
        }

        //this slot is locked to left click

        if (Input.GetMouseButton(0)) {
            //false to be replaced with check for clicking on enemy
            if (false || Input.GetKey(KeyCode.LeftShift)) { //clicked on enemy or player is holding shift so cast spell
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
        if (Input.GetMouseButtonDown(KEY_CODE_ABILITY_5)) {
            if (spellBar.spellWrappers[5] != null) {
                // Debug.Log("spell 5 casting");
                spellBar.Cast(5);
            }
        }
        

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
    
    public void ApplyBuff(Buff buff) {
        //Debug.Log("Applying Buff: " +  buff.ToString());
        HUD.DisplayNewBuff(buff);
    }
    public void RemoveBuff(Buff buff) {
        currentBuffsDebuffs.Remove(buff);
        HUD.ForceRemoveBuff(buff);
    }
    #endregion
    #region Getters/Setters/ToString
    
    public override string ToString() {
        return "Health: " + resourceManager.currentHealth + "/" + resourceManager.maxHealth + "\n" +
            "Mana:" + resourceManager.currentMana + "/" + resourceManager.maxMana + "\n" +
            "HP Regen: " + resourceManager.GetTotalHealthRegen() + "\n" +
            "Mana Regen: " + resourceManager.GetTotalManaRegen() + "\n" +
            "Movespeed:" + GetMovementSpeed() + "\n" +
            "Cooldown Reduction: " + (cooldownReduction * 100) + "%" + "\n" +
            "Action Speed: " + actionSpeed * 100 + "%";
    }
    public void StopMove() {
        moveToPosition = transform.position;
    }

    public float GetMovementSpeed() {
        return movementSpeed * movementSpeedMultiplier * actionSpeed;
    }
    public float GetCooldownReduction() {
        return cooldownReduction;
    }
    float GetDistanceSquared2D(Vector3 v1, Vector3 v2) {
        return Mathf.Pow(v2.x - v1.x, 2) + Mathf.Pow(v2.y - v1.y, 2);
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
}
