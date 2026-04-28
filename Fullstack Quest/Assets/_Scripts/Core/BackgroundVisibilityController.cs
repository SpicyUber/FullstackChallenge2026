using Shared.Enumerators;
using UnityEngine;

public class BackgroundVisibilityController : MonoBehaviour
{
    [SerializeField] private GameObject _easyBG, _mediumBG, _hardBG;

    public void ShowBackgroundFromDifficulty(Difficulty difficulty)
    {
        HideBackgrounds();

        switch(difficulty)
        {
            case Difficulty.EASY:
                _easyBG.SetActive(true);
                break;
            case Difficulty.MEDIUM:
                _mediumBG.SetActive(true);
                break;
            default:
                _hardBG.SetActive(true);
                break;
        }
    }

    public void HideBackgrounds()
    {
        _easyBG.SetActive(false);
        _mediumBG.SetActive(false);
        _hardBG.SetActive(false);
    }
}
