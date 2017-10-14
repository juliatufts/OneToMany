using UnityEngine;
using System.Collections;
[System.Serializable]
public class CampReflectInt : CampReflector<int>
{
	public CampReflectInt(){
		types = new string[]{typeof(int).ToString(),typeof(System.Single).ToString(),typeof(System.Int32).ToString()};
	}
	public override void SetValue (object value){
		int j = 0;
		int.TryParse (value + "", out j);
		base.SetValue (j);
	}
}

