using TMPro;
using UnityEngine;

public class CoinCalculator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] Canvas canvas;
    private int CoinMax = 1000;
    private int Cost = 50;
    private int currentCoin ;
    // Update is called once per frame
    void Awake()
    {
        currentCoin = CoinMax;
        coinText.text = "Coins" + currentCoin.ToString();
    }

   public  void CointDeduction()
    {

        currentCoin = currentCoin -  Cost;
        coinText.text = "Coins: " + currentCoin;

    }

    public void CoinAddition(int amount)
    {
       
        currentCoin += amount;
        canvas.enabled = true;
        if (canvas.enabled)
        {
            Invoke(nameof(DisableCanvas), 3f);
        }
        coinText.text = "Coins" + currentCoin.ToString();
    }
    void DisableCanvas()
    {
        canvas.enabled = false;
    }
}
