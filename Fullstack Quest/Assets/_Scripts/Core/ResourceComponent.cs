using System;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int CurrentValue { get; private set; }
    public int MaxValue { get; private set; }

    public int RegenValue { get => _regenValue; set { _regenValue = Mathf.Max(0, value); } }
    public int AllowedMinForUsing { get => _allowedMinForUsing; set { _allowedMinForUsing = Mathf.Max(0, value); } }

    public event Action ResourceEmpty;

    private int _allowedMinForUsing;
    private int _regenValue;

    public void Add(int value)
    {
        CurrentValue = Mathf.Min(CurrentValue + value, MaxValue);
    }

    /// <summary>
    /// Returns true and uses resource.
    /// Debuffs are returned as negative values.
    /// </summary>
    public bool UseResource(int value)
    {
        if(value - CurrentValue <= _allowedMinForUsing) return false;

        CurrentValue = Mathf.Max(CurrentValue - value, 0);

        return true;
    }

    public void SetMaxResource(int value, bool setResourceToMax = false)
    {
        MaxValue = value;
        if(setResourceToMax)
            CurrentValue = MaxValue;
        else
            CurrentValue = Mathf.Min(CurrentValue, MaxValue);
    }

    public void TakeResource(int value)
    {
        CurrentValue = Mathf.Max(CurrentValue - value, 0);
        if(CurrentValue == 0) ResourceEmpty?.Invoke();
    }

    public void ResetResource() => CurrentValue = MaxValue;
}
