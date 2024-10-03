using System.Collections.Generic;
using Data;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
[ CreateAssetMenu(fileName = "WeaponDatabaseSO", menuName = "Data/WeaponDatabaseSO")]
public class WeaponDatabaseSO : SerializedScriptableObject
{
	public SerializedDictionary<int,WeaponDataSO> items=new SerializedDictionary<int, WeaponDataSO>();

	[Button("Set Item ID's and Scriptable objects to prefabs")]
	public void Initialize()
	{
		foreach (KeyValuePair<int, WeaponDataSO> weapon in items)
		{
			weapon.Value.itemID = weapon.Key;
		}
	}
}
