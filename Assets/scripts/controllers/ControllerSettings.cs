using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class ControllerSettings
{
    public float maxSpeed = 15.0f;
    public float maxSpeedAir = 5.0f;

    public float jumpScale = 1500.0f;
    public float jumpWaitTime = 0.25f;

    public float skinWidth = 0.06f;

    public LayerMask ground;
}