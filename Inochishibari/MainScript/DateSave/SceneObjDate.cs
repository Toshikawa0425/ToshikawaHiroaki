using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class SceneObjDate : SingletonMonoBehaviour<SceneObjDate>
{
    [SerializeField]
    private List<GameObject> targetObjList = new List<GameObject>();
    [SerializeField]
    private SaveObjects saveObjects = new SaveObjects();

    [SerializeField]
    private string tempDatePath;
    private string saveDatePath;



    [System.Serializable]
    public class SaveObjects
    {
        public List<ObjStates> objectStatesList = new List<ObjStates>();
    }

    [System.Serializable]
    public class ObjStates
    {
        public int obj_ID;
        public bool isActive;
        public Vector3 pos;
    }



    public void TempSave()
    {
        Debug.Log("TempSave");
        tempDatePath = Path.Combine(SaveDateManager.Instance.GetSceneDate_Temp_DirectoryPath(), SceneManager.GetActiveScene().name);
        foreach (ObjStates _st in saveObjects.objectStatesList)
        {
            GameObject _obj = targetObjList[_st.obj_ID];
            _st.pos = _obj.transform.position;
            _st.isActive = _obj.activeSelf;
        }

        SaveDateManager.Instance.SceneDateSave_Temp(tempDatePath, saveObjects);
    }

    /*
    public void AddToList(GameObject _obj)
    {
        ObjStates _st = new ObjStates();
        _st.objName = _obj.name;
        //_st.transform = _obj.transform;
        saveObjects.objectStatesList.Add(_st);
    }
    */

    public void LoadSceneDate()
    {

        tempDatePath = Path.Combine(SaveDateManager.Instance.GetSceneDate_Temp_DirectoryPath(), SceneManager.GetActiveScene().name);

        Debug.Log(tempDatePath);
        //temp�t�H���_����SceneDate�t�H���_�ɃV�[���f�[�^�����邩�B
        if (Directory.Exists(tempDatePath))
        {
            Debug.Log("�V�[���f�[�^���[�hfromTemp");
            saveObjects = SaveDateManager.Instance.SceneDateLoad(tempDatePath);
            SetObjectsStates();
            return;
        }

        //temp�t�H���_���ɑ��݂��Ȃ��ꍇ
        //�Z�[�u�f�[�^�t�H���_���ɃV�[���f�[�^�����邩�B
        saveDatePath = Path.Combine(SaveDateManager.Instance.GetSceneDate_Saved_DirectoryPath(), SceneManager.GetActiveScene().name);

        if (Directory.Exists(saveDatePath))
        {
            Debug.Log("�V�[���f�[�^���[�hfromSave");
            saveObjects = SaveDateManager.Instance.SceneDateLoad(saveDatePath);
            SetObjectsStates();
            return;
        }

        //�V�[���f�[�^�����݂��Ȃ��̂Ȃ�A�����l��ݒ�B
        Debug.Log("�V�[���f�[�^�쐬");
        saveObjects = new SaveObjects();

        for(int i = 0; i < targetObjList.Count; i++)
        {
            ObjStates _st = new ObjStates();
            _st.obj_ID = i;

            saveObjects.objectStatesList.Add(_st);
        }
        return;
    }

    public void SetObjectsStates()
    {
        if (saveObjects.objectStatesList.Count != 0)
        {
            foreach(ObjStates _st in saveObjects.objectStatesList)
            {
                GameObject _obj = targetObjList[_st.obj_ID];

                _obj.transform.position = _st.pos;
                _obj.SetActive(_st.isActive);
            }
        }
        //saveObjects = new SaveObjects();
    }
}
