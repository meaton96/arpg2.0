using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityWrapper : MonoBehaviour {

    public Ability ability;
    public TextMeshProUGUI guiTimer;
    public enum AbilityState {
        cooldown,
        ready,
        active
    }
    private AbilityState abilityState;
    float cooldownDuration;
    float cooldownTimer;

    // Update is called once per frame
    void Update() {
        if (abilityState == AbilityState.cooldown) {
            if (cooldownTimer > 0) {

                cooldownTimer -= Time.deltaTime;
                guiTimer.text = cooldownTimer.ToString("0.#");
            }
            else {
                abilityState = AbilityState.ready;
            }
        }
        else {
            guiTimer.text = "";
        }
    }
    public bool Cast(Player player) {
        if (abilityState == AbilityState.ready) {
            if (player.resourceManager.TrySpendResource(ability.healthCost, ability.manaCost)) {
                var mousePos = GameController.CameraToWorldPointMousePos();
                var pos = player.transform.position;
                var vMouseToPlayer = (mousePos - pos).normalized * GameCharacter._PROJECTILE_SPAWN_RADIUS_;

                player.FaceDirection(mousePos);

                vMouseToPlayer += player._CHARACTER_HALF_HEIGHT_;

                ability.Cast(pos, mousePos, vMouseToPlayer, player.GetComponent<Collider2D>());

                cooldownDuration = ability.cooldown * (1 - player.cooldownReduction);
                cooldownTimer = cooldownDuration;
                abilityState = AbilityState.cooldown;
                return true;
            }

        }
        return false;
    }
    public void Init(Ability ability, TextMeshProUGUI guiTimer) {
        this.ability = ability;
        cooldownDuration = ability.cooldown;
        abilityState = AbilityState.ready;
        this.guiTimer = guiTimer;
    }
}
