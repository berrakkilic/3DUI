using UnityEngine;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    [SerializeField] private DancematTranslater danceMat;

    public static InputManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private PlayerControls playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
        playerControls = new PlayerControls();
        Cursor.visible = false;

        if (danceMat == null)
        {
            danceMat = FindObjectOfType<DancematTranslater>();
        }
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        if (danceMat != null && danceMat.GetMatMovement() != Vector2.zero)
        {
            return danceMat.GetMatMovement();
        }
        return playerControls.player.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return GetCameraLookDelta();
    }

    public Vector2 GetCameraLookDelta()
    {
        if (danceMat != null && danceMat.GetMatLookMovement() != Vector2.zero)
        {
            return danceMat.GetMatLookMovement();
        }

        return playerControls.player.Look.ReadValue<Vector2>();
    }

    public bool PlayerJumpedThisFrame()
    {
        if (danceMat != null && danceMat.PlayerJumpedThisFrame())
        {
            return true;
        }
        return playerControls.player.Jump.triggered;
    }
}
