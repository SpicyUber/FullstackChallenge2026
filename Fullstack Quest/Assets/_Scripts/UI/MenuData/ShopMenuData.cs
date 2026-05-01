using Shared.DataTransferObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopMenuData
{
    public int Gold { get; private set; }
    public int LevelUpTokens { get; private set; }

    public List<ItemDto> Shop { get; private set; }

    public ShopMenuData(int gold, int levelUpTokens, List<ItemDto> shop)
    {
        Gold = gold;
        Shop = shop;
        LevelUpTokens = levelUpTokens;
    }
}
