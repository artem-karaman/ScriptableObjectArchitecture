using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A flexible handler for int events in the form of a MonoBehaviour.
/// Responses can be connected directly from the Unity Inspector.
/// </summary>
public class IntEventListener : MonoBehaviour
{
	[SerializeField] private IntEventChannelSO _channel;

	public IntEvent OnEventRaised;

    private void OnEnable()
    {
        if(_channel != null)
        {
            _channel.OnEventRaised += Respond;
        }
            
    }
    private void OnDisable()
    {
        if (_channel != null)
        {
            _channel.OnEventRaised -= Respond;
        }
    }

    private void Respond(int value) => OnEventRaised?.Invoke(value);
}

/// <summary>
/// To use a generic UnityEvent type need to override the generic type.
/// </summary>
public class IntEvent : UnityEvent<int>{}
