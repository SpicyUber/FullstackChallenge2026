using Shared.DataTransferObjects;
using UnityEngine;

public class UpcomingBattleCardLoader : MonoBehaviour
{
    [SerializeField]
    private UpcomingBattleCard _leftBattleCard;
    [SerializeField]
    private UpcomingBattleCard _rightBattleCard;

    public void Load(EncounterDto leftChoice, EncounterDto rightChoice)
    {
        if(leftChoice != null)
        {
            _leftBattleCard.gameObject.SetActive(true);
            _leftBattleCard.SetEncounter(leftChoice, isRightChoice: false);
        }
        else
        {
            _leftBattleCard.gameObject.SetActive(false);
        }

        if(rightChoice != null)
        {
            _rightBattleCard.gameObject.SetActive(true);
            _rightBattleCard.SetEncounter(rightChoice, isRightChoice: true);
        }
        else
        {
            _rightBattleCard.gameObject.SetActive(false);
        }
    }
}
