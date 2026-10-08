using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomerRandomizer : MonoBehaviour
{
    public GameObject[] customers;

    public int maxActiveCustomers = 4;
    public float spawnDelay = 1f;

    public Transform[] buyPoints;

    public Transform startLeft;
    public Transform startRight;

    public Transform exitLeft;
    public Transform exitRight;

    private List<GameObject> availableCustomers =
        new List<GameObject>();

    private List<Transform> availableBuyPoints =
        new List<Transform>();

    void Start()
    {
        foreach (GameObject customer in customers)
        {
            if (customer != null)
                customer.SetActive(false);
        }

        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            int activeCount = 0;

            foreach (GameObject customer in customers)
            {
                if (customer != null && customer.activeSelf)
                    activeCount++;
            }

            // Maksimal 4 customer aktif
            if (activeCount >= maxActiveCustomers)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // Cari customer yang tersedia
            availableCustomers.Clear();

            foreach (GameObject customer in customers)
            {
                if (customer != null && !customer.activeSelf)
                    availableCustomers.Add(customer);
            }

            if (availableCustomers.Count == 0)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // Cari Buy Point yang belum dipakai
            availableBuyPoints.Clear();

            foreach (Transform buyPoint in buyPoints)
            {
                if (buyPoint == null)
                    continue;

                bool used = false;

                foreach (GameObject activeCustomer in customers)
                {
                    if (activeCustomer == null ||
                        !activeCustomer.activeSelf)
                        continue;

                    Transform activeBuyPoint =
                        GetCustomerBuyPoint(activeCustomer);

                    if (activeBuyPoint == buyPoint)
                    {
                        used = true;
                        break;
                    }
                }

                if (!used)
                    availableBuyPoints.Add(buyPoint);
            }

            if (availableBuyPoints.Count == 0)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            // Random customer
            GameObject currentCustomer =
                availableCustomers[
                    Random.Range(
                        0,
                        availableCustomers.Count
                    )
                ];

            // Random Buy Point
            Transform selectedBuyPoint =
                availableBuyPoints[
                    Random.Range(
                        0,
                        availableBuyPoints.Count
                    )
                ];

            // Random sisi masuk
            bool spawnFromLeft =
                Random.Range(0, 2) == 0;

            Transform spawnPoint =
                spawnFromLeft
                ? startLeft
                : startRight;

            if (spawnPoint == null)
            {
                Debug.LogError(
                    "Start Point belum diisi!"
                );

                yield return new WaitForSeconds(1f);
                continue;
            }

            // Pindahkan ke titik spawn
            currentCustomer.transform.position =
                spawnPoint.position;

            // Berikan Buy Point random
            SetCustomerBuyPoint(
                currentCustomer,
                selectedBuyPoint
            );

            // Berikan Exit
            SetCustomerExit(
                currentCustomer,
                spawnFromLeft
            );

            Debug.Log(
                currentCustomer.name +
                " → " +
                selectedBuyPoint.name
            );

            // Aktifkan customer
            currentCustomer.SetActive(true);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    Transform GetCustomerBuyPoint(GameObject customer)
    {
        CustomerMovement1 c1 =
            customer.GetComponent<CustomerMovement1>();

        if (c1 != null)
            return c1.buyPoint;

        CustomerMovement2 c2 =
            customer.GetComponent<CustomerMovement2>();

        if (c2 != null)
            return c2.buyPoint;

        CustomerMovement3 c3 =
            customer.GetComponent<CustomerMovement3>();

        if (c3 != null)
            return c3.buyPoint;

        CustomerMovement4 c4 =
            customer.GetComponent<CustomerMovement4>();

        if (c4 != null)
            return c4.buyPoint;

        return null;
    }

    void SetCustomerBuyPoint(
        GameObject customer,
        Transform buyPoint
    )
    {
        CustomerMovement1 c1 =
            customer.GetComponent<CustomerMovement1>();

        if (c1 != null)
        {
            c1.buyPoint = buyPoint;
            return;
        }

        CustomerMovement2 c2 =
            customer.GetComponent<CustomerMovement2>();

        if (c2 != null)
        {
            c2.buyPoint = buyPoint;
            return;
        }

        CustomerMovement3 c3 =
            customer.GetComponent<CustomerMovement3>();

        if (c3 != null)
        {
            c3.buyPoint = buyPoint;
            return;
        }

        CustomerMovement4 c4 =
            customer.GetComponent<CustomerMovement4>();

        if (c4 != null)
        {
            c4.buyPoint = buyPoint;
            return;
        }
    }

    void SetCustomerExit(
        GameObject customer,
        bool spawnFromLeft
    )
    {
        Transform exitPoint =
            spawnFromLeft
            ? exitRight
            : exitLeft;

        CustomerMovement1 c1 =
            customer.GetComponent<CustomerMovement1>();

        if (c1 != null)
        {
            c1.exitPoint = exitPoint;
            return;
        }

        CustomerMovement2 c2 =
            customer.GetComponent<CustomerMovement2>();

        if (c2 != null)
        {
            c2.exitPoint = exitPoint;
            return;
        }

        CustomerMovement3 c3 =
            customer.GetComponent<CustomerMovement3>();

        if (c3 != null)
        {
            c3.exitPoint = exitPoint;
            return;
        }

        CustomerMovement4 c4 =
            customer.GetComponent<CustomerMovement4>();

        if (c4 != null)
        {
            c4.exitPoint = exitPoint;
            return;
        }
    }
}