using System;
using UnityEngine;
using UnityEngine.UI;
public class GachaManager : MonoBehaviour
{
    // koin player, cost, storage
    [SerializeField] private int playerCoins = 200; //koin awal player
    [SerializeField] private int gachaCost = 30; //koin yang tersimpan di gacha
    //private int storageCoinValue = 0;

    //untuk sambungin ke storemanager
    [SerializeField] private StorageManager storageManager;

    // input persentase gacha
    [Header("Gacha Item Percentage Inputs")]
    [SerializeField] private InputField diamondInput;
    [SerializeField] private InputField goldInput;
    [SerializeField] private InputField silverInput;
    [SerializeField] private InputField bronzeInput;
    [SerializeField] private InputField metalInput;

    //total koin
    [SerializeField] private Text totalCoinText;

    //value gacha nya
    [Header("Gacha Item Values")]
    [SerializeField] private int metalValue = 10;
    [SerializeField] private int bronzeValue = 20;
    [SerializeField] private int silverValue = 30;
    [SerializeField] private int goldValue = 40;
    [SerializeField] private int diamondValue = 50;


    private void Start()
    {
        UpdateCoinUI();
    }

    public void Gacha()
    {
        Debug.Log("Gacha button clicked");
        if (playerCoins < gachaCost)
        {
            Debug.Log("Not enough coins");
            return;
        }

        // Logika requirement persentase
        float[] percentages = getPercentage(); //mengambil persentase dari input field menggunakan array
        float totalPercentage = 0; // ini buat hitung nanti

        for(int i = 0; i < percentages.Length; i++) //loop di index dalam batas array
        {
            if(percentages[i] < 0) //ga boleh minus
            {
                Debug.Log("Invalid percentages: cannot be negative");
                return;
            }
            totalPercentage += percentages[i];
        }

        if (totalPercentage == 0)
        {
            Debug.Log("Invalid percentages: total cannot be zero");
            return;
        }

        //Logika persentase efektif
        float[] effectivePercentage = new float[percentages.Length];
        for(int i = 0; i < percentages.Length; i++)
        {
            effectivePercentage[i] = (percentages[i] / totalPercentage) * 100f; //buat persentase efektif
        }

        // Logika koin player
        playerCoins -= gachaCost;
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        float currentPercentage = 0f;

        string[] rarity =
        {   "Metal",
            "Bronze",
            "Silver",
            "Gold",
            "Diamond"
        };

        int[] itemValue =
        {
            metalValue,
            bronzeValue,
            silverValue,
            goldValue,
            diamondValue
        };

        // Logika persentase rarity
        int selectedRarity = 0;
        for(int i = 0; i < effectivePercentage.Length; i++)
        {
            currentPercentage += effectivePercentage[i];
            if(randomValue < currentPercentage) //buat cek random value masuk ke dalam rentang
            {
                selectedRarity = i;
                break;
            }
        }

        string itemName       = rarity[selectedRarity]; //ambil item name sesuai rarity yang dipilih
        int itemValueSelected = itemValue[selectedRarity]; //ambil item valuenya
        Debug.Log("You got: " + itemName);
        //hasil diatas sambungin ke store manager
        if (storageManager != null)
        {
            storageManager.AddItem(itemName, itemValueSelected);
        }
        else
        {
            Debug.LogError("StorageManager belum dihubungkan!");
        }

        UpdateCoinUI();
        CheckGameOver();
    }
    private float[] getPercentage()
    {
        return new float[]
        {
            GetInputValue(metalInput),
            GetInputValue(bronzeInput),
            GetInputValue(silverInput),
            GetInputValue(goldInput),
            GetInputValue(diamondInput)
        };

    }

    private float GetInputValue(InputField inputField) //text berubah ke float
    {
        if(inputField == null)
        {
            return 0f;
        }
        if (float.TryParse(inputField.text, out float value))
        {
            return value;
        }
        return 0f;
    }
    private void UpdateCoinUI()
    {
        if (totalCoinText != null)
        {
            totalCoinText.text = "Total Coins: " + playerCoins.ToString();
        }
    }

    public int GetPlayerCoins()
    {
        //untuk ambil nilai koin player ke script lain
        return playerCoins;
    }

    public void AddCoins(int amount)
    {
        //ini untuk hitung koin player (kalau ada sell)
        playerCoins += amount;
        UpdateCoinUI();
        CheckGameOver();
    }

    public void CheckGameOver()
    {
        int currentStorageValue = 0;

        if (storageManager != null)
        {
            currentStorageValue = storageManager.GetStorageCoinValue();
        }

        if (playerCoins + currentStorageValue < 30)
        {
            Debug.Log("Game Over");
        }
    }
}
