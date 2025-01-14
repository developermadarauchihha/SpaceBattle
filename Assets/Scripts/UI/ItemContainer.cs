using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemContainer
{
    private int itemID = int.MaxValue;
    private int quantity = 0;
    public int ItemID
    {
        get { return itemID; }
        set { itemID = value; }
    }
    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public Sprite Icon
    {
        get { return PartManager.Instance.GetPartById(ItemID).Icon; }
    }

    public ItemContainer() {}
    public ItemContainer(int id, int quant)
    {
        itemID = id;
        Quantity = quant;
    }

    public void SetComponents(int id, int quant)
    {
        itemID = id;
        Quantity = quant;
    }

}