using Shared.DataTransferObjects;
using UnityEngine;

[CreateAssetMenu(fileName = "NetworkConfigSO", menuName = "Scriptable Objects/NetworkConfigSO")]
public class NetworkConfigSO : ScriptableObject
{
    [SerializeField] private string _baseURL;
    public string BaseURL => _baseURL;

    public string ApiPrefix => "/api";

    public string EncountersPrefix => "/encounters";
    public string RecommendedMovePrefix => "/move";

    public string ShopPrefix => "/shop";
    public string HeroesPrefix => "/heroes";

    public string GetMoveQuery(CharacterBattleStateDto characterBattleState)
        => $"?id={characterBattleState.Id}&currentHealth={characterBattleState.CurrentHealth}&maxHealth={characterBattleState.MaxHealth}" +
           $"&currentMana={characterBattleState.CurrentMana}&maxMana={characterBattleState.CurrentMana}";
}
