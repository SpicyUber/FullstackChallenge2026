using Shared.DataTransferObjects;
using Shared.Enumerators;
using UnityEngine;

public class DamageContext
{
    public string SenderName { get; set; }

    public string MoveName { get; private set; }
    public int DamageBeforeDefenseApplied { get; private set; }

    public DamageType Type { get; private set; }
    public EffectDto Effect { get; private set; }

    public DamageContext(string senderName, string moveName, int damageBeforeDefenseApplied, DamageType type, EffectDto effect)
    {
        SenderName = senderName;
        MoveName = moveName;
        DamageBeforeDefenseApplied = damageBeforeDefenseApplied;
        Type = type;
        Effect = effect;
    }
}
