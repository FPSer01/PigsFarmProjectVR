using DG.Tweening;
using UnityEngine;

/// <summary>
/// Класс окна настроек UI руки
/// </summary>
public class HandUISettingsWindow : SettingsWindow
{
    [Header("Follow")]
    [SerializeField] private float distanceFromCamera;
    [SerializeField] private float angleToMove;
    [SerializeField] private float moveTime;

    private Transform cameraTransform;
    private bool windowMoving = false;

    protected override void Start()
    {
        base.Start();

        cameraTransform = Camera.main.transform;
    }

    private void OnValidate()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (IsAngleExcided() && !windowMoving)
        {
            SetWindownBeforeCamera(moveTime);
        }
    }

    /// <summary>
    /// Передвинуть окно перед камерой
    /// </summary>
    /// <param name="moveTime"></param>
    private void SetWindownBeforeCamera(float moveTime)
    {
        Vector3 planeForward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);

        Vector3 newPosition = cameraTransform.position + planeForward * distanceFromCamera;
        Quaternion newRotation = Quaternion.LookRotation(planeForward, Vector3.up);

        windowMoving = true;

        transform.DOMove(newPosition, moveTime).SetUpdate(true);
        transform.DORotateQuaternion(newRotation, moveTime)
            .OnComplete(() => windowMoving = false)
            .SetUpdate(true);
    }

    /// <summary>
    /// Проверка на превышение угла UI и камеры
    /// </summary>
    /// <returns></returns>
    private bool IsAngleExcided()
    {
        float angle = Vector3.Angle(transform.forward, cameraTransform.forward);

        return angle > angleToMove;
    }

    public override void Enable(bool enable, float fadeTime = 0)
    {
        base.Enable(enable, fadeTime);
        
        if (enable)
        {
            SetWindownBeforeCamera(0);
        }
    }
}
