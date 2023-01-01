using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class is used for Events that have one int argument.
/// </summary>

public class IntEventChannelSO : ScriptableObject
{
	public UnityAction<int> OnEventRaised;

    public void RaiseEvent(int value) => OnEventRaised?.Invoke(value);
}