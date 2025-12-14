using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Класс объекта калитки
/// </summary>
public class Fence : XRInteractableHighlighter
{
    private Rigidbody rb;
    private XRGrabInteractable grabInteractable;

    [Header("SFX")]
    [SerializeField] private AudioSource sfxSource;
    [Space]
    [SerializeField] private AudioClip openSFX;
    [SerializeField] private AudioClip closeSFX;
    [SerializeField] private float angleThreshold;
    [SerializeField] private float velocityThreshold;
    [SerializeField] private float cooldownSFX = 0.25f;

    private Vector3 originalAngle;
    private bool canPlaySFX = true;
    private bool opened;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    protected override void Start()
    {
        base.Start();

        rb.isKinematic = true;

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        originalAngle = transform.eulerAngles;
    }

    private void OnReleased(SelectExitEventArgs arg0)
    {
        rb.isKinematic = true;
    }

    private void OnGrabbed(SelectEnterEventArgs arg0)
    {
        rb.isKinematic = false;
    }

    private void Update()
    {
        float angleDiff = Mathf.Abs(originalAngle.y - transform.eulerAngles.y);
        float velocity = rb.angularVelocity.y;

        if (angleDiff > angleThreshold && velocity > velocityThreshold && !opened)
        {
            PlayOpenSFX(true);
        }
        else if (angleDiff < angleThreshold && velocity < -velocityThreshold && opened)
        {
            PlayOpenSFX(false);
        }

        //Debug.Log($"a:{angleDiff}, y:{velocity}");
    }

    private void PlayOpenSFX(bool open)
    {
        if (opened == open || !canPlaySFX)
            return;

        opened = open;
        sfxSource.Stop();

        if (open)
        {       
            sfxSource.PlayOneShot(openSFX);
        }
        else
        {
            sfxSource.PlayOneShot(closeSFX);
        }

        canPlaySFX = false;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldownSFX);

        canPlaySFX = true;
    }
}
