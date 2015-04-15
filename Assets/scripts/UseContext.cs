using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class UseContext: MonoBehaviour
{
    public abstract bool CanUse(PlayerController player);
    protected abstract bool ApplyUsage(PlayerController player);

    public bool Use(PlayerController player)
    {
        if(!CanUse(player))
            return false;

        return ApplyUsage(player);
    }
}