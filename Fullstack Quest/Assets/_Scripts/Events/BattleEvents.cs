using Shared.DataTransferObjects;
using System;
using UnityEngine;

public class BattleEvents
{
    public static event Action HeroTurnStarted;
    public static event Action MonsterTurnStarted;
    public static event Action<DamageContext> HeroAttacked;
    public static event Action<DamageContext> MonsterAttacked;
    public static event Action BattleStarted;
    public static event Action PlayerDied;
    public static event Action MonsterDied;

    public static void InvokeHeroTurnStarted() => HeroTurnStarted?.Invoke();
    public static void InvokeMonsterTurnStarted() => MonsterTurnStarted?.Invoke();
    public static void InvokeHeroAttacked(DamageContext context) => HeroAttacked?.Invoke(context);
    public static void InvokeMonsterAttacked(DamageContext context) => MonsterAttacked?.Invoke(context);
    public static void InvokeBattleStarted() => BattleStarted?.Invoke();
    public static void InvokePlayerDied() => PlayerDied?.Invoke();
    public static void InvokeMonsterDied() => MonsterDied?.Invoke();
}
