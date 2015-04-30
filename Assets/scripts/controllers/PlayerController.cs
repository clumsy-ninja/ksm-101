using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public static readonly float Epsilon = 0.001f;

    public ToolType tool = ToolType.Ingot;
    public ControllerSettings settings;

    public ControllerState state {get; private set;}

    private class Inputs
    {
        public float run;
        public bool jump;

        public void Reset()
        {
            run = 0;
            jump = false;
        }

        public override string ToString()
        {
            return string.Format("run:{0}, jump:{1}", run, jump);
        }
    }

    private Rigidbody2D _rigidBody;
    private BoxCollider2D _boxCollider;
    private Inputs _inputs;
    private UseContext _useContext;

    void Awake()
    {
        state = new ControllerState();
        _inputs = new Inputs();

        _rigidBody = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UseContext usable = other.gameObject.GetComponent<UseContext>();
        if(usable != null)
            _useContext = usable;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        UseContext usable = other.gameObject.GetComponent<UseContext>();
        if(usable != null)
            _useContext = null;
    }

    void Update()
    {
        if(Input.GetAxis("Use") > 0 )
        {
            if(_useContext)
                _useContext.Use(this);
            else
            {
                //TODO: use the current tool
            }

            return;
        }

        _inputs.run = Input.GetAxis("Horizontal");
        _inputs.jump = Input.GetAxis("Jump") > 0;
    }

    void LateUpdate()
    {
        UpdateControllerState();

        if(_inputs.jump && CanJump())
        {
            _rigidBody.AddForce(new Vector2(0, settings.jumpScale));
            state.JumpCooldown = settings.jumpWaitTime;
        }

        if(Math.Abs(_inputs.run) > Epsilon && 
            ((_inputs.run < 0 && !state.IsCollidingLeft) ||
            (_inputs.run > 0 && !state.IsCollidingRight)))
        {
            float scale = state.IsCollidingBelow ?
                settings.maxSpeed :
                settings.maxSpeedAir;

            _rigidBody.velocity = new Vector2(_inputs.run * scale,
                _rigidBody.velocity.y);
        }

        _inputs.Reset();
    }

    private bool CanJump()
    {
        return state.IsCollidingBelow &&
            state.JumpCooldown <= 0 &&
            Mathf.Abs(_rigidBody.velocity.y) < Epsilon;
    }

    private void UpdateControllerState()
    {
        float distance;
        Vector2 center = _boxCollider.bounds.center;
        Vector2 size = _boxCollider.bounds.size;
        size.x -= settings.skinWidth;
        size.y -= settings.skinWidth;

        if(state.JumpCooldown > 0)
        {
            state.JumpCooldown -= Time.deltaTime;
            if(state.JumpCooldown < 0)
                state.JumpCooldown = 0;
        }

        state.LeftHit = Physics2D.BoxCast(center, size,
            0f, -Vector2.right, settings.ground);
        if(state.LeftHit.collider != null)
        {
            distance = (center - state.LeftHit.centroid).sqrMagnitude;
            state.IsCollidingLeft = distance < settings.skinWidth;
        }
        else
            state.IsCollidingLeft = false;

        state.RightHit = Physics2D.BoxCast(center, size, 
            0f, Vector2.right, settings.ground);
        if(state.RightHit.collider != null)
        {
            distance = (center - state.RightHit.centroid).sqrMagnitude;
            state.IsCollidingRight = distance < settings.skinWidth;
        }
        else
            state.IsCollidingRight = false;

        state.AboveHit = Physics2D.BoxCast(center, size,
            0f, Vector2.up, settings.ground);
        if(state.AboveHit.collider != null)
        {
            distance = (center - state.AboveHit.centroid).sqrMagnitude;
            state.IsCollidingAbove = distance < settings.skinWidth;
        }
        else
            state.IsCollidingAbove = false;

        bool previous = state.IsCollidingBelow;
        state.BelowHit = Physics2D.BoxCast(center, size, 
            0f, -Vector2.up, settings.ground);
        if(state.BelowHit.collider != null)
        {
            distance = (center - state.BelowHit.centroid).sqrMagnitude;
            state.IsCollidingBelow = distance < settings.skinWidth;
        }
        else
            state.IsCollidingBelow = false;
        if(!previous && state.IsCollidingBelow)
            state.JumpCooldown = settings.jumpWaitTime;
    }

    void OnDrawGizmosSelected()
    {
        if(state == null)
            return;

        Gizmos.color = state.IsCollidingLeft ? Color.red : Color.cyan;
        Gizmos.DrawSphere(state.LeftHit.point, 0.1f);

        Gizmos.color = state.IsCollidingRight ? Color.red : Color.cyan;
        Gizmos.DrawSphere(state.RightHit.point, 0.1f);

        Gizmos.color = state.IsCollidingAbove ? Color.red : Color.cyan;
        Gizmos.DrawSphere(state.AboveHit.point, 0.1f);

        Gizmos.color = state.IsCollidingBelow ? Color.red : Color.cyan;
        Gizmos.DrawSphere(state.BelowHit.point, 0.1f);
    }
}