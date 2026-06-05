using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{

    [SerializeField] private float horizontalSpeed = 450f;
    [SerializeField] private float verticalSpeed = 450f;
    [SerializeField] private float clampAngle = 80f;

    private InputManager inputManager;
    private Vector3 startingRotation;

    protected override void Awake(){
        base.Awake();
    }

    void Start(){
        inputManager = InputManager.Instance;
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {

    if (vcam.Follow == null) return;

    if (stage != CinemachineCore.Stage.Aim) return;

    if (inputManager == null)

    {

        inputManager = InputManager.Instance;

        if (inputManager == null)

        {

            return;

        }

    }

    Vector2 deltaInput = inputManager.GetMouseDelta();

    startingRotation.x += deltaInput.x * horizontalSpeed * deltaTime;

    startingRotation.y += deltaInput.y * verticalSpeed * deltaTime;

    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);

    state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
    }
    
}
