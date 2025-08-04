using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRecoil : MonoBehaviour
{
    // Позиции и вращения для камеры и оружия
    private Vector3 currentRotation, targetRotation;
    private Vector3 currentPosition, targetPosition, initialGunPosition;

    public Transform cam;

    // Параметры отдачи
    [SerializeField] private float recoilX = 2f;
    [SerializeField] private float recoilY = 2f;
    [SerializeField] private float recoilZ = 1f;

    // Параметры кикбэка (движения назад)
    [SerializeField] private float kickBackZ = 0.5f;

    // Скорость возвращения и сглаживания
    [SerializeField] private float snappiness = 10f;
    [SerializeField] private float returnAmount = 5f;

    // Множители для силы отдачи и кикбэка, чтобы их регулировать
    [SerializeField] private float recoilMultiplier = 1f;
    [SerializeField] private float kickBackMultiplier = 1f;

    private void Start()
    {
        // Инициализация начальной позиции оружия
        initialGunPosition = transform.localPosition;
    }

    private void Update()
    {
        // Плавное восстановление позиции и вращения оружия
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.deltaTime * snappiness);
        transform.localRotation = Quaternion.Euler(currentRotation);

        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * snappiness);
        transform.localPosition = currentPosition;

        // Синхронизация камеры
        cam.localRotation = Quaternion.Euler(currentRotation);
    }

    public void Recoil()
    {
        // Рассчитываем отдачу и кикбэк
        targetPosition -= new Vector3(0, 0, kickBackZ * kickBackMultiplier);
        targetRotation += new Vector3(recoilX * recoilMultiplier, Random.Range(-recoilY, recoilY) * recoilMultiplier, Random.Range(-recoilZ, recoilZ) * recoilMultiplier);
    }
}
