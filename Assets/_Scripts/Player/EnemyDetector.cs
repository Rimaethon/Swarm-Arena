using System;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Player
{
	public class EnemyDetector
	{
		public int Size { get; private set; }
		public float radius;
		public IDamageAble[] damageAbles;
		private Collider[] results;
		private readonly LayerMask enemyLayer;
		private readonly Transform playerTransform;
		private const int max_size = 320;
		private int initialSize = 20;

		public EnemyDetector(Transform playerTransform, LayerMask enemyLayer)
		{
			this.playerTransform = playerTransform;
			this.enemyLayer = enemyLayer;
			damageAbles = new IDamageAble[initialSize];
			results = new Collider[initialSize];
		}

		public void GetDamageAblesInLargestRange()
		{
			Vector3 position = playerTransform.position;
			Array.Clear(results, 0, results.Length);
			Array.Clear(damageAbles, 0, damageAbles.Length);
			Size = Physics.OverlapSphereNonAlloc(position, radius / 2, results, enemyLayer);
			if (Size == 0)
				return;

			while (Size > initialSize && initialSize < max_size)
			{
				initialSize *= 2;
				damageAbles = new IDamageAble[initialSize];
				results = new Collider[initialSize];
				Size = Physics.OverlapSphereNonAlloc(position, radius, results, enemyLayer);
			}

			int index = 0;

			for (int i = 0; i < Size; i++)
			{
				if (results[i] == null)
					break;

				if (!results[i].TryGetComponent(out IDamageAble damageAble)) continue;
				damageAbles[index] = damageAble;
				index++;
			}
			Size = index;
			if (Size < 2)
				return;

			Array.Sort(damageAbles, 0, Size, new DamageAbleComparer(position));
		}

		private class DamageAbleComparer : IComparer<IDamageAble>
		{
			private readonly Vector3 position;

			public DamageAbleComparer(Vector3 position)
			{
				this.position = position;
			}

			public int Compare(IDamageAble a, IDamageAble b)
			{
				if (a == null) return 1;
				if (b == null) return -1;
				return Vector3.Distance(a.Position, position).CompareTo(Vector3.Distance(b.Position, position));
			}
		}
	}
}
