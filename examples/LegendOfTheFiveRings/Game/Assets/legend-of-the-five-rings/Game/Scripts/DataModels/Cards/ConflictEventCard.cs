using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "NewConflictEventCard", menuName = "LegendOfFiveRings/Cards/Conflict/Event")]
public class ConflictEventCard : CardData {
    
	public enum EventAbility {
		Reaction,
		Interrupt,
		Action
	}
	
	public string Name = "NewEvent";
	public Sprite Icon;
	public Clan clan;
	public int cost;

	[Header("Ability")] 
	public EventAbility ability = EventAbility.Action;

	public bool hasCondition = false;

	[ConditionalField("hasCondition")]
	public PlayCondition Conditon;

	[ConditionalField("hasCondition")] 
	public ActionEffect[] Effect;
	
}
