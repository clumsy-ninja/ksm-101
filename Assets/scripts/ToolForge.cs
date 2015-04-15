using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ToolType
{
    Ingot,
    Key
}

public class ToolForge : UseContext
{
    public ToolType output;
    public Color usageColor;

    private SpriteRenderer _sprite;
    private Color _originalColor;
    private float _coolDown;
    private bool _isCoolingDown;

    public override bool CanUse(PlayerController player)
    {
        return player.tool != output;
    }

    protected override bool ApplyUsage(PlayerController player)
    {
        player.tool = output;

        _originalColor = _sprite.color;
        _sprite.color = usageColor;
        _coolDown = 0;
        _isCoolingDown = true;
        return true;
    }

    void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(_isCoolingDown)
        {
            _coolDown += 0.05f*Time.deltaTime;
            _sprite.color = Color.Lerp(_sprite.color, _originalColor, _coolDown);
            if(_coolDown >= 1f)
                _isCoolingDown = false;
        }
    }
}
