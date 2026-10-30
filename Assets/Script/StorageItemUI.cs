using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StorageItemUI : MonoBehaviour, IPointerClickHandler
{
    [Header("Item Images")]
    [SerializeField] private Image frameImage;
    [SerializeField] private Image itemImage;

    private StorageItemData itemData;
    private StorageManager storageManager;

    public void Setup(StorageItemData item, StorageManager manager)
    {
        itemData = item;
        storageManager = manager;
        //untuk cek imagenya ada atau tidak, jika ada maka akan di set sprite nya, jika tidak maka akan di disable
        if (frameImage != null) 
        {
            frameImage.sprite = item.frameSprite;
            frameImage.enabled = item.frameSprite != null;
        }

        if (itemImage != null)
        {
            itemImage.sprite = item.itemSprite;
            itemImage.enabled = item.itemSprite != null;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //mendeteksi event klik pada UI item, jika ada item data dan storage manager maka akan memanggil fungsi sell item 
        if (itemData != null && storageManager != null)
        {
            storageManager.SellItem(itemData);
        }
    }
}