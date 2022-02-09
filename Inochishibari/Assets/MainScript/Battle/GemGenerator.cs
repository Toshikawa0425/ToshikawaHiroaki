using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemGenerator : MonoBehaviour
{
    public int gemNum = 0;
    [SerializeField]
    private Transform generatePos;
    [SerializeField]
    private List<GameObject> gemList = new List<GameObject>();
    [SerializeField]
    private TextMeshPro gemNumTMPro;

    

    public void GenerateGems(int _num)
    {
        while(_num > 0 && gemNum < 10)
        {
            GameObject _gem = gemList[gemNum];
            _gem.SetActive(true);
            _gem.transform.position = generatePos.position + Random.insideUnitSphere * 5.0f;
            _num--;
            gemNum++;
        }
        gemNumTMPro.text = gemNum + "/10";
    }

    public void UseGems(int _num)
    {
        while(_num > 0 && gemNum > 0)
        {
            GameObject _gem = gemList[gemNum - 1];
            _gem.SetActive(false);
            _num--;
            gemNum--;
        }
        gemNumTMPro.text = gemNum + "/10";
    }


    
}
