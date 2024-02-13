using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelTrailRenderer : MonoBehaviour
{
    CarController carController;
    TrailRenderer trailRenderer;

    private void Awake()
    {
        carController = GetComponentInParent<CarController>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    void Update()
    {
        //Если дрифт или тормоза то оставляется след
        if (carController.IsTireScreeching(out float lateralVelocity, out bool isBreacking))
        {
            trailRenderer.emitting = true;
        }
        else trailRenderer.emitting = false;
    }
}
