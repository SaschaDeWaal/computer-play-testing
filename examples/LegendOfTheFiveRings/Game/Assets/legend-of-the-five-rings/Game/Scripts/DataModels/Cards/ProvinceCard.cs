using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProvinceCard", menuName = "LegendOfFiveRings/Cards/Province")]

public class ProvinceCard : CardData {
    
    [Header("Default")]
    public string Name = "New ProvinceCard";
    public Sprite Icon;
    public Clan Clan;

    [Header("Values")] 
    public int Strength = -1;
    
    // Serialization
    private Sprite _iconSaved;

}
