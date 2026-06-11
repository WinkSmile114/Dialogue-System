using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Country Stats")]
    [SerializeField] [Range(-100,100)] public int EconomyAlignment; //Stat for planned/state economy, centre is mixed, - is planned, + is free market
    [SerializeField] [Range(-100,100)] public int EconomyHealth; //Growth rate for economy, 0 is no growth or shrinkage
    [SerializeField] [Range(-100,100)] public int EconomySize; //Size in GDP for economy (should start at -30)
    [SerializeField] [Range(-100,100)] public int SocialStats; //-100 being no rights 100 being many freedoms
    [SerializeField] [Range(-100,100)] public int TradeVolume; //Volume of trade, influenced by trade deals and effects economic growth

    [Header("Political Stats")]
    [SerializeField] [Range(-100,100)] public int PartyRelation; //Party relation with their own party
    [SerializeField] [Range(-100,100)] public int LeftRelation; //Relationship to left leaning parties
    [SerializeField] [Range(-100,100)] public int RightRelation; //Relationship to right leaning parties
    [SerializeField] [Range(-100,100)] public int CommunistRelation; //Relationship to commie party
    [SerializeField] [Range(-100,100)] public int AltRightRelation; //Relationship to alt right party

    [Header("Public Stats")]
    //Swing support will be entirely calculated from economy health and size
    [SerializeField] [Range(-100,100)] public int LeftSupport; //Support from left voters
    [SerializeField] [Range(-100,100)] public int RightSupport; //Support from right voters
    [SerializeField] [Range(-100,100)] public int SasanSupport; //Support from Sasan voters
    [SerializeField] [Range(-100,100)] public int AsterlundieSupport; //Support from Asterlundie voters
    [SerializeField] [Range(-100,100)] public int RegionalSupport; //Support from regional voters
    [SerializeField] [Range(-100,100)] public int CitySupport; //Support from city voters
    [SerializeField] [Range(-100,100)] public int PoorSupport; //Support from poor voters
    [SerializeField] [Range(-100,100)] public int RichSupport; //Support from rich oliglarchs

    [Header("International Relation Stats")]
    [SerializeField] [Range(-100,100)] public int WelticaRelation; //Relationship with Weltica
    [SerializeField] [Range(-100,100)] public int DiestlundRelation; //Relationship with Diestlund
    [SerializeField] [Range(-100,100)] public int AsterlundRelation; //Relationship with Asterlund
    [SerializeField] [Range(-100,100)] public int CriviaRelation; //Relationship with Crivia
    [SerializeField] [Range(-100,100)] public int ValmorRelation; //Relationship with Valmor
    [SerializeField] [Range(-100,100)] public int ErebraRelation; //Relationship with Erebra
    [SerializeField] [Range(-100,100)] public int SasanRelation; //Relationship with Sasan
    [SerializeField] [Range(-100,100)] public int KarsovaRelation; //Relationship with Karsova
    [SerializeField] [Range(-100,100)] public int AmariaRelation; //Relationship with Amaria

    [Header("Character Relations")]
    [SerializeField] [Range(-100,100)] public int Family; //Relationship with player family
}