using Managers;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] Joystick _joystick;
    private PlayerController playerController;
    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        EventManager.RegisterHandler<OnPlayerDeath>(HandlePlayerDeath);
        EventManager.RegisterHandler<OnLevelCompleted>(HandleLevelCompleted);
    }

    private void OnDisable()
    {
        EventManager.UnregisterHandler<OnPlayerDeath>(HandlePlayerDeath);
        EventManager.UnregisterHandler<OnLevelCompleted>(HandleLevelCompleted);
    }

    private void HandlePlayerDeath(OnPlayerDeath data)
    {
        _joystick.gameObject.SetActive(false);
    }
    private void HandleLevelCompleted(OnLevelCompleted data)
    {
        _joystick.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        playerController.moveDirection = new Vector3(_joystick.Direction.x, 0, _joystick.Direction.y);
    }
}
