using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    // untuk UI nanti
    [Header("Storage UI")]
    [SerializeField] private Transform storageContent;
    [SerializeField] private StorageItemUI storageItemUI;

    //untuk atur koin player
    [Header("Player")]
    [SerializeField] private GachaManager gachaManager;

    //simpan data storage
    private List<StorageItemData> items = new List<StorageItemData>();

    //untuk tampilan item di UI
    [Header("Item Sprites")]
    [SerializeField] private Sprite swordSprite;
    [SerializeField] private Sprite metalFrame;
    [SerializeField] private Sprite bronzeFrame;
    [SerializeField] private Sprite silverFrame;
    [SerializeField] private Sprite goldFrame;
    [SerializeField] private Sprite diamondFrame;

    [Header("Storage Value UI")]
    [SerializeField] private UnityEngine.UI.Text storageCoinText;

    public void AddItem(string itemName, int itemValue)
    {
        //untuk item baru
        StorageItemData newItem = new StorageItemData();

        newItem.itemName = itemName;
        newItem.itemValue = itemValue;
        newItem.rarityOrder = GetRarityOrder(itemName);
        newItem.itemSprite = swordSprite;
        newItem.frameSprite = GetFrameSprite(itemName);

        //item baru masuk ke list
        items.Add(newItem);

        //setelah itu diurutkan dan di refresh UI
        SortItems();
        RefreshStorage();

        Debug.Log("Added to storage: " + itemName);
    }

    private int GetRarityOrder(string itemName)
    {
        switch (itemName)
        {
            case "Diamond":
                return 5;

            case "Gold":
                return 4;

            case "Silver":
                return 3;

            case "Bronze":
                return 2;

            case "Metal":
                return 1;

            default:
                return 0;
        }
    }

    private Sprite GetFrameSprite(string item_name)
    {
        switch (item_name)
        {
            case "Diamond":
                return diamondFrame;

            case "Gold":
                return goldFrame;

            case "Silver":
                return silverFrame;

            case "Bronze":
                return bronzeFrame;

            case "Metal":
                return metalFrame;

            default:
                return null;
        }
    }

    private void SortItems()
    {
        for (int i = 0; i < items.Count - 1; i++)
        {
            for (int j = 0; j < items.Count - 1 - i; j++)
            {
                //bubble sort dari tertinggi ke terendah
                if (items[j].rarityOrder <
                    items[j + 1].rarityOrder)
                {
                    StorageItemData temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
        }
    }

    private void RefreshStorage()
    {
        if (storageContent == null || storageItemUI == null)
        {
            //cek apakah ui prefab dan content sudah dihubung
            Debug.Log("Storage content or item UI prefab is not assigned.");
            return;
        }

        for (int i = storageContent.childCount - 1; i >= 0; i--)
        {
            //menghapus item UI di storage 
            Destroy(storageContent.GetChild(i).gameObject);
        }
        for (int i = 0; i < items.Count; i++)
        {
            StorageItemUI newUI = Instantiate(storageItemUI, storageContent);
            newUI.Setup(items[i], this);
        }

        UpdateStorageCoinText();
    }

    public void SellItem(StorageItemData item)
    {
        if (item == null || !items.Contains(item))
        {
            return;
        }

        items.Remove(item);

        if (gachaManager != null)
        {
            gachaManager.AddCoins(item.itemValue);
        }

        Debug.Log("Sold item: " + item.itemName + " for " + item.itemValue + " coins.");

        RefreshStorage();

        if (gachaManager != null)
        {
            gachaManager.CheckGameOver();
        }

    }

    public int GetStorageCoinValue()
    {
        //hitung total value dari semua item di storage (belum dijual)
        int totalValue = 0;
        for (int i = 0; i < items.Count; i++)
        {
            totalValue += items[i].itemValue;
        }
        return totalValue;
    }

    private void UpdateStorageCoinText()
    {
        if (storageCoinText != null)
        {
            storageCoinText.text = "Storage Value: " + GetStorageCoinValue().ToString();
        }
    }

    public int GetItemCount()
    {
        return items.Count;
    }
}