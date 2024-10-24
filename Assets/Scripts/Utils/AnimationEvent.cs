
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvent : MonoBehaviour
{
    public List<AnimEvent> events = new List<AnimEvent>();

    public void TriggerEvent(string eventName)
    {
        if (events.Exists(x => x.name == eventName))
        {
            var eventData = events.Find(x => x.name == eventName);
            eventData.action.Invoke();
        }
    }
}

[System.Serializable]
public class AnimEvent
{
    public string name;
    public UnityEvent action;
}
