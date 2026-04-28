using Shared.DataTransferObjects;
using System;
using UnityEngine;

public class BattleEvents
{
    public static event Action TurnStarted;

    public static event Action<DamageContext> HeroWasAttacked;
    public static event Action<DamageContext> MonsterWasAttacked;
    
    public static event Action BattleStarted;
    public static event Action PlayerDied;
    public static event Action MonsterDied;

    public static event Action<long> PlayerPickedMove;
    public static event Action<long> MonsterPickedMove;
    
    public static void InvokeHeroWasAttacked(DamageContext context) => HeroWasAttacked?.Invoke(context);
    public static void InvokeMonsterWasAttacked(DamageContext context) => MonsterWasAttacked?.Invoke(context);
    
    public static void InvokeBattleStarted() => BattleStarted?.Invoke();
    public static void InvokeTurnStarted() => TurnStarted?.Invoke();

    public static void InvokePlayerDied() => PlayerDied?.Invoke();
    public static void InvokeMonsterDied() => MonsterDied?.Invoke();

    public static void InvokePlayerPickedMove(long moveId) => PlayerPickedMove?.Invoke(moveId);
    public static void InvokeMonsterPickedMove(long moveId) => MonsterPickedMove?.Invoke(moveId);
}
