using Shared.DataTransferObjects;
using Shared.Enumerators;
using System.Linq;
using UnityEngine;

public class Hero : BaseCharacter
{
    public const int HeroItemSlots = 4;

    public ItemDto[] Items { get; private set; } = new ItemDto[HeroItemSlots];

    public override int Attack => CharacterInfo.Attack
        + (StatsUtils.AttackLevelScaling * GameManager.Instance.HeroAttackLevel)
        + GetEffectValue(EffectType.MODIFY_ATTACK) + Items.Sum(dto => dto?.AttackDelta ?? 0);

    public override int Defense => CharacterInfo.Defense
        + (StatsUtils.DefenseLevelScaling * GameManager.Instance.HeroDefenseLevel)
        + GetEffectValue(EffectType.MODIFY_DEFENSE) + Items.Sum(dto => dto?.DefenseDelta ?? 0);

    public override int Magic => CharacterInfo.Magic
        + (StatsUtils.MagicLevelScaling * GameManager.Instance.HeroMagicLevel)
        + GetEffectValue(EffectType.MODIFY_MAGIC) + Items.Sum(dto => dto?.MagicDelta ?? 0);

    public override int Health => CharacterInfo.Health
        + (StatsUtils.HealthLevelScaling * GameManager.Instance.HeroHealthLevel)
        + Items.Sum(dto => dto?.HealthDelta ?? 0);

    public override int Mana => CharacterInfo.Mana
        + (StatsUtils.ManaLevelScaling * GameManager.Instance.HeroManaLevel)
        + Items.Sum(dto => dto?.ManaDelta ?? 0);

    protected override void InitializeSpecific(CharacterDto characterDto)
    {
        _spriteRenderer.flipX = true;

        CastMove(Moves[0]);
    }

    protected override void OnDeath() => BattleEvents.InvokePlayerDied();

    protected override void OnDisableSpecific()
    {
        GameEvents.HeroSelected -= Initialize;
        GameEvents.HeroBoughtUpgrade -= ResetEffectsAndResources;
        BattleEvents.HeroAttacked += TakeDamageAndApplyEffects;
    }

    protected override void OnEnableSpecific()
    {
        GameEvents.HeroSelected += Initialize;
        GameEvents.HeroBoughtUpgrade += ResetEffectsAndResources;
        BattleEvents.HeroAttacked -= TakeDamageAndApplyEffects;
    }

}
