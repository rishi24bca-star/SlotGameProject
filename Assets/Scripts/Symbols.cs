using UnityEngine;

[CreateAssetMenu(fileName = "Symbols", menuName = "Scriptable Objects/Symbols")]
public  class Symbols : ScriptableObject
{
    [SerializeField] string symbolName;
    [SerializeField] private int coins;
    [SerializeField] private GameObject sprite;
    public string SymbolName => symbolName;
    public int Coins => coins;
    public GameObject Sprite => sprite;
    
    
}
