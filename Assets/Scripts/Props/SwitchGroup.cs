using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGroup : MonoBehaviour
{
    [SerializeField] List<Switch> switches = new List<Switch>();

    public void CheckStates()
    {
        foreach (Switch unSwitch in switches)
        {
            if (unSwitch.GetState() == false)
            {
                return;
            }
        }
        Clear();
    }

    private void Clear()
    {
        foreach (Switch unSwitch in switches)
        {
            unSwitch.Unactivate();
        }
    }
}
