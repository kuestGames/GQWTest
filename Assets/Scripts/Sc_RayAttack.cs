using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_RayAttack : MonoBehaviour{

    public bool active;
    public Transform rayOrigin;
    public Transform rayTarget;
    public LineRenderer lineRenderer;
    // Start is called before the first frame update
    private void Start() {
        lineRenderer.SetPosition(0, rayOrigin.position);
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (active) {
            //lineRenderer.SetPosition(0, rayOrigin.localPosition);
            lineRenderer.SetPosition(1, rayTarget.position);
        }
    }





}
