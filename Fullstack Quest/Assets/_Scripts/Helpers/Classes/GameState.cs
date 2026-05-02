using Newtonsoft.Json;
using NUnit.Framework;
using Shared.DataTransferObjects;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public int XpToNextLevelUp { get; private set; }
    public int LevelUpStatTokens { get; private set; }

    public int Gold { get; private set; }
    public long EncounterId { get; private set; }

    public bool IsEndless { get; private set; }
    public int LoopCount { get; private set; }

    public long HeroId { get; private set; }

    public List<ItemDto> OwnedItems { get; private set; }
    public List<MoveDto> LearnedMoves { get; private set; }

    public ItemDto[] Items { get; private set; }
    public MoveDto[] Moves { get; private set; }

    public HeroLevelsState HeroLevels { get; private set; }

    [JsonConstructor]
    public GameState(int xpToNextLevelUp, int levelUpStatTokens, int gold, long encounterId, bool isEndless, int loopCount, long heroId, List<ItemDto> ownedItems, List<MoveDto> learnedMoves, ItemDto[] items, MoveDto[] moves, HeroLevelsState heroLevels)
    {
        XpToNextLevelUp = xpToNextLevelUp;
        LevelUpStatTokens = levelUpStatTokens;

        Gold = gold;
        EncounterId = encounterId;

        IsEndless = isEndless;
        LoopCount = loopCount;

        HeroId = heroId;
        OwnedItems = ownedItems;

        LearnedMoves = learnedMoves;
        Items = items;

        Moves = moves;
        HeroLevels = heroLevels;
    }
}
