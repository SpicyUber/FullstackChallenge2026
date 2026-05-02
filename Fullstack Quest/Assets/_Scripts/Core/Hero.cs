using Shared.DataTransferObjects;
using Shared.Enumerators;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hero : BaseCharacter
{
    public const int HeroItemSlots = 4;

    public int AttackLevel { get; set; }
    public int DefenseLevel { get; set; }

    public int MagicLevel { get; set; }

    public int HealthLevel { get; set; }
    public int ManaLevel { get; set; }

    public List<MoveDto> LearnedMoves { get; set; } = new();
    public List<ItemDto> OwnedItems { get; set; } = new();

    public ItemDto[] Items { get; private set; } = new ItemDto[HeroItemSlots];

    public override int Attack => (int)(Mathf.Max(0, CharacterInfo.Attack
        + (StatsUtils.AttackLevelScaling * GameManager.Instance.HeroAttackLevel)
        + Items.Sum(dto => dto?.AttackDelta ?? 0))
        * (1 + (GetEffectValue(EffectType.MODIFY_ATTACK) / 100f)));

    public override int Defense => (int)(Mathf.Max(0, CharacterInfo.Defense
        + (StatsUtils.DefenseLevelScaling * GameManager.Instance.HeroDefenseLevel)
        + Items.Sum(dto => dto?.DefenseDelta ?? 0))
        * (1 + (GetEffectValue(EffectType.MODIFY_DEFENSE) / 100f)));

    public override int Magic => (int)(Mathf.Max(0, CharacterInfo.Magic
        + (StatsUtils.MagicLevelScaling * GameManager.Instance.HeroMagicLevel)
        + Items.Sum(dto => dto?.MagicDelta ?? 0))
        * (1 + (GetEffectValue(EffectType.MODIFY_MAGIC) / 100f)));

    public override int Health => Mathf.Max(_healthComponent.AllowedMinForUse, CharacterInfo.Health
        + (StatsUtils.HealthLevelScaling * GameManager.Instance.HeroHealthLevel)
        + Items.Sum(dto => dto?.HealthDelta ?? 0));

    public override int Mana => Mathf.Max(_manaComponent.AllowedMinForUse, CharacterInfo.Mana
        + (StatsUtils.ManaLevelScaling * GameManager.Instance.HeroManaLevel)
        + Items.Sum(dto => dto?.ManaDelta ?? 0));

    public override bool IsMyTurn => GameManager.Instance.BattleTurnState == TurnState.HERO;

    protected override bool _flipVFXSprite => false;

    protected override void InitializeSpecific(CharacterDto characterDto)
    {
        _spriteRenderer.flipX = true;

        LearnedMoves.Clear();
        OwnedItems.Clear();
        Items = new ItemDto[HeroItemSlots];

        foreach(var move in Moves)
            LearnedMoves.Add(move);

    }

    protected override void OnDeath() => BattleEvents.InvokePlayerDied();

    protected override void OnDisableSpecific()
    {
        GameEvents.HeroSelected -= Initialize;
        GameEvents.HeroBoughtUpgrade -= ResetEffectsAndResources;
        BattleEvents.HeroWasAttacked -= TakeDamageAndApplyEffects;
        BattleEvents.PlayerPickedMove -= CastMoveWithId;
    }

    protected override void OnEnableSpecific()
    {
        GameEvents.HeroSelected += Initialize;
        GameEvents.HeroBoughtUpgrade += ResetEffectsAndResources;
        BattleEvents.HeroWasAttacked += TakeDamageAndApplyEffects;
        BattleEvents.PlayerPickedMove += CastMoveWithId;
    }

}
