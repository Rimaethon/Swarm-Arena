using UnityEngine;
using DG.Tweening;
using Managers;

[ExecuteAlways]
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset= new Vector3(0, 16, -16);
    [SerializeField] private float cameraShakeDuration=0.5f;
    private const float start_rotation = 40;
    private Transform player;

    private void Awake()
    {
        player= GameObject.FindGameObjectWithTag("Player").transform;
        transform.rotation = Quaternion.Euler(start_rotation, 0, 0);
    }

    private void OnEnable()
    {
        EventManager.RegisterHandler<OnImpact>(ImpactShake);
    }

    private void OnDisable()
    {
        EventManager.UnregisterHandler<OnImpact>(ImpactShake);
    }

    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;
    }

    private void ImpactShake(OnImpact impact)
    {
        transform.DOComplete();
        transform.DOShakePosition(cameraShakeDuration, impact.ImpactStrength/10);
        transform.DOShakeRotation(cameraShakeDuration, impact.ImpactStrength/10);
    }
}
