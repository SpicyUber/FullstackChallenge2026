using NUnit.Framework;
using Shared.DataTransferObjects;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectMenuData
{
    public List<CharacterDto> Heroes {get; private set;}

    public CharacterSelectMenuData(List<CharacterDto> heroes)
    {
        Heroes = heroes;
    }
}
