using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс системы карманов
/// </summary>
public class PocketSystem : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField] private List<Transform> pockets;
    [SerializeField] private float heightRatio;

    private Vector3 currentHeadPos;
    private Quaternion currentHeadRot;

    private void Update()
    {
        currentHeadPos = head.position;
        currentHeadRot = head.rotation;

        foreach (var pocket in pockets)
        {
            UpdatePocketTransform(pocket);
        }

        transform.position = new Vector3(currentHeadPos.x, 0, currentHeadPos.z);
        transform.rotation = new Quaternion(transform.rotation.x, currentHeadRot.y, transform.rotation.z, currentHeadRot.w);
    }

    private void UpdatePocketTransform(Transform pocket)
    {
        pocket.transform.position = new Vector3(pocket.position.x, currentHeadPos.y * heightRatio, pocket.position.z);
    }
}
