using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObj : SingletonMonoBehaviour<PlayerObj>
{
    [SerializeField]
    private GameObject playerMain;

    public void PlayerOn()
    {
        playerMain.SetActive(true);
    }

    public void PlayerOff()
    {
        //playerMain.SetActive(false);

        Destroy(this.gameObject);
    }
}
