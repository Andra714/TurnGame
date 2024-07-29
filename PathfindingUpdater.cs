using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    private void Start() 
    {
        DestructbleCrate.OnAnyDestoryed += DestructbleCrate_OnAnyDestoryed;  
    }

    private void DestructbleCrate_OnAnyDestoryed(object sender, EventArgs e)
    {
        DestructbleCrate destructbleCrate = sender as DestructbleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructbleCrate.GetGridPosition(), true);
    }
}
