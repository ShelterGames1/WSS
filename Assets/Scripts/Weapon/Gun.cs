using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour
{
    public Camera playerCamera;               // ������ ������ ��� ����������� ��������
    public GameObject muzzle;                  // ������ �������� �����
    public float damage = 10f;                // ���� �� ����
    public float range = 100f;                // ��������� �������� Raycast
    public float fireRate = 0.1f;             // �������� �������� � �������������� ������
    public bool isAutomatic = false;          // �����: ������������������ (false) ��� �������������� (true)
    public ParticleSystem muzzleFlash;        // ������ ������� ��������
    public GameObject impactEffect;           // ������ ����� ����
    public GameObject impactEffectBlood;      // ������ ����� ���� - �����
    public int maxAmmo = 30;                  // �������� �������� � ������
    public int currentAmmo;                   // ������� ������� � ������
    public float reloadTime = 1.5f;           // ����� �����������
    public AudioClip shootSound;              // ���� ��������
    public AudioClip reloadSound;             // ���� �����������
    public AudioSource audioSource;           // ����� �������� ��� ��������������� ������
    public float horizontalOffset = 0f;       // �������� Raycast �� �����������
    public float verticalOffset = 0f;         // �������� Raycast �� ���������
    public Animator animator;

    [HideInInspector] public bool isReloading = false;  // ��������, ���� �� �����������
    private float nextTimeToFire = 0f;                  // ��������� ����� ��� ��������

    public ProceduralRecoil recoil;

    void Start()
    {
        // ������������� ��������
        currentAmmo = maxAmmo;
    }

    void Update()
    {
        // ���� ���� �����������, ���������� ��������
        if (isReloading)
            return;

        // �����������, ���� ������� ��������� ��� ������ ������� R
        if (currentAmmo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            animator.SetBool("IsPricel", false);
            animator.SetTrigger("Reload");
            StartCoroutine(Reload());
            return;
        }

        // ������������ ������� �������� �� B
        if (Input.GetKeyDown(KeyCode.B))
        {
            isAutomatic = !isAutomatic;
        }

        // ������������������ �����: ������� ��� ������� ���
        if (!isAutomatic && Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        // �������������� �����: ��������� ��� ��� ��������
        if (isAutomatic && Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        recoil.Recoil();
        // �������� �� ������� ��������
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        currentAmmo--;

        // ������ �������
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }

        // ��������������� ����� ��������
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // ���������� ��������� ������� Raycast �� ������� �������� �����
        Vector3 rayOrigin = muzzle.transform.position
                            + muzzle.transform.right * horizontalOffset
                            + muzzle.transform.up * verticalOffset;
        Vector3 rayDirection = muzzle.transform.forward;

        // Raycast ��� �������� ���������
        RaycastHit hit;
        if (Physics.Raycast(rayOrigin, rayDirection, out hit, range))
        {
            Debug.Log("Hit " + hit.transform.name);

            // ����������� ����� �������� �� ����� ���������
            Debug.DrawLine(rayOrigin, hit.point, Color.red, 0.2f);

            // ��������� � ������ � ����������� "Health"
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(damage);
            }

            if (hit.collider.tag == "Untagged")
            {
                GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f); // �������� ������� ����� 2 �������
            }
            else if (hit.collider.tag == "Zombie")
            {
                GameObject impactGOBlood = Instantiate(impactEffectBlood, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGOBlood, 2f); // �������� ������� ����� 2 �������
            }
        }
        else
        {
            // ����������� ����� �������� �� ������������ ���������, ���� ��� ���������
            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * range, Color.red, 0.2f);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        // ��������������� ����� �����������
        if (audioSource != null && reloadSound != null)
        {
            audioSource.PlayOneShot(reloadSound);
        }

        Debug.Log("Reloading...");
        yield return new WaitForSeconds(reloadTime);

        // ��������������� ������� ����� �����������
        currentAmmo = maxAmmo;
        isReloading = false;
    }
}
