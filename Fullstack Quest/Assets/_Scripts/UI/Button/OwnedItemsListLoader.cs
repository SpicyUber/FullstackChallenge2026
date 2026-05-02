using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OwnedItemsListLoader : MonoBehaviour
{
    [SerializeField]
    RectTransform _scrollViewRectTransform;

    [SerializeField]
    GameObject _UnequippedItemPrefab;

    private List<GameObject> _loadedLearnedItems = new();

    public void Load(Hero hero, ItemDetailsDisplay detailsDisplay)
    {
        foreach(var item in _loadedLearnedItems)
        {
            Destroy(item);
        }
        _loadedLearnedItems.Clear();

        foreach(var item in hero.OwnedItems.GroupBy(m => m.Id).Select(m => m.First()).ToList())
        {
            var obj = Instantiate(_UnequippedItemPrefab, _scrollViewRectTransform);
            var itemDisplay = obj.GetComponent<UnequippedItemDisplay>();

            itemDisplay.SetItemAndDetailsPanel(item, detailsDisplay, hero);
            _loadedLearnedItems.Add(obj);
        }
    }
}
