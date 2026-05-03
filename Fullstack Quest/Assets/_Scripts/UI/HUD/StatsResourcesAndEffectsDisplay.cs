using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsResourcesAndEffectsDisplay : MonoBehaviour, IHUDElement
{
    [SerializeField] private Slider _heroHP, _heroMana, _monsterHP, _monsterMana;
    [SerializeField] private Image _heroProfile, _monsterProfile;

    [SerializeField] private TextMeshProUGUI _heroHPLabel, _heroManaLabel, _monsterHPLabel, _monsterManaLabel;
    [SerializeField] private TextMeshProUGUI _heroStatsLabel, _monsterStatsLabel;
    [SerializeField] private TextMeshProUGUI _heroNameLabel, _monsterNameLabel;

    [SerializeField] private Transform _heroBuffContainer, _monsterBuffContainer;

    [SerializeField] private GameObject _buffIconDisplayPrefab;

    private BaseCharacter _hero;
    private BaseCharacter _monster;

    private void OnEnable()
    {
        if(_hero)
        {
            _hero.AppliedEffectTick += Refresh;
            _hero.HealthComponent.ResourceChanged += Refresh;
            _hero.ManaComponent.ResourceChanged += Refresh;
        }

        if(_monster)
        {
            _monster.AppliedEffectTick += Refresh;
            _monster.HealthComponent.ResourceChanged += Refresh;
            _monster.ManaComponent.ResourceChanged += Refresh;
        }
  
        BattleEvents.GlobalEffectApplied += RefreshAfterGlobalEffect;
    }

    public void Initialize(BaseCharacter hero, BaseCharacter monster)
    {
        _hero = hero;
        _monster = monster;

        if(_hero)
        {
            _hero.AppliedEffectTick += Refresh;
            _hero.HealthComponent.ResourceChanged += Refresh;
            _hero.ManaComponent.ResourceChanged += Refresh;        }

        if(_monster)
        {
            _monster.AppliedEffectTick += Refresh;
            _monster.HealthComponent.ResourceChanged += Refresh;
            _monster.ManaComponent.ResourceChanged += Refresh;
        }

        Refresh();
    }

    public void Refresh()
    {
        _heroHP.maxValue = _hero.HealthComponent.MaxValue;
        _heroMana.maxValue = _hero.ManaComponent.MaxValue;

        _monsterHP.maxValue = _monster.HealthComponent.MaxValue;
        _monsterMana.maxValue = _monster.ManaComponent.MaxValue;

        _heroHP.value = _hero.HealthComponent.CurrentValue;
        _heroMana.value = _hero.ManaComponent.CurrentValue;

        _monsterHP.value = _monster.HealthComponent.CurrentValue;
        _monsterMana.value = _monster.ManaComponent.CurrentValue;

        _heroProfile.sprite = LookupDictionaries.Instance.GetCharacterSprite(_hero.CharacterInfo.Type);
        _monsterProfile.sprite = LookupDictionaries.Instance.GetCharacterSprite(_monster.CharacterInfo.Type);

        _heroHPLabel.SetText($"{_hero.HealthComponent.CurrentValue}/{_hero.HealthComponent.MaxValue}");
        _heroManaLabel.SetText($"{_hero.ManaComponent.CurrentValue}/{_hero.ManaComponent.MaxValue}");

        _monsterHPLabel.SetText($"{_monster.HealthComponent.CurrentValue}/{_monster.HealthComponent.MaxValue}");
        _monsterManaLabel.SetText($"{_monster.ManaComponent.CurrentValue}/{_monster.ManaComponent.MaxValue}");

        _heroStatsLabel.SetText($"A {_hero.Attack} D {_hero.Defense} M {_hero.Magic}");
        _monsterStatsLabel.SetText($"A {_monster.Attack} D {_monster.Defense} M {_monster.Magic}");

        _heroNameLabel.SetText(_hero.CharacterInfo.Name);
        _monsterNameLabel.SetText(_monster.CharacterInfo.Name);

        RefreshBuffs(_heroBuffContainer, _hero.Effects);
        RefreshBuffs(_monsterBuffContainer, _monster.Effects);
    }

    private void RefreshAfterGlobalEffect(EffectDto effect) => Refresh();

    private void RefreshBuffs(Transform buffContainer, List<(EffectDto, int)> effects)
    {
        foreach(Transform child in buffContainer)
        {
            Destroy(child.gameObject);
        }

        foreach(var (effect, duration) in effects)
        {
            GameObject icon = Instantiate(_buffIconDisplayPrefab, buffContainer);
            icon.GetComponent<Image>().sprite = LookupDictionaries.Instance.GetBuffSprite(effect.Type, effect.IsDebuff);
        }
    }

    private void OnDisable()
    {
        if(_hero)
        {
            _hero.AppliedEffectTick -= Refresh;
            _hero.HealthComponent.ResourceChanged -= Refresh;
            _hero.ManaComponent.ResourceChanged -= Refresh;
        }

        if(_monster)
        {
            _monster.AppliedEffectTick -= Refresh;
            _monster.HealthComponent.ResourceChanged -= Refresh;
            _monster.ManaComponent.ResourceChanged -= Refresh;
        }

        BattleEvents.GlobalEffectApplied -= RefreshAfterGlobalEffect;

    }
}
