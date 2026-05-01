using Shared.DataTransferObjects;
using UnityEngine;

public class OverviewMenuData
{
    public OverviewMenuData(Hero hero, EncounterTreeNodeDto currentEncounterNode)
    {
        Hero = hero;
        CurrentEncounterNode = currentEncounterNode;
    }

    public Hero Hero { get; private set; }
    public EncounterTreeNodeDto CurrentEncounterNode { get; private set; }

}
