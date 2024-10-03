using Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Data", menuName = "Data/Weapon Data")]
public class WeaponDataSO : UpgradeAbleItemSO
{
    public AnimationClip reloadAnimation;
    public BulletTrailDataSO TrailRenderer;
    public GameObject weaponPrefab;
    public WeaponType weaponType;
}

public enum WeaponType
{
    HANDGUN,
    SKILL
}
