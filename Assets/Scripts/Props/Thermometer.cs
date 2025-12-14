using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

/// <summary>
/// Класс термометра
/// </summary>
public class Thermometer : MonoBehaviour
{
    [Header("General")]
    // Точка из которой идет луч проверки температуры
    [SerializeField] private Transform laserPoint;
    // Максимальная дистанция, на которую будет проходить проверка
    [SerializeField] private float maxLaserDistance;
    // Радиус луча лазера
    [SerializeField] private float laserRadius;
    // Слои, на которые будет реагировать лазер
    [SerializeField] private LayerMask laserHitMask;
    [Space]
    [SerializeField] private float cooldownTime;

    [Header("UI")]
    [SerializeField] private TMP_Text valueText;
    // Время очищения циферблата, после того как он выводит что-либо
    [SerializeField] private float timeToClearText;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;

    private IThermometerInteractable currentInteractable;
    private XRGrabInteractable grabInteractable;

    // Корутин очистки текста циферблата
    private IEnumerator scheduleClearTextCoroutine;

    private bool canMeasureTemperature = true;
    private bool grabbed = false;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        grabInteractable.activated.AddListener(OnActivated);

        ScheduleClearValueText(true);
    }

    private void FixedUpdate()
    {
        if (!grabbed)
            return;

        CheckForInteractable();
    }

    #region XRGrabInteractable Events

    // Когда отпустили
    private void OnReleased(SelectExitEventArgs arg0)
    {
        grabbed = false;
        ClearCurrentInteractable();
    }

    // Когда взяли
    private void OnGrabbed(SelectEnterEventArgs arg0)
    {
        grabbed = true;
    }

    // При нажатии триггера во время держания термометра
    private void OnActivated(ActivateEventArgs arg0)
    {
        if (!canMeasureTemperature)
            return;

        CheckTemperature();

        // Запускаем кулдаун
        canMeasureTemperature = false;
        StartCoroutine(Cooldown());
    }

    #endregion

    /// <summary>
    /// Задать значение температуры на циферблате термометра
    /// </summary>
    /// <param name="temperature">Значение температуры</param>
    private void SetTemperatureValueText(float temperature)
    {
        temperature = MathF.Round(temperature, 1);

        valueText.text = $"{temperature} C";

        ScheduleClearValueText();
    }

    /// <summary>
    /// Вывести на циферблат сообщение
    /// </summary>
    private void SetMessage(string message)
    {
        valueText.text = message;

        ScheduleClearValueText();
    }

    /// <summary>
    /// Очистить текст циферблата через какое-то время
    /// </summary>
    /// <remarks>Заменяет прошлую задержку очистки</remarks>
    /// <param name="clearImmediate">Очистить сразу?</param>
    private void ScheduleClearValueText(bool clearImmediate = false)
    {
        if (clearImmediate)
        {
            valueText.text = "";
        }
        else
        {
            if (scheduleClearTextCoroutine != null)
            {
                StopCoroutine(scheduleClearTextCoroutine);
                scheduleClearTextCoroutine = null;
            }

            scheduleClearTextCoroutine = ValueTextClear();
            StartCoroutine(scheduleClearTextCoroutine);
        }
    }

    /// <summary>
    /// Корутин очистки циферблата через определенное время
    /// </summary>
    /// <returns></returns>
    private IEnumerator ValueTextClear()
    {
        yield return new WaitForSeconds(timeToClearText);

        valueText.text = "";
    }

    private void CheckForInteractable()
    {
        // Пускаем рейкаст в форме сферы
        if (Physics.SphereCast(
            laserPoint.position,
            laserRadius,
            laserPoint.forward,
            out RaycastHit hit,
            maxLaserDistance,
            laserHitMask))
        {
            // Объект, который лазер задел
            Collider hitCollider = hit.collider;

            // Если у объекта есть нужный интерфейс
            if (hitCollider.TryGetComponent(out IThermometerInteractable interactable))
            {
                // Запоминаем этот объект
                currentInteractable = interactable;
                // Подсвечиваем объект
                currentInteractable.HighlightInteractable(true);
            }
            else
            {
                ClearCurrentInteractable();
            }
        }
        else
        {
            ClearCurrentInteractable();
        }
    }

    private void CheckTemperature()
    {
        if (currentInteractable != null)
        {
            // Получаем температуру объекта
            float temperature = currentInteractable.GetTemperature();

            // Выводим температуру на циферблате термометра
            SetTemperatureValueText(temperature);
            sfxSource.Play();
        }
        else
        {
            SetMessage("N/A");
        }      
    }

    /// <summary>
    /// Корутин кулдауна измерения температуры
    /// </summary>
    /// <returns></returns>
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        canMeasureTemperature = true;
    }

    /// <summary>
    /// Очистить нынешний объект взаимодействия
    /// </summary>
    private void ClearCurrentInteractable()
    {
        // Если currentInteractable не нулевой (null), то отключаем подсветку
        currentInteractable?.HighlightInteractable(false);
        // Очищаем переменную
        currentInteractable = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(laserPoint.position, laserRadius);
        Gizmos.DrawRay(laserPoint.position, laserPoint.forward * maxLaserDistance);
        Gizmos.DrawWireSphere(laserPoint.position + laserPoint.forward * maxLaserDistance, laserRadius);
    }
}
