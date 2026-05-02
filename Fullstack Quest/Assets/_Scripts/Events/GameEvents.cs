using Shared.DataTransferObjects;
using System;
using UnityEngine;

public class GameEvents
{
    public static event Action<CharacterDto> HeroSelected;
    public static event Action<CharacterDto> EncounterMonsterSelected;

    public static event Action<int> HeroLevelUp;
    public static event Action HeroBoughtUpgrade;

    public static event Action SavedAndQuit;

    public static void InvokeHeroSelected(CharacterDto dto) => HeroSelected?.Invoke(dto);
    public static void InvokeEncounterSelected(CharacterDto dto) => EncounterMonsterSelected?.Invoke(dto);

    public static void InvokeHeroLevelUp(int level) => HeroLevelUp?.Invoke(level);
    public static void InvokeHeroBoughtUpgrade() => HeroBoughtUpgrade?.Invoke();

    public static void InvokeSavedAndQuit() => SavedAndQuit?.Invoke();
}
