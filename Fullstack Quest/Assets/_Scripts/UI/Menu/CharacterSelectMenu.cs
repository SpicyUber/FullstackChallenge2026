using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectMenu : Menu<CharacterSelectMenuData>
{
    [SerializeField]
    private GameObject _characterSelectDisplayPrefab;
    [SerializeField]
    private RectTransform _cardsPanelTransform;

    private List<GameObject> _heroCards = new();

    public override void Load(CharacterSelectMenuData data)
    {
        foreach(var card in _heroCards)
            Destroy(card.gameObject);

        _heroCards.Clear();

        foreach(var hero in data.Heroes)
        {
            var heroCard = Instantiate(_characterSelectDisplayPrefab, _cardsPanelTransform);
            heroCard.GetComponent<HeroSelectCard>().SetCharacter(hero);
            _heroCards.Add(heroCard);
            
        }
    }
}
