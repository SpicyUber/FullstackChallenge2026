using System.Linq;
using UnityEngine;

public class LearnedMovesListLoader : MonoBehaviour
{
    [SerializeField]
    RectTransform _scrollViewRectTransform;

    [SerializeField]
    GameObject _UnequippedMovePrefab;

    public void Load(Hero hero, MoveDetailsDisplay detailsDisplay)
    {
        foreach(var move in hero.LearnedMoves.GroupBy(m => m.Id).Select(m => m.First()).ToList())
        {
            Instantiate(_UnequippedMovePrefab, _scrollViewRectTransform).GetComponent<UnequippedMoveDisplay>().SetMoveAndDetailsPanel(move, detailsDisplay);
        }
    }
}
