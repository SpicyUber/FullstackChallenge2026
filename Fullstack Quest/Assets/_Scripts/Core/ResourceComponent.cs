using System;
using UnityEngine;

public class ResourceComponent : MonoBehaviour
{
    public int CurrentValue { get; private set; }
    public int MaxValue { get; private set; }

    public int RegenValue { get => _regenValue; set { _regenValue = Mathf.Max(0, value); } }
    public int AllowedMinForUsing { get => _allowedMinForUsing; set { _allowedMinForUsing = Mathf.Max(0, value); } }

    public event Action ResourceEmpty;
    public event Action ResourceChanged;

    private int _allowedMinForUsing;
    private int _regenValue;

    public void Add(int value)
    {
        int originalValue = CurrentValue;
        CurrentValue = Mathf.Min(CurrentValue + value, MaxValue);

        InvokeEventIfResourceChanged(originalValue);
    }

    public void InvokeEventIfResourceChanged(int oldValue)
    {
        if(oldValue != CurrentValue)
            ResourceChanged?.Invoke();
    }

    /// <summary>
    /// Returns true and uses resource.
    /// Debuffs are returned as negative values.
    /// </summary>
    public bool UseResource(int value)
    {
        int originalValue = CurrentValue;

        if(value - CurrentValue <= _allowedMinForUsing) return false;
        CurrentValue = Mathf.Max(CurrentValue - value, 0);

        InvokeEventIfResourceChanged(originalValue);
        return true;
    }

    public void SetMaxResource(int value, bool setResourceToMax = false)
    {
        int originalValue = CurrentValue;
        MaxValue = value;

        if(setResourceToMax)
        {
            CurrentValue = MaxValue;
        }
        else
        {
            CurrentValue = Mathf.Min(CurrentValue, MaxValue);
        }

        InvokeEventIfResourceChanged(originalValue);
    }

    public void TakeResource(int value)
    {
        int originalValue = CurrentValue;

        CurrentValue = Mathf.Max(CurrentValue - value, 0);
        if(CurrentValue == 0) ResourceEmpty?.Invoke();

        InvokeEventIfResourceChanged(originalValue);
    }

    public void ResetResource() => CurrentValue = MaxValue;
}
