using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class EventManager : MonoBehaviour
{
    public Transform events;
    public ARSession session;

    void HandleEventsState()
    {
#if UNITY_EDITOR
        events.gameObject.SetActive(true);
#else
        if (session.subsystem != null)
        {
            events.gameObject.SetActive(session.subsystem.running);
        }
#endif
    }

    private void Update()
    {
        HandleEventsState();
    }
}
