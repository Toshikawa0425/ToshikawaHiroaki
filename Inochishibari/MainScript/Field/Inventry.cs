using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventry : SingletonMonoBehaviour<Inventry>
{
    [SerializeField]
    private int maxItemNum_Use = 10;
    [SerializeField]
    private int maxItemNum_Important = 10;
    public List<CardBase> itemList_USE = new List<CardBase>();
    public List<CardBase> itemList_IPT = new List<CardBase>();


    public void GetItem(CardBase _item, int _itemNum)
    {
        if (_item.itemAndSkillStates.isImportant)
        {
            for (int i = 0; i < _itemNum; i++)
            {
                if (itemList_IPT.Count == maxItemNum_Important)
                {
                    return;
                }

                itemList_IPT.Add(_item);
                itemList_IPT.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
            }
        }
        else
        {
            for (int i = 0; i < _itemNum; i++)
            {
                if (itemList_USE.Count == maxItemNum_Use)
                {
                    return;
                }

                itemList_USE.Add(_item);
                itemList_USE.Sort((a, b) => (int.Parse(a.CardID) - int.Parse(b.CardID)));
            }
        }
    }
}
