using System;
using System.Collections.Generic;
using _Scripts.Player.Weapons;
using Data;
using Managers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float HorizontalSpeed => transform.InverseTransformDirection(moveDirection).normalized.x;
    public float VerticalSpeed => transform.InverseTransformDirection(moveDirection).normalized.z;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float aimRotationSpeed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] Vector3 colliderSize= new Vector3(5, 5, 5);

    public Vector3 moveDirection;
    private CharacterController characterController;

    [Header("Character Rig Settings")]
    [SerializeField] private Rig aimRig;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private float aimSmoothTime;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private WeaponDatabaseSO weaponDatabase;

    private float aimSmoothVelocity;
    private const float aim_rig_weight = 1f;
    private bool hasTarget;
    private RangeWeapon currentWeapon;
    private List<GameObject> sideWeapons = new List<GameObject>();
    private PlayerAnimationManager playerAnimationManager;
    private PlayerStatusManager playerStatusManager;
    private readonly Collider[] results = new Collider[20];
    PlayerData playerData;

    private void Awake()
    {
        playerAnimationManager = GetComponent<PlayerAnimationManager>();
        playerStatusManager = GetComponent<PlayerStatusManager>();
        characterController = GetComponent<CharacterController>();
        playerData = SaveManager.Instance.GetPlayerData();
        InitializeWeapons();
        currentWeapon.Construct(playerAnimationManager);
    }

    private void Update()
    {
        if (playerStatusManager.IsDead)
            return;
        HandleAimingMovement();
        aimRig.weight = Mathf.SmoothDamp(aimRig.weight, aim_rig_weight, ref aimSmoothVelocity, aimSmoothTime);
        if (hasTarget)
        {
            currentWeapon.PerformShot();
            return;
        }
        aimTarget.position = transform.position + transform.forward * 6+Vector3.up;
        CalculateMovementRotation();
    }

    private void InitializeWeapons()
    {
        foreach (WeaponDataSO weapon in weaponDatabase.items.Values)
        {
            if(!playerData.playerTalents[weapon.itemID].isUnlocked)
                continue;
            if (weapon.weaponType == WeaponType.HANDGUN)
            {
                currentWeapon=Instantiate(weapon.weaponPrefab, weaponHolder).GetComponent<RangeWeapon>();
                currentWeapon.InitializeWeapon(weapon);
            }
            else
            {
                GameObject skillWeapon = Instantiate(weapon.weaponPrefab, transform);
                skillWeapon.GetComponent<IWeapon>().InitializeWeapon(weapon);
                sideWeapons.Add(skillWeapon);
            }
        }
    }

    private void CalculateMovementRotation()
    {
        if (moveDirection == Vector3.zero) return;
        Quaternion movementRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, movementRotation, rotationSpeed * Time.deltaTime);
    }

    private void HandleAimingMovement()
    {
        Aim();
        AimMovement();
    }

    private void Aim()
    {
        Vector3 position = GetNearestEnemy(transform.position, 6f);
        if (hasTarget == false)
            return;
        aimTarget.position = position+ transform.forward * 6+Vector3.up;
        Vector3 direction = position - transform.position;
        direction.y = 0;
        Quaternion aimRotation = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, aimRotation, aimRotationSpeed * Time.deltaTime);
    }

    private void AimMovement()
    {
        characterController.Move(moveDirection * (movementSpeed* Time.deltaTime) + new Vector3(0, Physics.gravity.y / 2, 0));
    }

    private Vector3 GetNearestEnemy(Vector3 position, float radius)
    {
        Array.Clear(results, 0, results.Length);
        Physics.OverlapBoxNonAlloc(position, colliderSize,results, quaternion.identity, enemyLayer);
        Transform nearestEnemy=null;
        float nearestDistanceSqr = Mathf.Infinity;

        foreach (Collider collider in results)
        {
            if (collider == null)
                continue;
            if(collider.gameObject.TryGetComponent<IDamageAble>(out IDamageAble damageable) && damageable.IsDead)
                continue;
            float distanceSqr = (collider.transform.position - position).sqrMagnitude;
            if (!(distanceSqr < nearestDistanceSqr)) continue;
            nearestDistanceSqr = distanceSqr;
            nearestEnemy = collider.transform;
        }

        if(nearestEnemy == null)
        {
            hasTarget = false;
            return Vector3.zero;
        }

        hasTarget = true;
        return nearestEnemy.position;
    }

}
