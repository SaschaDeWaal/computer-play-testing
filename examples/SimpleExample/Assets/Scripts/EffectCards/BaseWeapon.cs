using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

[DataContract]
[KnownType("DerivedTypes")]
public class BaseWeapon {

	public virtual string Name => "BaseEffectCad";
	public virtual int Power => 0;
	public virtual int LifeTime => 0;

	[DataMember] private int lifeCount;

	public BaseWeapon() {
		lifeCount = LifeTime;
	}

	public void UsedLife() {
		lifeCount--;
	}

	public int GetCurrentLifeCount() {
		return lifeCount;
	}
	
	public bool ShouldBeRemoved() {
		return (lifeCount < 0);
	}
	
	private static Type[] DerivedTypes() {
		return Assembly.GetExecutingAssembly().GetTypes().Where(_ => _.IsSubclassOf(typeof(BaseWeapon))).ToArray();
	}
}
