using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableSceneManager : SingletonMonoBehaviour<TableSceneManager>
{
    [SerializeField]
    private GameObject tableCam;

    public void TableCamActive()
    {
        tableCam.SetActive(true);
    }

    public void TableCamNoActive()
    {
        tableCam.SetActive(false);
    }
}
