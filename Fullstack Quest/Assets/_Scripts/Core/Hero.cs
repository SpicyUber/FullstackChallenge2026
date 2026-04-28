using Shared.DataTransferObjects;
using Shared.Enumerators;
using System.Linq;
using UnityEngine;

public class Hero : BaseCharacter
{
    public const int HeroItemSlots = 4;

    public ItemDto[] Items { get; private set; } = new ItemDto[HeroItemSlots];

    public override int Attack => Mathf.Max(0,CharacterInfo.Attack
        + (StatsUtils.AttackLevelScaling * GameManager.Instance.HeroAttackLevel)
        + GetEffectValue(EffectType.MODIFY_ATTACK) + Items.Sum(dto => dto?.AttackDelta ?? 0));

    public override int Defense => Mathf.Max(0, CharacterInfo.Defense
        + (StatsUtils.DefenseLevelScaling * GameManager.Instance.HeroDefenseLevel)
        + GetEffectValue(EffectType.MODIFY_DEFENSE) + Items.Sum(dto => dto?.DefenseDelta ?? 0));

    public override int Magic => Mathf.Max(0, CharacterInfo.Magic
        + (StatsUtils.MagicLevelScaling * GameManager.Instance.HeroMagicLevel)
        + GetEffectValue(EffectType.MODIFY_MAGIC) + Items.Sum(dto => dto?.MagicDelta ?? 0));

    public override int Health => CharacterInfo.Health
        + (StatsUtils.HealthLevelScaling * GameManager.Instance.HeroHealthLevel)
        + Items.Sum(dto => dto?.HealthDelta ?? 0);

    public override int Mana => CharacterInfo.Mana
        + (StatsUtils.ManaLevelScaling * GameManager.Instance.HeroManaLevel)
        + Items.Sum(dto => dto?.ManaDelta ?? 0);

    protected override bool IsMyTurn => GameManager.Instance.BattleTurnState == TurnState.HERO;

    protected override void InitializeSpecific(CharacterDto characterDto)
    {
        _spriteRenderer.flipX = true;
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
