using UnityEngine;

public class GoToOverviewOnClick : MonoBehaviour, IClickHandler
{
    public int Priority => -1;

    public void Handle()
    {
        GameManager.Instance.GoToOverview();
    }
}
