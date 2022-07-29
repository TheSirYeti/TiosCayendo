using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventRunner : MonoBehaviour
{
    public string eventName;
    
    public void RunBoolEvent()
    {
        EventManager.Trigger(eventName, true);
    }
}
