using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections;
using UnityEngine;

public class Monster : BaseCharacter
{
    [SerializeField]
    private float _delayBeforeAttackingInSeconds = 1f;

    public override int Attack => Mathf.Max(0, CharacterInfo.Attack + GetEffectValue(EffectType.MODIFY_ATTACK));

    public override int Defense => Mathf.Max(0, CharacterInfo.Defense + GetEffectValue(EffectType.MODIFY_DEFENSE));

    public override int Magic => Mathf.Max(0, CharacterInfo.Magic + GetEffectValue(EffectType.MODIFY_MAGIC));

    public override int Health => CharacterInfo.Health;

    public override int Mana => CharacterInfo.Mana;

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
        if(!IsMyTurn || _healthComponent.CurrentValue==0) yield break;

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
