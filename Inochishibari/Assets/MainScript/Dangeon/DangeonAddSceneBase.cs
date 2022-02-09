using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DangeonAddSceneBase : MonoBehaviour
{
    [SerializeField]
    private protected Animator mainAnim;
    public virtual void InitState()
    {
        TableSceneManager.Instance.TableCamActive();
    }

    public virtual void SelectFirstCard()
    {
    }
    public void CamSet()
    {
        TableSceneManager.Instance.TableCamNoActive();
    }

    public virtual void CloseScene()
    {
        //DeckManager.Instance.SetDeckAll(mainDeck_Base, skills_Base, items_Base, adds_Base);
        mainAnim.SetTrigger("Close");
    }

    public virtual void UnLoadScene()
    {
        SceneEventManager.Instance.UnloadScene(1);
    }
}
