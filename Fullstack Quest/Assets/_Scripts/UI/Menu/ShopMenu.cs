using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : Menu<ShopMenuData>
{
    [SerializeField]
    private TextMeshProUGUI _gold;

    [SerializeField]
    private TextMeshProUGUI _levelUpTokens;

    [SerializeField]
    private RectTransform _itemPanel;

    [SerializeField]
    private GameObject _itemDisplayPrefab;

    [SerializeField]
    private UpgradeDisplay _udAttack, _udDefense, _udMagic, _udMana, _udHealth;

    [SerializeField]
    private Button[] _buttonsToDisableWhenOutOfUpgrades;

    public override void Load(ShopMenuData data)
    {
        LoadItemShop(data);
        LoadUpgradeShop(data);
        OnUpgrade();
        OnBuy();

    }

    private void LoadItemShop(ShopMenuData data)
    {
        foreach(var display in transform.GetComponentsInChildren<ShopItemDisplay>())
        {
            display.ItemBought -= OnBuy;
            Destroy(display.gameObject);
        }

        foreach(var item in data.Shop)
        {
            var display = Instantiate(_itemDisplayPrefab, _itemPanel).GetComponent<ShopItemDisplay>();
            display.SetItem(item);
            display.ItemBought += OnBuy;

        }
    }

    private void LoadUpgradeShop(ShopMenuData data)
    {
        _udAttack.SetFunctionality(
            () => GameManager.Instance.HeroAttackLevel + 1 + "",
            () =>
            {
                if(GameManager.Instance.LevelUpStatTokens == 0) return;
                GameManager.Instance.LevelUpStatTokens--;
                GameManager.Instance.HeroAttackLevel++;
            });

        _udDefense.SetFunctionality(
            () => GameManager.Instance.HeroDefenseLevel + 1 + "",
            () =>
            {
                if(GameManager.Instance.LevelUpStatTokens == 0) return;
                GameManager.Instance.LevelUpStatTokens--;
                GameManager.Instance.HeroDefenseLevel++;
            });

        _udMagic.SetFunctionality(
            () => GameManager.Instance.HeroMagicLevel + 1 + "",
            () =>
            {
                if(GameManager.Instance.LevelUpStatTokens == 0) return;
                GameManager.Instance.LevelUpStatTokens--;
                GameManager.Instance.HeroMagicLevel++;
            });

        _udHealth.SetFunctionality(
            () => GameManager.Instance.HeroHealthLevel + 1 + "",
            () =>
            {
                if(GameManager.Instance.LevelUpStatTokens == 0) return;
                GameManager.Instance.LevelUpStatTokens--;
                GameManager.Instance.HeroHealthLevel++;
            });

        _udMana.SetFunctionality(
            () => GameManager.Instance.HeroManaLevel + 1 + "",
            () =>
            {
                if(GameManager.Instance.LevelUpStatTokens == 0) return;
                GameManager.Instance.LevelUpStatTokens--;
                GameManager.Instance.HeroManaLevel++;
            });

        _udAttack.WasUpgraded -= OnUpgrade;
        _udDefense.WasUpgraded -= OnUpgrade;
        _udMana.WasUpgraded -= OnUpgrade;
        _udMagic.WasUpgraded -= OnUpgrade;
        _udHealth.WasUpgraded -= OnUpgrade;

        _udAttack.WasUpgraded += OnUpgrade;
        _udDefense.WasUpgraded += OnUpgrade;
        _udMana.WasUpgraded += OnUpgrade;
        _udMagic.WasUpgraded += OnUpgrade;
        _udHealth.WasUpgraded += OnUpgrade;
    }

    private void OnBuy() => _gold.SetText("" + GameManager.Instance.Gold);

    private void OnUpgrade()
    {
        _levelUpTokens.SetText("" + GameManager.Instance.LevelUpStatTokens);

        if(GameManager.Instance.LevelUpStatTokens == 0) DisableUpgradeButtons();
        else EnableUpgradeButtons();
    }

    private void EnableUpgradeButtons()
    {
        foreach(Button button in _buttonsToDisableWhenOutOfUpgrades)
            button.interactable = true;
    }

    private void DisableUpgradeButtons()
    {
        foreach(Button button in _buttonsToDisableWhenOutOfUpgrades)
            button.interactable = false;
    }
}

