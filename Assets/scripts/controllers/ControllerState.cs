using System;
using System.Collections;
using UnityEngine;

public class ControllerState
{
    public RaycastHit2D LeftHit { get; set; }
    public RaycastHit2D RightHit { get; set; }
    public RaycastHit2D AboveHit { get; set; }
    public RaycastHit2D BelowHit { get; set; }

    public bool IsCollidingLeft { get; set; }
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingBelow { get; set; }

    public float JumpCooldown { get; set; }
}