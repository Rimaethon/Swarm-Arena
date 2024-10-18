using System.Collections.Generic;
using Enums;
using Object_Pool;
using UnityEngine;

namespace Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Impact", menuName = "Impact System/Impact")]
	public class ImpactSO : ScriptableObject
	{
		[field: SerializeField]
		public ImpactType ImpactType { get; private set; }
		[field: SerializeField]
		public PoolAbleParticle EffectPrefab { get; private set; }
		[field: SerializeField]
		public List<AudioClip> SoundEffects { get; private set; }
	}
}
