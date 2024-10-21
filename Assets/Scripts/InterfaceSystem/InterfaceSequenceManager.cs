using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceSequenceManager : MonoBehaviour
{
    public List<GameObject> interfaces = new List<GameObject>();
    private int currentIndex = -1;
    private GameObject currentInterface;

    void HandleDestroyCurrentInterface()
    {
        if (currentInterface != null)
        {
            Destroy(currentInterface);
        }
    }

    void HandleSwitchToInterface(int index)
    {
        var choosenInterface = interfaces[index];
        currentInterface = Instantiate(choosenInterface, transform);
    }

    public void GoNextInterface()
    {
        HandleDestroyCurrentInterface();
        currentIndex = Mathf.Clamp(currentIndex + 1, 0, interfaces.Count - 1);
        HandleSwitchToInterface(currentIndex);
    }
    public void GoPreviousInterface()
    {
        HandleDestroyCurrentInterface();
        currentIndex = Mathf.Clamp(currentIndex - 1, 0, interfaces.Count - 1);
        HandleSwitchToInterface(currentIndex);
    }

    private void Awake()
    {
        GoNextInterface();
    }
}
