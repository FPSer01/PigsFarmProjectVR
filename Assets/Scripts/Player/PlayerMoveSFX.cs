using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

/// <summary>
/// Класс-контроллер для звука передвижения
/// </summary>
public class PlayerMoveSFX : MonoBehaviour
{
    // Контроллер ходьбы в этом случае
    [SerializeField] private DynamicMoveProvider controller;
    // Минимум значения ввода стика левого контроллера при котором будет воспроизводиться звук ходьбы
    [SerializeField] private float moveInputThreshold;
    [Space]
    [SerializeField] private AudioSource sfxSource;
    // Время плавного переключения звука ходьбы
    [SerializeField] private float fadeTime;
    private float originalSFXVolume;

    private bool sfxEnabled;

    private void Start()
    {
        originalSFXVolume = sfxSource.volume;
        sfxSource.volume = 0f;
        sfxSource.Stop();
    }

    private void Update()
    {
        if (IsMoving())
        {
            EnableMovementSFX(true);
        }
        else
        {
            EnableMovementSFX(false);
        }
    }

    /// <summary>
    /// Двигается ли сейчас пользователь
    /// </summary>
    /// <returns></returns>
    private bool IsMoving()
    {
        Vector2 leftInput = controller.leftHandMoveInput.ReadValue();
        Vector2 rightInput = controller.rightHandMoveInput.ReadValue();

        if (leftInput.sqrMagnitude > moveInputThreshold || rightInput.sqrMagnitude > moveInputThreshold)
        {
            return true;
        }

        return false;
    }
    
    // <summary>
    /// Включить или выключить звук ходьбы
    /// </summary>
    /// <param name="enable"></param>
    private void EnableMovementSFX(bool enable)
    {
        // Не меняем значение, если оно одно и то же
        if (sfxEnabled == enable)
            return;

        sfxEnabled = enable;

        if (enable)
        {
            sfxSource.Play();
            sfxSource.DOFade(originalSFXVolume, fadeTime).SetUpdate(true);
        }
        else
        {
            sfxSource.DOFade(0, fadeTime).SetUpdate(true).OnComplete(() =>
            {
                sfxSource.Pause();
            });
        }
    }
}
