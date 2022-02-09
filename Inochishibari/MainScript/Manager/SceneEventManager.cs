using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventManager : SingletonMonoBehaviour<SceneEventManager>
{
    public bool isLoading = false;
    private GameObject sceneObj = null;

    private Scene currentScene;

    [SerializeField]
    private GameObject loadingObj;

    private void Start()
    {
        SceneManager.sceneLoaded += SceneLoaded;
        SceneManager.sceneUnloaded += SceneUnLoaded;
    }

    public void LoadScene(string _sceneName,LoadSceneMode _mode, int _evNum, bool _isBattle = false)
    {
        StartCoroutine(SceneLoading(_sceneName, _mode,_evNum,_isBattle)) ;
    }

    public void AddScene(string _sceneName, bool _activeChange)
    {
        Debug.Log("AddScene");
        StartCoroutine(AddSceneCoroutine(_sceneName,_activeChange));
    }

    public void ChangeScene(string _sceneName, int _evNum)
    {
        StartCoroutine(ChangeSceneCoroutine(_sceneName,_evNum));
    }

    private IEnumerator ChangeSceneCoroutine(string _sceneName, int _evNum)
    {
        isLoading = true;
        loadingObj.SetActive(true);
        Scene _nowScene = SceneManager.GetActiveScene();
        

        yield return null;

        SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive);

        yield return new WaitUntil(() => isLoading == false);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));

        yield return new WaitForSeconds(3.0f);

        isLoading = true;

        SceneManager.UnloadSceneAsync(_nowScene);

        yield return new WaitUntil(() => isLoading == false);
        loadingObj.SetActive(false);
        if (GameObject.Find("FirstEvent"))
        {
            GameObject.Find("FirstEvent").GetComponent<FirstEvent>().PlayEvent(_evNum);
        }
    }

    private IEnumerator AddSceneCoroutine(string _sceneName, bool activeChange)
    {
        isLoading = true;
        loadingObj.SetActive(true);
        Scene _nowScene = SceneManager.GetActiveScene();
        currentScene = _nowScene;
        yield return null;

        SceneManager.LoadSceneAsync(_sceneName,LoadSceneMode.Additive);

        yield return new WaitUntil(() => isLoading == false);
        loadingObj.SetActive(false);
        if (activeChange)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
        }
        else
        {
            SceneManager.SetActiveScene(_nowScene);
        }

        yield return null;
    }

    public void UnloadScene(int _evNum = -1)
    {
        StartCoroutine(SceneUnLoading(_evNum));
    }

    private IEnumerator SceneLoading(string _sceneName, LoadSceneMode _mode, int _evNum, bool _isBattle)
    {
        isLoading = true;
        loadingObj.SetActive(true);
        if (PlController_Field.Instance)
        {
            PlController_Field.Instance.CanMoveOff();
        }


        if(SceneObjDate.Instance != null)
        {
            SceneObjDate.Instance.TempSave();
        }

        yield return new WaitForSeconds(2.0f);
        

        if (_mode == LoadSceneMode.Additive)
        {
            GameManager.Instance.NotActivePlayerObj();
            sceneObj = GameObject.Find("SceneObj");
            sceneObj.SetActive(false);
        }

        yield return null;

        SceneManager.LoadSceneAsync(_sceneName, _mode);

        yield return new WaitUntil(()=> isLoading == false);
        loadingObj.SetActive(false);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));

        yield return null;
        if (SceneObjDate.Instance)
        {
            SceneObjDate.Instance.LoadSceneDate();
        }
        if (GameObject.Find("FirstEvent"))
        {
            GameObject.Find("FirstEvent").GetComponent<FirstEvent>().PlayEvent(_evNum);
        }
        //DisplayManager.Instance.GamenOpen();
    }

    public IEnumerator SceneUnLoading(int _evNum)
    {
        isLoading = true;
        /*
        DisplayManager.Instance.GamenClose(1.0f);
        yield return new WaitForSeconds(2.0f);
        */
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

        yield return new WaitUntil(() => isLoading == false);


        //GameManager.Instance.ActivePlayerObj();
        /*
        if (sceneObj != null)
        {
            sceneObj.SetActive(true);
            sceneObj = null;
        }
        */

        yield return null;
        SceneManager.SetActiveScene(currentScene);
        if(_evNum != -1 && GameObject.Find("FirstEvent"))
        {
            GameObject.Find("FirstEvent").GetComponent<FirstEvent>().PlayEvent(_evNum);
        }
        //EventManager.Instance.AfterTalk();
        //DisplayManager.Instance.GamenOpen(1.0f);
        //yield return new WaitForSeconds(2.0f);
        
    }

    private void SceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isLoading = false;
    }

    private void SceneUnLoaded(Scene scene)
    {
        if (sceneObj != null)
        {
            GameManager.Instance.ActivePlayerObj();
            sceneObj.SetActive(true);
        }
        sceneObj = null;
        isLoading = false;
    }


}
