using Codice.CM.Common;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageToast : MonoBehaviour {
    [SerializeField] private float movementSpeed = .4f;
    public const float TOAST_DURATION = 1f;
    public Vector3 TOAST_MOVEMENT_DIR = new(.577f, 1.1547f, 0);
    public readonly List<Vector3> popUpOffsets = new() {
        new Vector3 (.75f,2.25f,0),
        new Vector3 (-.75f,2.25f,0),
        new Vector3 (0, 2.25f,0),
        new Vector3 (0,2.25f,0),
        new Vector3 (.75f,3f,0),
        new Vector3 (-.75f,3f,0),
        new Vector3 (.75f,1.5f,0),
        new Vector3 (-.75f,1.5f,0)
    };
    Vector3 initPos;
    Vector3 finalPos;
    float startTime;
    public static int offSetIndex = 0;
    // Start is called before the first frame update
    void Start() {
        //iterates through popUpOffsets array so that damage numbers are not drawn all on top of eachother 
        transform.position += popUpOffsets[offSetIndex++ % popUpOffsets.Count];
        if (offSetIndex > 10000) {
            offSetIndex = 0;
        }
        startTime = Time.time;
        initPos = transform.position;
        finalPos = initPos + (TOAST_DURATION * TOAST_MOVEMENT_DIR);
    }

    // Update is called once per frame
    void Update() {
        Destroy(gameObject, TOAST_DURATION);
        //transform.Translate(movementSpeed * TOAST_MOVEMENT_DIR);
        // Calculate the fraction of the journey completed
        float journeyLength = Vector3.Distance(initPos, finalPos);
        float distCovered = (Time.time - startTime) * movementSpeed;
        float fractionOfJourney = distCovered / journeyLength;

        // Set our position as a fraction of the distance between the markers.
        transform.position = Vector3.Lerp(initPos, finalPos, fractionOfJourney);
    }
    public void SetDamageAmount(float amount) {
        GetComponent<TextMeshPro>().text = amount.ToString();
    }
}
