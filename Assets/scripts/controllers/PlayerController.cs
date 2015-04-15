using System;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    public static readonly float Epsilon = 0.001f;

    public float maxSpeed = 5.0f;

    public float jumpScale = 25.0f;
    public float jumpWaitTime = 0.25f;

    public float groundThreshold = 0.03f;
    public LayerMask groundLayer;
    public ToolType tool = ToolType.Ingot;

    private class Detector
    {
        public Vector2 min;
        public Vector2 max;
        public Collider2D hit;
    }

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
    private Detector _ground;
    private Inputs _inputs;
    private UseContext _useContext;

    void Awake()
    {
        _inputs = new Inputs();
        _ground = new Detector();

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

    void FixedUpdate()
    {
        Vector2 min = _boxCollider.bounds.min;
        Vector2 max = _boxCollider.bounds.max;

        _ground.min = new Vector2(min.x, min.y - groundThreshold);
        _ground.max = new Vector2(max.x, min.y);

        _ground.hit = Physics2D.OverlapArea(_ground.min,
            _ground.max, groundLayer);

        if(Math.Abs(_inputs.run) > Epsilon)
        {
            _rigidBody.velocity = new Vector2(_inputs.run * maxSpeed,
                _rigidBody.velocity.y);
        }

        if(_inputs.jump && canJump())
            _rigidBody.AddForce(new Vector2(0, jumpScale));

        _inputs.Reset();
    }

    private bool canJump()
    {
        return Math.Abs(_rigidBody.velocity.y) < Epsilon && 
            _ground.hit != null;
    }

    void OnDrawGizmosSelected()
    {
        if(_ground != null)
            DrawDetector(_ground);
    }

    private static readonly Color Active = new Color(1f, 1f, 0f, 0.2f);
    private static readonly Color Inactive = new Color(0.7f, 0.8f, 0.9f, 0.2f);
    private void DrawDetector(Detector bounds)
    {
        GL.Begin(GL.QUADS);
        GL.Color(bounds.hit ? Active : Inactive);
        GL.Vertex3(bounds.min.x, bounds.min.y, 0);
        GL.Vertex3(bounds.max.x, bounds.min.y, 0);
        GL.Vertex3(bounds.max.x, bounds.max.y, 0);
        GL.Vertex3(bounds.min.x, bounds.max.y, 0);
        GL.End();
    }
}