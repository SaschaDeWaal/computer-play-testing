using System.Runtime.Serialization;

[DataContract]
public class Pistol : BaseWeapon {
	public override string Name => "Pistol";
	public override int Power => 10;
	public override  int LifeTime => 1;
}
