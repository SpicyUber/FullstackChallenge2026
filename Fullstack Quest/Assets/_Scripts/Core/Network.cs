using Newtonsoft.Json;
using Shared.DataTransferObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Network : MonoBehaviour
{
    [SerializeField] private NetworkConfigSO _serverData;

    public long EnemyMoveId { get; private set; } = -1;
    public List<ItemDto> Shop { get; private set; } = new();

    public EncounterTreeNodeDto EncounterTree { get; private set; } = null;
    public List<CharacterDto> Heroes { get; private set; }

    public bool LastRequestSuccess { get; private set; }

    private void Start()
    {
        StartCoroutine(GetShop());
        StartCoroutine(GetMove(new(4, 2, 200, 200, 200)));
        StartCoroutine(GetEncounterTree());
        StartCoroutine(GetHeroes());
    }

    public IEnumerator GetShop()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(_serverData.BaseURL + _serverData.ApiPrefix + _serverData.ShopPrefix))
        {
            yield return request.SendWebRequest();

            LastRequestSuccess = request.result == UnityWebRequest.Result.Success;

            if(!LastRequestSuccess)
                yield break;

            Shop = JsonConvert.DeserializeObject<List<ItemDto>>(request.downloadHandler.text);
            Debug.Log("recieved\n" + request.downloadHandler.text);
        }
    }

    public IEnumerator GetMove(CharacterBattleStateDto characterState)
    {
        using(UnityWebRequest request = UnityWebRequest.Get(_serverData.BaseURL + _serverData.ApiPrefix + _serverData.RecommendedMovePrefix + _serverData.GetMoveQuery(characterState)))
        {
            yield return request.SendWebRequest();

            LastRequestSuccess = request.result == UnityWebRequest.Result.Success;

            if(!LastRequestSuccess)
                yield break;

            EnemyMoveId = JsonConvert.DeserializeObject<long>(request.downloadHandler.text);

            Debug.Log("recieved\n" + request.downloadHandler.text);
        }
    }

    public IEnumerator GetEncounterTree()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(_serverData.BaseURL + _serverData.ApiPrefix + _serverData.EncountersPrefix))
        {
            yield return request.SendWebRequest();

            LastRequestSuccess = request.result == UnityWebRequest.Result.Success;

            if(!LastRequestSuccess)
                yield break;

            EncounterTree = JsonConvert.DeserializeObject<EncounterTreeNodeDto>(request.downloadHandler.text);

            Debug.Log("recieved\n" + request.downloadHandler.text);
        }
    }

    public IEnumerator GetHeroes()
    {
        using(UnityWebRequest request = UnityWebRequest.Get(_serverData.BaseURL + _serverData.ApiPrefix + _serverData.HeroesPrefix))
        {
            yield return request.SendWebRequest();

            LastRequestSuccess = request.result == UnityWebRequest.Result.Success;

            if(!LastRequestSuccess)
                yield break;

            Heroes = JsonConvert.DeserializeObject<List<CharacterDto>>(request.downloadHandler.text);

            Debug.Log("recieved\n" + request.downloadHandler.text);
        }
    }

}
