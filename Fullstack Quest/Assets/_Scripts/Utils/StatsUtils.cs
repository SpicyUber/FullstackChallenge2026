using Shared.Enumerators;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class StatsUtils
{
    public const int AttackLevelScaling = 15;
    public const int DefenseLevelScaling = 15;
    public const int MagicLevelScaling = 15;
    public const int HealthLevelScaling = 50;
    public const int ManaLevelScaling = 50;

    public const int DefaultManaRegen = 5;
    public const int DefaultMinHealth = 1;

    public const int EnemyStatBuffMultiplierPerLevelLoop = 10;

    public static IReadOnlyList<EffectType> StatBuffs { get; } =
    new List<EffectType>
    {
        EffectType.MODIFY_ATTACK,
        EffectType.MODIFY_MAGIC,
        EffectType.MODIFY_DEFENSE
    };

    public static IReadOnlyList<EffectType> UseResourceHealthBuffs { get; } =
    new List<EffectType>
    {
        EffectType.POISON,
    };

    public static IReadOnlyList<EffectType> TakeResourceHealthBuffs { get; } =
    new List<EffectType>
    {
        EffectType.BLEED,
    };
}
