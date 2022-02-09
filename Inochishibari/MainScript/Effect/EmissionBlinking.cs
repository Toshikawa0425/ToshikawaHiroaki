using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmissionBlinking : MonoBehaviour
{
    private Material mat;
    [SerializeField]
    private float intensity;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        mat.EnableKeyword("_EMISSION");

        float factor = Mathf.Pow(2, intensity);
        mat.SetColor("_EmissionColor", new Color(0.0f * factor, 0.0f * factor, 1.0f * factor));
    }
}
