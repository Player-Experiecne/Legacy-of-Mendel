using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GeneInfo;

public class LootBackpack : MonoBehaviour
{

    [System.Serializable]
    public class GeneTypeCount
    {
        public GeneInfo.geneTypes geneType;
        public int count;
    }
    public TextMeshProUGUI text;
    public static LootBackpack Instance { get; private set; }

    public List<GeneInfo.geneTypes> lootGeneTypes;
    public List<GeneTypeCount> geneTypeCountsList;

    public int lootCultureMedium = 0;
    
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        RefreshUI();
    }

    public void LootGeneType(GeneInfo.geneTypes geneType)
    {
       
        if (!lootGeneTypes.Contains(geneType))
        {
            lootGeneTypes.Add(geneType);
        }

        
        GeneTypeCount foundItem = geneTypeCountsList.Find(item => item.geneType == geneType);
        if (foundItem != null)
        {
            
            foundItem.count++;
        }
        else
        {
          
            geneTypeCountsList.Add(new GeneTypeCount { geneType = geneType, count = 1 });
        }
    }


    public void LootCultureMedium(int cultureMedium)
    {
        lootCultureMedium += cultureMedium;
        RefreshUI();
    }
    private void RefreshUI()
    {
        text.text = lootCultureMedium.ToString();
    }

}
