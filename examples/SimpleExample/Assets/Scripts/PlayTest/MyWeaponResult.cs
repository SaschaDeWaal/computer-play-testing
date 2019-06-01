using System.Runtime.Serialization;
using ComputerPlayTesting;

[DataContract]
public class MyWeaponResult : PlayTestResult {

	[DataMember] private string weaponName;
	[DataMember] public int BattledTimes = 0;
	[DataMember] public int WonTimes = 0;

	[DataMember] public override string ResultType {
		get => "WeaponResult";
		set { }
	}


	public MyWeaponResult(string name, int battled, int won) {
		weaponName = name;
		BattledTimes = battled;
		WonTimes = won;
	}
}
