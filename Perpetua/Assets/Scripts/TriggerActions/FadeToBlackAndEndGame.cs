using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Trigger Actions/Fade to Black and End Game")]
public class FadeToBlackAndEndGame : TriggerAction
{
    public override void DoAction()
    {
        ColorOverlay.FadeToBlack();
        ColorOverlay.Instance.StartCoroutine(Credits());
    }

    public IEnumerator Credits()
    {
        MenuPresenter.ShowCredits();
        yield return new WaitForSeconds(30f);
        MenuPresenter.Instance.credits.SetActive(false);
        MenuPresenter.Instance.gameObject.SetActive(true);
        MenuPresenter.Instance._tc.SetTab(MenuPresenter.Instance.TitleScreen);
        ColorOverlay.FadeToTransparent();
    }
}