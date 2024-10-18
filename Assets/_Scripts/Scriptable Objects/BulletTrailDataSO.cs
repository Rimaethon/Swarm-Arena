using UnityEngine;

namespace Scriptable_Objects
{
	[CreateAssetMenu(fileName = "Bullet Trail Data", menuName = "Data/Bullet Trail Data")]
	public class BulletTrailDataSO : ScriptableObject
	{
		public Material Material;
		public AnimationCurve widthCurve;
		public float Duration = 0.5f;
		public float MinVertexDistance = 0.1f;
		public Gradient Color;
		public float SimulationSpeed = 100f;

		public TrailRenderer GetTrailRendererInstance()
		{
			GameObject trailPrefab = new GameObject("Bullet Trail");
			TrailRenderer trailRenderer = trailPrefab.AddComponent<TrailRenderer>();
			trailRenderer.material = Material;
			trailRenderer.widthCurve = widthCurve;
			trailRenderer.time = Duration;
			trailRenderer.minVertexDistance = MinVertexDistance;
			trailRenderer.colorGradient = Color;
			trailRenderer.autodestruct = false;
			return trailRenderer;
		}
	}
}
