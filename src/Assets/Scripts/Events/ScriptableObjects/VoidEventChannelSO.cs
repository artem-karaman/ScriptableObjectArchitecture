using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have no arguments (Example: Exit game event)
/// </summary>

[CreateAssetMenu(menuName = "SOArchitecture/Events/VoidEventChannelSO")]
public class VoidEventChannelSO : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaisedEvent() => OnEventRaised?.Invoke();
}
