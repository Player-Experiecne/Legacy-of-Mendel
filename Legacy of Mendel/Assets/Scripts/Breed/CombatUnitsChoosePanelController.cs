﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUnitsChoosePanelController : MonoBehaviour
{
    public static BreedManager Instance;

    public int itemsPerPage = 10;
    private int currentPage = 0;
    private int totalPageCount = 0;
    public Button nextPageButton;
    public Button previousPageButton;


    public PlayerDefenderInventory playerDefenderInventory;
    public List<Image> combatUnitImages;
    

    public Image defenderDisplayImage;
    public Button confirmButton; 
    public GameObject thisPanel;
    //public GameObject previousPanel; 

    private int lastInventoryCount = -1;


    private int selectedIndex = -1;

    void Start()
    {
        UpdatePageCount();
        RefreshDisplay();
        UpdatePageButtons();
    }
    public void NextPage()
    {
        if (currentPage < totalPageCount - 1)
        {
            currentPage++;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            RefreshDisplay();
            UpdatePageButtons();
        }
    }

    private void UpdatePageCount()
    {
        totalPageCount = Mathf.CeilToInt((float)playerDefenderInventory.ownedDefenders.Count / itemsPerPage);
    }

    private void UpdatePageButtons()
    {
        // 控制翻页按钮的可用性
        nextPageButton.interactable = currentPage < totalPageCount - 1;
        previousPageButton.interactable = currentPage > 0;

        // 如果总项目数少于每页显示数，则隐藏翻页按钮
        nextPageButton.gameObject.SetActive(totalPageCount > 1);
        previousPageButton.gameObject.SetActive(totalPageCount > 1);
    }



    void Update()
    {
        // 检查库存数量是否有变化
        if (playerDefenderInventory.ownedDefenders.Count != lastInventoryCount)
        {
            RefreshDisplay(); // 如果有变化，更新显示
            lastInventoryCount = playerDefenderInventory.ownedDefenders.Count; // 更新最后的库存数量，以便下次检查
        }
        if (HasInventoryCountChanged())
        {
            UpdateDefenderDisplays();
        }
    }

    private bool HasInventoryCountChanged()
    {
        // 这个方法检查每个defender的数量是否有变化
        // 如果是，则返回true，否则返回false
        for (int i = 0; i < playerDefenderInventory.ownedDefenders.Count; i++)
        {
            if (playerDefenderInventory.ownedDefenders[i].count != lastInventoryCount)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateDefenderDisplays()
    {
        for (int i = 0; i < playerDefenderInventory.ownedDefenders.Count; i++)
        {
            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    // 更新数量显示
                    countTextComponent.text = "Count: " + playerDefenderInventory.ownedDefenders[i].count.ToString();
                }
            }
        }
        // 更新最后的库存计数
        lastInventoryCount = playerDefenderInventory.ownedDefenders.Count;
    }
    public void RefreshDisplay()
    {
        for (int i = 0; i < combatUnitImages.Count; i++)
        {
            if (i + currentPage * itemsPerPage < playerDefenderInventory.ownedDefenders.Count)
            {
                int defenderIndex = currentPage * itemsPerPage + i;
                combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[defenderIndex].defender.defenderImage;
                combatUnitImages[i].gameObject.SetActive(true);

                // 更新其他显示的内容，例如名称和数量
                UpdateText(combatUnitImages[i], "NameText", playerDefenderInventory.ownedDefenders[defenderIndex].defender.defenderName);
                UpdateText(combatUnitImages[i], "CountText", "Count: " + playerDefenderInventory.ownedDefenders[defenderIndex].count.ToString());

                // 更新选中逻辑
                combatUnitImages[i].GetComponent<Button>().onClick.RemoveAllListeners();
                combatUnitImages[i].GetComponent<Button>().onClick.AddListener(() => {
                    ToggleSelection(defenderIndex);
                    SelectDefender(defenderIndex);
                });
            }
            else
            {
                combatUnitImages[i].gameObject.SetActive(false);
            }
        }
    }

    private void UpdateText(Image parentImage, string childName, string text)
    {
        Transform childTransform = parentImage.transform.Find(childName);
        if (childTransform != null)
        {
            TextMeshProUGUI textComponent = childTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = text;
            }
        }
    }


    private void PopulateCombatUnitImages()
    {
        int count = Mathf.Min(playerDefenderInventory.ownedDefenders.Count, combatUnitImages.Count);

        for (int i = 0; i < count; i++)
        {
            combatUnitImages[i].sprite = playerDefenderInventory.ownedDefenders[i].defender.defenderImage;
            Transform nameTextTransform = combatUnitImages[i].transform.Find("NameText");
            if (nameTextTransform != null)
            {
                TextMeshProUGUI nameTextComponent = nameTextTransform.GetComponent<TextMeshProUGUI>();
                if (nameTextComponent != null)
                {
                    nameTextComponent.text = "Name: " + playerDefenderInventory.ownedDefenders[i].defender.defenderName;
                }
            }

            Transform countTextTransform = combatUnitImages[i].transform.Find("CountText");
            if (countTextTransform != null)
            {
                TextMeshProUGUI countTextComponent = countTextTransform.GetComponent<TextMeshProUGUI>();
                if (countTextComponent != null)
                {
                    countTextComponent.text = "Count: " + playerDefenderInventory.ownedDefenders[i].count.ToString();
                }
            }
            combatUnitImages[i].gameObject.SetActive(true);
            int index = i; 
            combatUnitImages[i].GetComponent<Button>().onClick.RemoveAllListeners(); 
            combatUnitImages[i].GetComponent<Button>().onClick.AddListener(() => {
                ToggleSelection(index); 
                SelectDefender(index); 
            });
        }

        for (int i = count; i < combatUnitImages.Count; i++)
        {
            combatUnitImages[i].gameObject.SetActive(false);
        }
    }


    private void ToggleSelection(int index)
    {
        if (selectedIndex == index) 
        {
            combatUnitImages[index].color = Color.white; 
            defenderDisplayImage.sprite = null; 
            selectedIndex = -1;
        }
        else
        {
            if (selectedIndex != -1) 
            {
                combatUnitImages[selectedIndex].color = Color.white;
            }
            combatUnitImages[index].color = Color.green; 
            selectedIndex = index;
           
            defenderDisplayImage.sprite = playerDefenderInventory.ownedDefenders[index].defender.defenderImage;
        }
    }

    private void SelectDefender(int index)
    {
        
        defenderDisplayImage.sprite = playerDefenderInventory.ownedDefenders[index].defender.defenderImage;
        BreedManager.Instance.SetSelectedDefender(playerDefenderInventory.ownedDefenders[index].defender);
    }
    public void ConfirmSelection()
    {

        if (selectedIndex != -1)
        {
            combatUnitImages[selectedIndex].color = Color.white;
            //defenderDisplayImage.sprite = null;
            selectedIndex = -1; 
        }
        thisPanel.SetActive(false);
      
        
    }

    public void ClosePanel()
    {
        thisPanel.SetActive(false);

    }
}

