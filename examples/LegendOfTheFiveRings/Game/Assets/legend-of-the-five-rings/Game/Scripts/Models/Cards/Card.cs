using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;


[DataContract]
[KnownType("DerivedTypes")]
public class Card {

	public virtual Sprite CardSprite => null;

	public T As<T>() where T : Card {
		return this as T;
	}

	public bool Is<T>() where T : Card {
		return this is T;
	}
	
	private static Type[] DerivedTypes() {
		return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(Card))).ToArray();
	}
	
}
