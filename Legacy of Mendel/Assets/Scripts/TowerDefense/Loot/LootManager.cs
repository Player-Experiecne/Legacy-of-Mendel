using UnityEngine;
using System.Collections.Generic;

public class LootManager : MonoBehaviour
{
    public static LootManager Instance { get; private set; }

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
    }

    public GameObject lootGeneADomPrefab;
    public GameObject lootGeneAHetPrefab;
    public GameObject lootGeneARecPrefab;
    public GameObject lootCultureMediumPrefab;

    public void DropLoot(Transform transform, List<GeneInfo.geneTypes> lootGeneTypes, int lootCultureMedium)
    {
        // Drop lootGene
        foreach (var lootGeneType in lootGeneTypes)
        {
            Vector3 dropPosition = GetRandomDropPosition(transform);
            if (lootGeneType == GeneInfo.geneTypes.ADom)
            {
                Instantiate(lootGeneADomPrefab, dropPosition, Quaternion.identity);
            }
            else if (lootGeneType == GeneInfo.geneTypes.AHet)
            {
                Instantiate(lootGeneAHetPrefab, dropPosition, Quaternion.identity);
            }
            else if (lootGeneType == GeneInfo.geneTypes.ARec)
            {
                Instantiate(lootGeneARecPrefab, dropPosition, Quaternion.identity);
            }
        }

        // Drop lootCultureMedium
        for (int i = 0; i < lootCultureMedium; i++)
        {
            Vector3 dropPosition1 = GetRandomDropPosition(transform);
            Instantiate(lootCultureMediumPrefab, dropPosition1, Quaternion.identity);
        }
    }

    private Vector3 GetRandomDropPosition(Transform transform)
    {
        float radius = 1.0f; // Adjust as needed
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection.y = 0; // Assuming a 2D game, or you don't want to change the height
        Vector3 dropPosition = transform.position + randomDirection;
        return dropPosition;
    }
}