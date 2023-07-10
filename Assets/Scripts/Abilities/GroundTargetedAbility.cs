using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundTargetedAbility : DamagingAbility {

    public float maxRange;

    public override string ToString() {
        return
            base.ToString() +
            "Max Range: " + maxRange + "\n";

    }

}
