using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections;
using UnityEngine;

public class Monster : BaseCharacter
{
    [SerializeField]
    private float _delayBeforeAttackingInSeconds = 1f;

    public override int Attack => (int)(Mathf.Max(0, CharacterInfo.Attack) * (1 + (GetEffectValue(EffectType.MODIFY_ATTACK) / 100f)) * GameManager.Instance.LoopStatMultiplier);

    public override int Defense => (int)(Mathf.Max(0, CharacterInfo.Defense) * (1 + (GetEffectValue(EffectType.MODIFY_DEFENSE) / 100f)) * GameManager.Instance.LoopStatMultiplier);

    public override int Magic => (int)(Mathf.Max(0, CharacterInfo.Magic) * (1 + (GetEffectValue(EffectType.MODIFY_MAGIC) / 100f)) * GameManager.Instance.LoopStatMultiplier);

    public override int Health => CharacterInfo.Health * GameManager.Instance.LoopStatMultiplier;

    public override int Mana => CharacterInfo.Mana * GameManager.Instance.LoopStatMultiplier;

    public override bool IsMyTurn => GameManager.Instance.BattleTurnState == TurnState.MONSTER;

    protected override bool _flipVFXSprite => true;

    protected override void InitializeSpecific(CharacterDto characterDto)
    {
        _spriteRenderer.flipX = false;
    }

    protected override void OnDeath() => BattleEvents.InvokeMonsterDied();

    protected override void OnDisableSpecific()
    {
        GameEvents.EncounterMonsterSelected -= Initialize;
        BattleEvents.MonsterWasAttacked -= TakeDamageAndApplyEffects;
        BattleEvents.TurnStarted -= GetAndPlayRecommendedMoveFromServer;
    }

    private void GetAndPlayRecommendedMoveFromServer()
    {
        var battleState =
            new CharacterBattleStateDto(
                CharacterInfo.Id,
                _healthComponent.CurrentValue,
                _manaComponent.CurrentValue,
                Health,
                Mana);

        StartCoroutine(GetAndPlayRecommendedMoveFromServerCoroutine(battleState));
    }

    private IEnumerator GetAndPlayRecommendedMoveFromServerCoroutine(CharacterBattleStateDto battleState)
    {

        yield return new WaitForSeconds(_delayBeforeAttackingInSeconds);
        if(!IsMyTurn || _healthComponent.CurrentValue == 0) yield break;

        yield return GameManager.Instance.GetRecommendedMoveCoroutine(battleState);
        CastMoveWithId(GameManager.Instance.RecommendedCharacterMoveId);
    }

    protected override void OnEnableSpecific()
    {
        GameEvents.EncounterMonsterSelected += Initialize;
        BattleEvents.MonsterWasAttacked += TakeDamageAndApplyEffects;
        BattleEvents.TurnStarted += GetAndPlayRecommendedMoveFromServer;
    }
}
