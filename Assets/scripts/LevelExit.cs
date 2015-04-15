using UnityEngine;
using System.Collections;

public class LevelExit : UseContext
{
    public Color unlockColor = Color.green;

    public override bool CanUse(PlayerController player)
    {
        return player.tool == ToolType.Key;
    }

    protected override bool ApplyUsage(PlayerController player)
    {
        gameObject.GetComponent<SpriteRenderer>().color = unlockColor;
        return true;
    }
}
