using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : SingletonMonoBehaviour<MenuController>
{
    [SerializeField]
    private int menuNum = 0;

    [SerializeField]
    private GameObject circleMenu;
    [SerializeField]
    private Button centerButton;
    [SerializeField]
    private bool menuIsOpen = false;

    [SerializeField]
    private MenuType menuType;

    [SerializeField]
    private GameObject nowWindow;

    [SerializeField]
    private GameObject ItemWindow;

    [SerializeField]
    private Transform itemsParent;

    [SerializeField]
    private GameObject[] itemButtons_USE = new GameObject[10];
    [SerializeField]
    private GameObject[] itemButtons_AST = new GameObject[5];
    [SerializeField]
    private GameObject[] itemButtons_IPT = new GameObject[10];

    [SerializeField]
    private Image[] itemImage_USE = new Image[10];
    [SerializeField]
    private Image[] itemImage_AST = new Image[5];
    [SerializeField]
    private Image[] itemImage_IPT = new Image[10];

    public enum MenuType
    {
        Circle,
        States,
        Map,
        Item,
        Setting
    }

    // Update is called once per frame
    void LateUpdate()
    {

        if (InputSetting.Instance.Menu)
        {
            if (menuIsOpen)
            {
                CloseMenu();
            }
            else
            {
                if (PlController_Field.Instance.canMove)
                {
                    OpenMenu();
                }
            }
        }
    }

    private void OpenMenu()
    {
        PlController_Field.Instance.canMove = false;
        circleMenu.SetActive(true);
        centerButton.Select();
        menuIsOpen = true;
        menuType = MenuType.Circle;
    }

    private void CloseMenu()
    {
        switch (menuType)
        {
            default:
                PlController_Field.Instance.canMove = true;
                circleMenu.SetActive(false);
                menuIsOpen = false;
                break;

            case MenuType.Item:
                CloseItemWindow();
                circleMenu.SetActive(true);
                menuType = MenuType.Circle;
                centerButton.Select();
                break;
        }
        
    }

    public void OpenItemWindow()
    {
        circleMenu.SetActive(false);
        ItemWindow.SetActive(true);

        int _useNum = Inventry.Instance.itemList_USE.Count;
        //int _astNum = Inventry.Instance.itemList_AST.Count;
        int _iptNum = Inventry.Instance.itemList_IPT.Count;

        /*
        for(int i = 0; i < 10; i++)
        {

            if(i >= _useNum && i >= _astNum && i >= _iptNum)
            {
                break;
            }
            
            if(i < _astNum)
            {
                itemButtons_AST[i].SetActive(true);
                itemImage_AST[i].sprite = Inventry.Instance.itemList_AST[i].item.itemSprite;
            }

            if(i < _useNum)
            {
                itemButtons_USE[i].SetActive(true);
                itemImage_USE[i].sprite = Inventry.Instance.itemList_USE[i].item.itemSprite;
            }

            if(i < _iptNum)
            {
                itemButtons_IPT[i].SetActive(true);
                itemImage_IPT[i].sprite = Inventry.Instance.itemList_IPT[i].item.itemSprite;
            }

        }

        if (_useNum > 0 || _astNum > 0 || _iptNum > 0)
        {
            if (_useNum > 0)
            {
                itemButtons_USE[0].GetComponent<Button>().Select();
            }
            else if(_astNum > 0)
            {
                itemButtons_AST[0].GetComponent<Button>().Select();
            }
            else
            {
                itemButtons_IPT[0].GetComponent<Button>().Select();
            }
        }
        */
        menuType = MenuType.Item;

    }

    public void CloseItemWindow()
    {
        int _useNum = Inventry.Instance.itemList_USE.Count;
        //int _astNum = Inventry.Instance.itemList_AST.Count;
        int _iptNum = Inventry.Instance.itemList_IPT.Count;

        /*
        for(int i = 0; i < 10; i++)
        {
            if (i >= _useNum && i >= _astNum && i >= _iptNum)
            {
                break;
            }

            if (i < _astNum)
            {
                itemButtons_AST[i].SetActive(false);
                itemImage_AST[i].sprite = null;
            }

            if (i < _useNum)
            {
                itemButtons_USE[i].SetActive(false);
                itemImage_USE[i].sprite = null;
            }

            if (i < _iptNum)
            {
                itemButtons_IPT[i].SetActive(false);
                itemImage_IPT[i].sprite = null;
            }
        }
        */
        ItemWindow.SetActive(false);
    }
}
