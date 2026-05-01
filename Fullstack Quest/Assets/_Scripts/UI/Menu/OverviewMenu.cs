using Shared.DataTransferObjects;
using UnityEngine;

public class OverviewMenu : Menu<OverviewMenuData>
{
    [SerializeField] private EquippedMoveSlotsLoader _equippedMoveSlotsLoader;
    [SerializeField] private LearnedMovesListLoader _learnedMovesListLoader;
    [SerializeField] private MoveDetailsDisplay _detailsDisplay;

    [SerializeField] private UpcomingBattleCardLoader _upcomingBattleCardLoader;

    public override void Load(OverviewMenuData data)
    {
        _equippedMoveSlotsLoader.Load(data.Hero);
        _learnedMovesListLoader.Load(data.Hero, _detailsDisplay);
        _upcomingBattleCardLoader.Load(data.CurrentEncounterNode.Left.Encounter, data.CurrentEncounterNode.Right.Encounter);
    }
}
