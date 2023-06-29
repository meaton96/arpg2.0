using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileBehaviour : MonoBehaviour {
    public const int WALL_LAYER = 6;
    protected float speed;
    protected GameCharacter caster;
    [Range(0f, 1.0f)]
    public float animationSpeed = .14f;
    protected bool flagAnimated = false;
    protected Animator animator;
    protected int pierce = 0;
    protected int chain;
    protected float chainingRange = 4f;
    protected const int NUM_RAYCASTS = 8;
    protected GameObject prefab;
    protected ProjectileAbility ability;
    protected Rigidbody2D rb;
    protected int enemiesHit = 0;

    #region Initialize
    // Start is called before the first frame update
    void Awake() {

        if (TryGetComponent(out animator)) {
            flagAnimated = true; 
        }
        rb = GetComponent<Rigidbody2D>();

        if (flagAnimated) {
            animator.speed = animationSpeed;
        }
        
    }
   

    public virtual void Init(GameCharacter caster, ProjectileAbility ability, GameObject prefab, Vector3 direction, 
                        float speed, int pierce, int chain, int enemiesHit = 0) {

        this.prefab = prefab;
        //this.direction = direction;
        this.speed = speed;
        rb.velocity = direction * speed;
        this.caster = caster;
        this.pierce = pierce;
        this.chain = chain;
        this.ability = ability;
        this.enemiesHit = enemiesHit;

    }
    #endregion
    #region Collision
    protected void OnTriggerEnter2D(Collider2D other) {

        if (other.gameObject.layer == WALL_LAYER) {
            Destroy(gameObject);
        }
        else {
            

            if (chain > 0) {

                TryChain(other);
            }
            if (pierce == 0) {
                Destroy(gameObject);
            }
            else {
                enemiesHit++;
                pierce--;
            }

        }
    }
    #endregion
    #region Chaining
    //performs a projectile chain
    //performs a number of raycasts in a circle around the target hit by this projectile
    //attempts to create a projectile towards an enemy hit
    protected void TryChain(Collider2D collider) {
        var angle = Mathf.PI * 2 / NUM_RAYCASTS;
        int random = Random.Range(0, 2);
        var initialPosition = collider.transform.position;

        //either 0 or 1 to change chaining seek logic
        if (random > 0) {
            for (int x = 0; x < NUM_RAYCASTS; x++) {
                if (CalculateChain(collider, x * angle, initialPosition)) {
                    return;
                }
            }
        }
        else {
            for (int x = NUM_RAYCASTS; x > 0; x--) {
                if (CalculateChain(collider, x * angle, initialPosition)) {
                    return;
                }
            }
        }
    }
    //performs a raycast from the position of collider in the angle of i_angle
    //if the raycast hits an enemy a new projectile will be created and shot at that enemy
    //the projectile created is the same as this one
    protected bool CalculateChain(Collider2D collider, float i_angle, Vector3 initialPosition) {

        var dir = new Vector2(Mathf.Cos(i_angle), Mathf.Sin(i_angle)).normalized;   //grab the direction vector towards i_angle
        var colliderInRange = Physics2D.RaycastAll(
                                initialPosition,
                                dir,
                                chainingRange,
                                LayerMask.GetMask("Enemy"));    //perform the raycast and get all colliders in range
                                                                //checks until chainingRange and only hits targets on enemy layer
        //iterate all colliders in the raycast hit array
        foreach (var enemyCollider in colliderInRange) {
            if (enemyCollider.collider == collider) { continue; } //ignore hittings itself
            var vecToChainTarget = (enemyCollider.
                                            collider.
                                            gameObject.
                                            transform.
                                            position
                                            - initialPosition).
                                            normalized; //get the vector to the hit target, this is not the same as the racast direction 

            //create the new projectile
            var newProj = Instantiate(gameObject, 
                initialPosition + GameCharacter._CHARACTER_HALF_HEIGHT_, 
                Quaternion.Euler(
                    new Vector3(
                        0, 
                        0,
                        Mathf.Atan(vecToChainTarget.y / vecToChainTarget.x) * Mathf.Rad2Deg)));
            //initiate the projectile with the vars from this object, except reduce the number of chains
            

            CreateProjectile(newProj, collider, caster, ability, prefab, vecToChainTarget, speed, pierce, chain, enemiesHit);
            //ignore the collision with the target this projectile orginially hit
            Physics2D.IgnoreCollision(collider, newProj.GetComponent<Collider2D>());
            return true; //flag successful chain
        }
        return false;
    }

    protected virtual void CreateProjectile(GameObject newProj, Collider2D collider, GameCharacter caster, ProjectileAbility ability, GameObject prefab, Vector3 direction,
                        float speed, int pierce, int chain, int enemiesHit) {
        newProj.GetComponent<ProjectileBehaviour>().
                Init(
                    ability: ability,
                    prefab: prefab,
                    direction: direction,
                    speed: speed,
                    caster: caster,
                    pierce: pierce,
                    chain: chain - 1,
                    enemiesHit: enemiesHit + 1
                    );
        //ignore the collision with the target this projectile orginially hit
        Physics2D.IgnoreCollision(collider, newProj.GetComponent<Collider2D>());
    }
    #endregion
    public virtual float CalculateDmage() {
        
        return ability.CalculateDamage();
    }



}
