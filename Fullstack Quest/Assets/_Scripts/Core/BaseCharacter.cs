using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterDto CharacterInfo { get; protected set; }
    public const int MoveCount = 4;

    [SerializeField] protected SpriteRenderer _spriteRenderer;

    [SerializeField] protected ResourceComponent _healthComponent;
    [SerializeField] protected ResourceComponent _manaComponent;

    public ResourceComponent HealthComponent => _healthComponent;
    public ResourceComponent ManaComponent => _manaComponent;

    public MoveDto[] Moves { get; private set; } = new MoveDto[MoveCount];

    public List<(EffectDto, int)> Effects { get; private set; } = new();

    public abstract int Attack { get; }
    public abstract int Defense { get; }
    public abstract int Magic { get; }
    public abstract int Health { get; }
    public abstract int Mana { get; }

    protected abstract void InitializeSpecific(CharacterDto characterDto);

    protected abstract void OnEnableSpecific();

    protected abstract void OnDisableSpecific();

    protected abstract void OnDeath();

    private void OnEnable()
    {
        _healthComponent.ResourceEmpty += OnDeath;
        BattleEvents.BattleStarted += ResetEffectsAndResources;
        BattleEvents.HeroTurnStarted += ApplyEffectTick;
        BattleEvents.MonsterTurnStarted += ApplyEffectTick;
        BattleEvents.MonsterDied += Hide;
        BattleEvents.PlayerDied += Hide;
        BattleEvents.BattleStarted += Show;
        DontDestroyOnLoad(this.gameObject);
        OnEnableSpecific();
    }

    private void OnDisable()
    {
        _healthComponent.ResourceEmpty -= OnDeath;
        BattleEvents.BattleStarted -= ResetEffectsAndResources;
        BattleEvents.HeroTurnStarted -= ApplyEffectTick;
        BattleEvents.MonsterTurnStarted -= ApplyEffectTick;
        BattleEvents.MonsterDied -= Hide;
        BattleEvents.PlayerDied -= Hide;
        BattleEvents.BattleStarted -= Show;
        OnDisableSpecific();
    }

    public void Initialize(CharacterDto dto)
    {
        CharacterInfo = dto;
        _spriteRenderer.sprite = LookupDictionaries.Instance.GetCharacterSprite(dto.Type);

        _healthComponent.SetMaxResource(dto.Health, setResourceToMax: true);
        _manaComponent.SetMaxResource(dto.Mana, setResourceToMax: true);

        _manaComponent.RegenValue = StatsUtils.DefaultManaRegen;
        _healthComponent.AllowedMinForUsing = StatsUtils.DefaultMinHealth;

        for(int i = 0; i < Moves.Length; i++)
        {
            Moves[i] = CharacterInfo.Moves[i];
        }

        InitializeSpecific(dto);
    }

    /// <summary>
    /// Returns the cumulative effect value for effects of a given type.
    /// Debuffs are returned as negative values.
    /// </summary>
    public int GetEffectValue(EffectType effectType)
    {
        int amount = 0;

        foreach(var (effect, duration) in Effects)
        {
            if(effect.Type == effectType)
            {
                amount +=
                    ((effect.IsDebuff) ? -1 : 1)
                    * effect.Amount;
            }
        }

        return amount;
    }

    /// <summary>
    /// Ticks down the effect duration.
    /// Also applies health buffs/debuffs.
    /// (e.g., bleed,poison).
    /// </summary>
    private void ApplyEffectTick()
    {
        ApplyHealthBuffs();

        for(int i = Effects.Count - 1; i >= 0; i--)
        {
            var (effect, duration) = Effects[i];

            if(duration <= 1)
                Effects.RemoveAt(i);
            else
                Effects[i] = (effect, duration - 1);
        }
    }

    private void ApplyHealthBuffs()
    {
        int amount = 0;
        foreach(var effectType in StatsUtils.HealthBuffs)
        {
            amount += GetEffectValue(effectType);
        }

        _healthComponent.TakeResource(amount);
    }

    protected virtual void TakeDamageAndApplyEffects(DamageContext damageContext)
    {
        if(damageContext == null)
            return;

        int finalDamage = 0;

        switch(damageContext.Type)
        {
            case DamageType.NONE:
                finalDamage = 0;
                break;
            case DamageType.PHYSICAL:
                finalDamage = Mathf.Max(0, damageContext.DamageBeforeDefenseApplied - Defense);
                break;
            default:
                finalDamage = damageContext.DamageBeforeDefenseApplied;
                break;
        }

        if(damageContext.Effect != null)
            Effects.Add((damageContext.Effect, damageContext.Effect.Duration));

        _healthComponent.TakeResource(finalDamage);
    }

    private void Show()
    {
        _spriteRenderer.enabled = true;
    }

    private void Hide()
    {
        _spriteRenderer.enabled = false;
    }

    protected void ResetEffectsAndResources()
    {
        Effects = new List<(EffectDto, int)>();

        _healthComponent.SetMaxResource(Health, true);
        _manaComponent.SetMaxResource(Mana, true);
    }

    protected void CastMove(MoveDto move)
    {
        ApplyHealFrom(move);

        EffectDto effect = move.Effect;

        if(move.IsVFXAndEffectSelfCast && move.Effect != null)
        {
            Effects.Add((move.Effect, move.Effect.Duration));
            effect = null;
        }


        int damageBeforeDefense = CalculateDamageBeforeDefense(move);

        var damageContext = new DamageContext(CharacterInfo.Name, move.Name, damageBeforeDefense, move.DamageType, effect);
        var fxContext = new FXContext(move.SFXType, move.VFXType, move.Element, move.IsVFXAndEffectSelfCast);

        GameManager.Instance.PlayMove(damageContext, fxContext);
    }

    private int CalculateDamageBeforeDefense(MoveDto move)
    {
        switch(move.DamageType)
        {
            case DamageType.PHYSICAL:
                return move.DamageScaling * Attack;
            case DamageType.MAGICAL:
                return move.DamageScaling * Magic;
        }

        return 0;
    }

    private void ApplyHealFrom(MoveDto move) => HealthComponent.Add(move.SelfHealingScaling * Magic);
}
