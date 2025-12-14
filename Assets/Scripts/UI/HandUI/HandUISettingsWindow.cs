using UnityEngine;

/// <summary>
/// Класс окна настроек UI руки
/// </summary>
public class HandUISettingsWindow : SettingsWindow
{
    [Header("Follow")]
    [SerializeField] private float distanceFromCamera;

    private Transform cameraTransform;

    protected override void Start()
    {
        base.Start();

        cameraTransform = Camera.main.transform;
    }

    private void OnValidate()
    {
        cameraTransform = Camera.main.transform;

        FollowTargetPoint();
    }

    private void Update()
    {
        FollowTargetPoint();
    }

    private void FollowTargetPoint()
    {
        Vector3 planeForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

        transform.position = cameraTransform.position + planeForward * distanceFromCamera;
        transform.rotation = Quaternion.LookRotation(planeForward, Vector3.up);
    }
}
