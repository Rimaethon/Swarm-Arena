using UnityEngine;

[CreateAssetMenu(fileName = "Bullet Trail Data", menuName = "Data/Bullet Trail Data")]
public class BulletTrailDataSO : ScriptableObject
{
    public Material Material;
    public AnimationCurve widthCurve;
    public float Duration = 0.5f;
    public float MinVertexDistance = 0.1f;
    public Gradient Color;

    public float MissDistance = 100f;
    public float SimulationSpeed = 100f;
}
