using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FootStep : MonoBehaviour
{
    private float[] footMap = new float[0];
    [SerializeField]
    private SeInfo nowFootStepSEInfo = null;

    private int currentNum = -1;
    private string currentTag = "";

    [Serializable]
    public class SeInfo
    {
        public AudioClip se;
        public float volume;
    }

    public void SetFootStep()
    {
        Ray ray = new Ray(transform.position + Vector3.up* 1.0f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            if (hit.collider.CompareTag("Terrain"))
            {
                currentTag = "";

                TerrainData terrainData = hit.collider.gameObject.GetComponent<Terrain>().terrainData;
                int _x = Mathf.FloorToInt(hit.textureCoord.x * terrainData.alphamapWidth);
                int _y = Mathf.FloorToInt(hit.textureCoord.y * terrainData.alphamapHeight);

                float[,,] alphaMaps = terrainData.GetAlphamaps(_x, _y, 1, 1);
                int layerCount = terrainData.alphamapLayers;

                if(footMap.Length == 0 || footMap.Length != layerCount)
                {
                    footMap = new float[layerCount];
                }

                for(int n = 0; n < layerCount; n++)
                {
                    footMap[n] = alphaMaps[0, 0, n];
                }

                int maxIndex = Array.IndexOf(footMap, Mathf.Max(footMap));

                if(currentNum != maxIndex)
                {
                    nowFootStepSEInfo = FootSteSettings.Instance.GetSE_Terrain(maxIndex);
                    currentNum = maxIndex;
                }
            }
            else
            {
                string _tag = hit.collider.tag;
                currentNum = -1;

                if (currentTag != _tag)
                {
                    nowFootStepSEInfo = FootSteSettings.Instance.GetSE_Obj(_tag);
                    currentTag = _tag;
                }
                
            }

            PlayFootSE();
        }
        else
        {
            currentNum = -1;
            currentTag = "";
            nowFootStepSEInfo = null;
        }
    }

    public void PlayFootSE()
    {
        if (nowFootStepSEInfo != null)
        {
            AudioPlayer_SE.Instance.PlaySE(nowFootStepSEInfo.se, nowFootStepSEInfo.volume);
        }
    }

    public void ResetFootSteps()
    {
        footMap = new float[0];
        currentNum = -1;
        currentTag = "";
        nowFootStepSEInfo = null;
    }
}
