using Shared.Enumerators;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StatsUtils
{
    public const int AttackLevelScaling = 10;
    public const int DefenseLevelScaling = 10;
    public const int MagicLevelScaling = 10;
    public const int HealthLevelScaling = 30;
    public const int ManaLevelScaling = 30;

    public const int DefaultManaRegen = 1;
    public const int DefaultMinHealth = 1;

    public static IReadOnlyList<EffectType> StatBuffs { get; } =
    new List<EffectType>
    {
        EffectType.MODIFY_ATTACK,
        EffectType.MODIFY_MAGIC,
        EffectType.MODIFY_DEFENSE
    };

    public static IReadOnlyList<EffectType> HealthBuffs { get; } =
    new List<EffectType>
    {
        EffectType.POISON,
        EffectType.BLEED
    };
}
