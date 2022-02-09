using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTest : MonoBehaviour
{
    [SerializeField]
    private GameObject Managers;

    [SerializeField]
    private bool loadTableScene;

    [SerializeField]
    private FirstEvent firstEvent = null; 

    [SerializeField]
    private int evNum;

    private void Start()
    {
        if (GameManager.Instance == null || (loadTableScene && !GameObject.Find("TableSceneObj")))
        {
            StartCoroutine(InitStateCoroutine());
        }
    }

    private IEnumerator InitStateCoroutine()
    {
        Instantiate(Managers);

        yield return null;

        if (loadTableScene)
        {
            if (!GameObject.Find("TableSceneObj"))
            {
                Debug.Log("loadTableScene");
                SceneEventManager.Instance.AddScene("TableScene", false);

                yield return null;
            }
        }

        yield return new WaitUntil(() => SceneEventManager.Instance.isLoading == false);

        if(firstEvent != null)
        {
            firstEvent.PlayEvent(evNum);
        }
    }
}
