using DG.Tweening;
using Shared.DataTransferObjects;
using Shared.Enumerators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public abstract class BaseCharacter : MonoBehaviour
{
    public CharacterDto CharacterInfo { get; protected set; }
    public const int MoveCount = 4;

    [Header("Visuals & Sound")]
    [SerializeField] protected SpriteRenderer _spriteRenderer;
    [SerializeField] private AudioClip _hurtSound;
    [SerializeField] private float _healAndHurtFXDurationInSeconds = 0.5f;
    [SerializeField] private AudioClip _addBuffSound, _addDebuffSound;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private AudioClip _blockDamageSound;

    [Header("Resources")]
    [SerializeField] protected ResourceComponent _healthComponent;
    [SerializeField] protected ResourceComponent _manaComponent;

    public ResourceComponent HealthComponent => _healthComponent;
    public ResourceComponent ManaComponent => _manaComponent;

    public MoveDto[] Moves { get; private set; } = new MoveDto[MoveCount];

    public List<(EffectDto, int)> Effects { get; private set; } = new();

    public event Action AppliedEffectTick;

    public abstract int Attack { get; }
    public abstract int Defense { get; }
    public abstract int Magic { get; }
    public abstract int Health { get; }
    public abstract int Mana { get; }

    public abstract bool IsMyTurn { get; }

    protected abstract bool _flipVFXSprite { get; }

    protected abstract void InitializeSpecific(CharacterDto characterDto);

    protected abstract void OnEnableSpecific();

    protected abstract void OnDisableSpecific();

    protected abstract void OnDeath();

    private void OnEnable()
    {
        _healthComponent.ResourceEmpty += OnDeath;
        BattleEvents.BattleStarted += ResetEffectsAndResources;
        BattleEvents.MonsterDied += Hide;
        BattleEvents.PlayerDied += Hide;
        BattleEvents.BattleStarted += Show;
        BattleEvents.GlobalEffectApplied += AddGlobalEffect;
        GameEvents.SavedAndQuit += Hide;
        DontDestroyOnLoad(this.gameObject);
        OnEnableSpecific();
    }

    private void OnDisable()
    {
        _healthComponent.ResourceEmpty -= OnDeath;
        BattleEvents.BattleStarted -= ResetEffectsAndResources;
        BattleEvents.MonsterDied -= Hide;
        BattleEvents.PlayerDied -= Hide;
        BattleEvents.BattleStarted -= Show;
        BattleEvents.GlobalEffectApplied -= AddGlobalEffect;
        GameEvents.SavedAndQuit -= Hide;
        OnDisableSpecific();
    }

    public void Initialize(CharacterDto dto)
    {
        CharacterInfo = dto;
        _spriteRenderer.sprite = LookupDictionaries.Instance.GetCharacterSprite(dto.Type);

        _healthComponent.SetMaxResource(dto.Health, setResourceToMax: true);
        _manaComponent.SetMaxResource(dto.Mana, setResourceToMax: true);

        _manaComponent.RegenValue = StatsUtils.DefaultManaRegen;
        _healthComponent.AllowedMinForUse = StatsUtils.DefaultMinHealth;

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
    private void ApplyEffectTick(bool useHurtFX = false)
    {
        if(!IsMyTurn) return;

        ApplyHealthBuffs(useHurtFX);

        for(int i = Effects.Count - 1; i >= 0; i--)
        {
            var (effect, duration) = Effects[i];

            if(duration <= 0)
                Effects.RemoveAt(i);
            else
                Effects[i] = (effect, duration - 1);
        }

        AppliedEffectTick?.Invoke();
    }

    private void ApplyHealthBuffs(bool useHurtFX)
    {
        bool playFX = ApplyUseResourceBuff();
        playFX = ApplyTakeResourceBuff() || playFX;
        if(playFX && useHurtFX)
            PlayHurtFX();
    }

    private bool ApplyUseResourceBuff()
    {
        int amount = 0;
        foreach(var effectType in StatsUtils.UseResourceHealthBuffs)
        {
            amount += GetEffectValue(effectType);
        }
        int healthDelta = (int)(amount * _healthComponent.CurrentValue / 100f);
        return healthDelta < 0 && _healthComponent.UseResource(Mathf.Abs(healthDelta));
    }

    private bool ApplyTakeResourceBuff()
    {
        int amount = 0;
        foreach(var effectType in StatsUtils.TakeResourceHealthBuffs)
        {
            amount += GetEffectValue(effectType);
        }
        int healthDelta = (int)(amount * _healthComponent.CurrentValue / 100f);
        if(healthDelta < 0)
            _healthComponent.TakeResource(Mathf.Abs(healthDelta));
        return healthDelta < 0;
    }

    protected virtual void TakeDamageAndApplyEffects(DamageContext damageContext)
    {
        if(damageContext == null || !IsMyTurn)
        {
            ApplyEffectTick();
            return;
        }

        int finalDamage = 0;

        switch(damageContext.Type)
        {
            case DamageType.NONE:
                finalDamage = 0;
                break;
            case DamageType.PHYSICAL:
                finalDamage = damageContext.DamageBeforeDefenseApplied - Defense;
                break;
            default:
                finalDamage = damageContext.DamageBeforeDefenseApplied;
                break;
        }

        if(damageContext.Effect != null)
            AddEffectUnique(damageContext.Effect);

        if(finalDamage > 0)
        {
            PlayHurtFX();
        }
        else if(damageContext.Type == DamageType.PHYSICAL)
        {
            GameManager.Instance.PlayBlockedParticle(transform.position);
            AudioSource.PlayClipAtPoint(_blockDamageSound, Camera.main.transform.position);
        }
        _healthComponent.TakeResource(Mathf.Max(0, finalDamage));

        if(_healthComponent.CurrentValue > 0)
            ApplyEffectTick(useHurtFX: !(finalDamage > 0));
    }

    private void PlayHurtFX()
    {
        AudioSource.PlayClipAtPoint(_hurtSound, Camera.main.transform.position);
        DOTween.Sequence(_spriteRenderer)
            .Append(_spriteRenderer.DOColor(Color.red, _healAndHurtFXDurationInSeconds / 2f))
            .Append(_spriteRenderer.DOColor(Color.white, _healAndHurtFXDurationInSeconds / 2f));
    }

    private void AddEffectUnique(EffectDto effect, float sfxDelayInSeconds = 0f)
    {
        for(int i = 0; i < Effects.Count; i++)
        {
            if(Effects[i].Item1.Type == effect.Type)
            {
                Effects[i] = (effect, effect.Duration);
                StartCoroutine(PlayBuffSound(effect.IsDebuff, sfxDelayInSeconds));
                return;
            }
        }

        StartCoroutine(PlayBuffSound(effect.IsDebuff, sfxDelayInSeconds));
        Effects.Add((effect, effect.Duration));
    }

    private void AddGlobalEffect(EffectDto effect) => AddEffectUnique(effect);

    private IEnumerator PlayBuffSound(bool isDebuff, float sfxDelayInSeconds)
    {
        yield return new WaitForSeconds(sfxDelayInSeconds);
        if(isDebuff) AudioSource.PlayClipAtPoint(_addDebuffSound, Camera.main.transform.position);
        else AudioSource.PlayClipAtPoint(_addBuffSound, Camera.main.transform.position);
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

    protected void CastMoveInSlot(int slot) => CastMove(Moves[slot]);

    protected void CastMoveWithId(long moveId) => CastMove(Moves.FirstOrDefault(m => m.Id == moveId));

    private void CastMove(MoveDto move)
    {
        if(move == null) return;
        if(!IsMyTurn) return;

        var previousMana = _manaComponent.CurrentValue;
        var previousHealth = _healthComponent.CurrentValue;

        if(!(_healthComponent.UseResource(move.HealthCost) && _manaComponent.UseResource(move.ManaCost)))
        {
            _healthComponent.SetValue(previousHealth);
            _manaComponent.SetValue(previousMana);

            GameManager.Instance.SkipMove(CharacterInfo.Name, move.Name);
            return;
        }

        ApplyHealFrom(move);

        EffectDto effect = move.Effect;

        if(move.IsVFXAndEffectSelfCast && move.Effect != null)
        {
            AddEffectUnique(move.Effect);
            effect = null;
        }

        int damageBeforeDefense = CalculateDamageBeforeDefense(move);

        var damageContext = new DamageContext(CharacterInfo.Name, move.Name, damageBeforeDefense, move.DamageType, effect);
        var fxContext = new FXContext(move.SFXType, move.VFXType, move.Element, move.IsVFXAndEffectSelfCast, _flipVFXSprite, transform.position);

        GameManager.Instance.PlayMove(damageContext, fxContext);
    }

    private int CalculateDamageBeforeDefense(MoveDto move)
    {
        switch(move.DamageType)
        {
            case DamageType.PHYSICAL:
                return (int)((move.DamageScaling / (float)100f) * Attack);
            case DamageType.MAGICAL:
                return (int)((move.DamageScaling / (float)100f) * Magic);
        }

        return 0;
    }

    private void ApplyHealFrom(MoveDto move)
    {
        int scalingStat = move.DamageType == DamageType.PHYSICAL ? Attack : Magic;
        int finalHealing = (int)(scalingStat * move.SelfHealingScaling / 100f);

        if(finalHealing <= 0) return;

        DOVirtual.DelayedCall(GameManager.Instance.DelayBetweenMoveAndNextTurnInSeconds, () =>
        {
            HealthComponent.Add(finalHealing);
            PlayHealFX();
        });

    }

    private void PlayHealFX()
    {
        AudioSource.PlayClipAtPoint(_healSound, Camera.main.transform.position);
        DOTween.Sequence(_spriteRenderer)
            .Append(_spriteRenderer.DOColor(Color.green, _healAndHurtFXDurationInSeconds / 2f))
            .Append(_spriteRenderer.DOColor(Color.white, _healAndHurtFXDurationInSeconds / 2f));
    }
}
