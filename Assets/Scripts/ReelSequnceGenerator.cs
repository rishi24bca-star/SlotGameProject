using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class ReelSequnceGenerator : MonoBehaviour
{
    [SerializeField] private Symbols[] allSymbols;

    private Symbols[] symbols = new Symbols[20];

    [SerializeField] private GameObject ReelPoint;
    [SerializeField] private ReelController reelController;

    [SerializeField] private float symbolSpacing = 0.85f;

    private void Start()
    {
        Called();
    }

    void Called()
    {
        if (allSymbols == null || allSymbols.Length < symbols.Length)
        {
            Debug.LogError(
                "You need at least 20 symbols in All Symbols " +
                "because your generator selects unique entries."
            );

            return;
        }

        bool[] usedIndices = new bool[allSymbols.Length];

        for (int index = 0; index < symbols.Length; index++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, allSymbols.Length);
            }
            while (usedIndices[randomIndex]);

            usedIndices[randomIndex] = true;

            symbols[index] = allSymbols[randomIndex];

            Debug.Log(symbols[index].SymbolName);
        }

        SpawnReelPoint();
    }

    void SpawnReelPoint()
    {
        float upwards = 0f;

        foreach (Symbols symbol in symbols)
        {
            GameObject sprite = Instantiate(
                symbol.Sprite,
                ReelPoint.transform
            );

            sprite.transform.localPosition =
                new Vector3(0f, upwards, 0f);

            upwards += symbolSpacing;
        }

        // Initialize only after all 20 symbols are spawned.
        reelController.Initialize(ReelPoint.transform);
    }
}