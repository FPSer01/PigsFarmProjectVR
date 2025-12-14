using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Класс для контроля поведения свиньи в загоне
/// </summary>
public class PigBehaviour : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Movement")]
    // Интервал кулдауна передвижения мин и макс
    [SerializeField] private float minCooldownToMove;
    [SerializeField] private float maxCooldownToMove;
    [Space]
    // Интервал расстояния передвижения мин и макс
    [SerializeField] private float minDistanceToMove;
    [SerializeField] private float maxDistanceToMove;

    [Header("SFX")]
    // Искточник звука
    [SerializeField] private AudioSource sfxSource;
    // Звуки свиньи
    [SerializeField] private List<AudioClip> pigSFX;
    // Интервал задержки проигрывания звуков мин и макс
    [SerializeField] private float minSFXDelay;
    [SerializeField] private float maxSFXDelay;

    [Header("Components")]
    [SerializeField] private Animator animator;

    // Лежит ли свинья
    private bool laidDown;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        StartCoroutine(StartPlaySFXLoop());
    }

    private void Update()
    {
        UpdateWalkAnimationValue();
    }

    /// <summary>
    /// Задать тип поведения свиньи
    /// </summary>
    /// <param name="type"></param>
    public void SetBehaviourType(PigBehaviourType type)
    {
        switch (type)
        {
            case PigBehaviourType.Normal:
                StartCoroutine(MovementLoop());
                break;

            case PigBehaviourType.LaidDown:
                laidDown = true;
                animator.SetTrigger("LayDown");
                SetRandomRotationY();
                break;
        }
    }

    /// <summary>
    /// Корутин цикла счета таймингов движения свиньи
    /// </summary>
    /// <returns></returns>
    private IEnumerator MovementLoop()
    {
        while (true)
        {
            float timeToMove = Random.Range(minCooldownToMove, maxCooldownToMove);

            Move();

            // Ждем время до следующего движения
            yield return new WaitForSeconds(timeToMove);
        }
    }

    /// <summary>
    /// Начать движение свиньи в рандомном направлении (точки)
    /// </summary>
    private void Move()
    {
        float distance = Random.Range(minDistanceToMove, maxDistanceToMove);

        // Создаем рандомное направление по плоскости XZ
        Vector3 rngDirection = new Vector3(
            Random.Range(-1f, 1f),
            0f,
            Random.Range(-1f, 1f)
            );

        Vector3 point = transform.position + rngDirection.normalized * distance;

        agent.SetDestination(point);
    }

    /// <summary>
    /// Обновить значения отвечающие за переключение с состояния Idle на Walk и наоборот
    /// </summary>
    /// <remarks>Обновляет NormalizedVelocity в аниматоре для контроля Idle and Walk состояния</remarks>
    private void UpdateWalkAnimationValue()
    {
        if (laidDown)
            return;

        float maxVelocity = agent.speed;
        float currentVelocity = agent.velocity.magnitude;

        float normalizedVelocity = currentVelocity / maxVelocity;

        animator.SetFloat("NormalizedVelocity", normalizedVelocity);
    }

    /// <summary>
    /// Запустить цикл проигрывания звуков свиней
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartPlaySFXLoop()
    {
        while (true)
        {
            // Делаем рандомное время ожидания для проигрывания звуков в разнобой
            yield return new WaitForSeconds(Random.Range(minSFXDelay, maxSFXDelay));

            AudioClip clip = pigSFX[Random.Range(0, pigSFX.Count)];

            sfxSource.PlayOneShot(clip);

            // Ждем проигрывания звука, чтобы не перекрывать следующий
            yield return new WaitForSecondsRealtime(clip.length);
        }
    }

    /// <summary>
    /// Задать свинье рандомное значение поворота по Y
    /// </summary>
    private void SetRandomRotationY()
    {
        float angle = Random.Range(0f, 360f);

        Vector3 angles = transform.eulerAngles;
        angles.y = angle;

        transform.eulerAngles = angles;
    }
}
