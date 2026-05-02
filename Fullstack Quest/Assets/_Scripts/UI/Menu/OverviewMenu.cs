using Shared.DataTransferObjects;
using UnityEngine;

public class OverviewMenu : Menu<OverviewMenuData>
{
    [SerializeField] private EquippedMoveSlotsLoader _equippedMoveSlotsLoader;
    [SerializeField] private LearnedMovesListLoader _learnedMovesListLoader;
    [SerializeField] private MoveDetailsDisplay _moveDetailsDisplay;

    [SerializeField] private EquippedItemSlotsLoader _equippedItemSlotsLoader;
    [SerializeField] private OwnedItemsListLoader _ownedItemsListLoader;
    [SerializeField] private ItemDetailsDisplay _itemDetailsDisplay;

    [SerializeField] private UpcomingBattleCardLoader _upcomingBattleCardLoader;

    public override void Load(OverviewMenuData data)
    {
        _equippedMoveSlotsLoader.Load(data.Hero);
        _learnedMovesListLoader.Load(data.Hero, _moveDetailsDisplay);

        _equippedItemSlotsLoader.Load(data.Hero, _itemDetailsDisplay);
        _ownedItemsListLoader.Load(data.Hero, _itemDetailsDisplay);

        _upcomingBattleCardLoader.Load(data.CurrentEncounterNode.Left.Encounter, data.CurrentEncounterNode.Right.Encounter);
    }
}
