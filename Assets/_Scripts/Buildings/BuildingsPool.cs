using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsPool : MonoBehaviour
{
    [SerializeField] private List<Building> _buildings = new List<Building>();

    public Building[] GetBuildings()
    {
        ValidateIDs();
        Building[] buildings = new Building[_buildings.Count];
        Array.Copy(_buildings.ToArray(), buildings, _buildings.Count);
        return buildings;
    }
    

    private void ValidateIDs()
    {
        for (int i = 0; i < _buildings.Count-1; i++)
        {
            for (int j = i+1; j < _buildings.Count; j++)
            {
                if (_buildings[i] == _buildings[j])
                    continue;
                if (_buildings[i].ID == _buildings[j].ID)
                    _buildings[j].ChangeID(_buildings[j].ID + 1);
            }
        }
    }
}
