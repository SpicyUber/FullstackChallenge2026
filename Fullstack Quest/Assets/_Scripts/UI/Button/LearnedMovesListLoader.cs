using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LearnedMovesListLoader : MonoBehaviour
{
    [SerializeField]
    RectTransform _scrollViewRectTransform;

    [SerializeField]
    GameObject _UnequippedMovePrefab;

    private List<GameObject> _loadedLearnedMoves = new();

    public void Load(Hero hero, MoveDetailsDisplay detailsDisplay)
    {
        foreach(var move in _loadedLearnedMoves)
        {
            Destroy(move);
        }
        _loadedLearnedMoves.Clear();

        foreach(var move in hero.LearnedMoves.GroupBy(m => m.Id).Select(m => m.First()).ToList())
        {
            var obj = Instantiate(_UnequippedMovePrefab, _scrollViewRectTransform);
            var moveDisplay = obj.GetComponent<UnequippedMoveDisplay>();

            moveDisplay.SetMoveAndDetailsPanel(move, detailsDisplay);
            _loadedLearnedMoves.Add(obj);
        }
    }
}
