using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TitleManager : MonoBehaviour
{
    private string saveDatepath;

    [SerializeField]
    private string newGameSceneName;
    [SerializeField]
    private int newGameSceneEvNum;

    [SerializeField]
    private Card_Object newGameCard;
    [SerializeField]
    private Card_Object continueCard;

    [SerializeField]
    private TextMeshProUGUI continueText;

    [SerializeField]
    private Color noEnableColor;

    private bool canContinue = false;

    private bool started = false;

    private Animator animator;

    public CardSE cardSE;

    [System.Serializable]
    public class CardSE
    {
        public AudioClip open;
        public AudioClip play;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenCard_New()
    {
        Debug.Log("open");
        newGameCard.RotateCard();
        PlaySE_Open();
    }

    public void OpenCard_Continue()
    {
        Debug.Log("open");
        continueCard.RotateCard();
        PlaySE_Open();
    }

    public void StartSetting()
    {
        if (!started)
        {
            
            Debug.Log("startset");
            newGameCard.SelectThisCard();
            started = true;
        }
    }

    public void StartNewGame()
    {
        PlaySE_Play();
        ButtonManager.Instance.ResetLastButton();
        animator.SetTrigger("Close");
        DisplayManager.Instance.GamenClose(3.0f);
        Invoke("LoadNewGameScene", 2.0f);
    }

    public void ContinueGame()
    {
        ButtonManager.Instance.ResetLastButton();
        animator.SetTrigger("Close");
        DisplayManager.Instance.GamenClose(3.0f);
        Invoke("LoadContinueScene", 2.0f);
    }

    public void LoadNewGameScene()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene(newGameSceneName, newGameSceneEvNum);
    }

    public void LoadContinueScene()
    {
        TableSceneManager.Instance.TableCamActive();
        SceneEventManager.Instance.ChangeScene("WorldMap",0);
    }

    public void PlaySE(AudioClip _clip)
    {
        AudioPlayer_SE.Instance.PlaySE(_clip, 0.4f);
    }

    public void PlaySE_Open()
    {
        AudioPlayer_SE.Instance.PlaySE(cardSE.open, 0.4f);
    }

    public void PlaySE_Play()
    {
        AudioPlayer_SE.Instance.PlaySE(cardSE.play, 0.4f);
    }
}
