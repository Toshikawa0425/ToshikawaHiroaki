using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System;

public class SaveDateManager : SingletonMonoBehaviour<SaveDateManager>
{
    public bool isSaving = false;
    public bool isLoading = false;

    [SerializeField]
    private string saveDateParentDirectoryPath;
    [SerializeField]
    private string tempDirectoryPath;
    [SerializeField]
    private string saveDateDirectoryPath;
    private string sceneDate_Temp_DirectoryPath;
    private string sceneDate_Saved_DirectoryPath;

    private string playerDatePath;


    public string GetSceneDate_Temp_DirectoryPath()
    {
        return sceneDate_Temp_DirectoryPath;
    }

    public string GetSceneDate_Saved_DirectoryPath()
    {
        return sceneDate_Saved_DirectoryPath;
    }

    [System.Serializable]
    public class PlayerDate
    {
        public List<PlayerCharactor> playerList = new List<PlayerCharactor>();
        public List<int> playerHPs = new List<int>();

        public string currentSceneName = "";
        public int posNum;
    }


    private void Start()
    {
        saveDateParentDirectoryPath = Path.Combine(Application.dataPath, "SaveDates");
        tempDirectoryPath = Path.Combine(saveDateParentDirectoryPath, "Temp");
        saveDateDirectoryPath = Path.Combine(saveDateParentDirectoryPath, "SaveDate");
        sceneDate_Temp_DirectoryPath = Path.Combine(tempDirectoryPath, "SceneDate");
        sceneDate_Saved_DirectoryPath = Path.Combine(saveDateDirectoryPath, "SceneDate");

        playerDatePath = Path.Combine(saveDateDirectoryPath, "PlayerDate.json");

        if (Directory.Exists(sceneDate_Temp_DirectoryPath))
        {
            Debug.Log("exist");
            Directory.Delete(sceneDate_Temp_DirectoryPath, true);
        }
    }

    public bool CheckSaveDateExist()
    {
        return File.Exists(playerDatePath);
    }



    public void ResetSaveDate()
    {
        StartCoroutine(CreateSaveDateDirectories());
    }

    private IEnumerator CreateSaveDateDirectories()
    {
        if (Directory.Exists(saveDateParentDirectoryPath))
        {
            Directory.Delete(saveDateParentDirectoryPath,true);
            yield return null;
        }

        Directory.CreateDirectory(saveDateParentDirectoryPath);
        yield return null;

        Directory.CreateDirectory(tempDirectoryPath);
        Directory.CreateDirectory(saveDateDirectoryPath);

        yield return null;
        Directory.CreateDirectory(sceneDate_Saved_DirectoryPath);
        Directory.CreateDirectory(sceneDate_Temp_DirectoryPath);
    }




    public void CreateDirectory_SceneDate()
    {
        if (!Directory.Exists(Application.dataPath + "/SceneSaveDate/" + SceneManager.GetActiveScene().name))
        {
            Directory.CreateDirectory(Application.dataPath + "/SceneSaveDate/" + SceneManager.GetActiveScene().name);
        }
    }

  

    public void PlayerDateSave(List<PlayerCharactor> _plCharas, List<int> _plHPs, string _sceneName, int _posNum)
    {
        PlayerDate _date = new PlayerDate();
        _date.playerList = _plCharas;
        _date.playerHPs = _plHPs;
        _date.currentSceneName = _sceneName;
        _date.posNum = _posNum;


        Debug.Log("PlayerDateSave");
        string jsonStr = JsonUtility.ToJson(_date);

        using (StreamWriter writer = new StreamWriter(playerDatePath, false))
        {
            try
            {
                writer.Write(jsonStr);
            }
            catch (Exception e)
            {
                Debug.LogError("セーブエラー:" + e);
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }

        

        StartCoroutine(SceneDateSave());
    }

    private IEnumerator SceneDateSave()
    {
        if (SceneObjDate.Instance)
        {
            SceneObjDate.Instance.TempSave();
            yield return null;
        }

        //tempフォルダ内のシーンデータフォルダをセーブフォルダに移動させる。
        //tempフォルダ内のシーンデータフォルダの中のデータフォルダのパスを取得
        string[] _sceneDirectories = Directory.GetDirectories(sceneDate_Temp_DirectoryPath,"*");

        if (_sceneDirectories.Length != 0)
        {
            for (int i = 0; i < _sceneDirectories.Length; i++)
            {
                //フォルダの名前を取得。
                string _name = Path.GetFileNameWithoutExtension(_sceneDirectories[i]);
                Debug.Log("name : " + _name);
                string _savePath = Path.Combine(sceneDate_Saved_DirectoryPath, _name);

                //セーブフォルダに同じ名前のフォルダがあるのなら、削除
                if (Directory.Exists(_savePath))
                {
                    Directory.Delete(_savePath,true);

                    yield return null;
                }

                //セーブフォルダへ移動
                Directory.Move(_sceneDirectories[i], _savePath);

                yield return null;
            }

                Directory.Delete(sceneDate_Temp_DirectoryPath, true);
        }

        
    }

    public void SceneDateSave_Temp(string _directoryPath, SceneObjDate.SaveObjects _date)
    {
        string _path = Path.Combine(_directoryPath, "ObjDate.json");
        Debug.Log(_path);
            

        if (!Directory.Exists(_directoryPath))
        {
            Directory.CreateDirectory(_directoryPath);
        }

        Debug.Log("ObjSave");
        string jsonStr = JsonUtility.ToJson(_date);
        using (StreamWriter writer = new StreamWriter(_path, false))
        {
            try
            {
                writer.Write(jsonStr);
            }
            catch (Exception e)
            {
                Debug.LogError("セーブエラー:" + e);
            }
            finally
            {
                writer.Flush();
                writer.Close();
            }
        }
    }

    public PlayerDate LoadPlayerDate()
    {
        PlayerDate _date = new PlayerDate();

        if (File.Exists(playerDatePath))
        {
            try
            {
                using (FileStream fs = new FileStream(playerDatePath, FileMode.Open))
                using (StreamReader reader = new StreamReader(fs))
                {
                    string jsomStr = reader.ReadToEnd();
                    reader.Close();
                    _date = JsonUtility.FromJson<PlayerDate>(jsomStr);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("ロードエラー:" + e);
            }

            return _date;
        }
        else
        {
            return null;
        }
    }

    

    public SceneObjDate.SaveObjects SceneDateLoad(string _directoryPath)
    {
        string _path = Path.Combine(_directoryPath, "ObjDate.json");
        SceneObjDate.SaveObjects _date = new SceneObjDate.SaveObjects();


        try
        {
            using (FileStream fs = new FileStream(_path, FileMode.Open))
            using (StreamReader reader = new StreamReader(fs))
            {
                string jsomStr = reader.ReadToEnd();
                reader.Close();
                _date = JsonUtility.FromJson<SceneObjDate.SaveObjects>(jsomStr);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("ロードエラー:" + e);
        }

        return _date;
    }
}
