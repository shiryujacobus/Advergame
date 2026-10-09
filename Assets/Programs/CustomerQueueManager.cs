
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueueManager : MonoBehaviour
{
    public static CustomerQueueManager Instance;

    [Header("6 Customers")]
    public MonoBehaviour[] customers = new MonoBehaviour[6];

    [Header("Spawn Points")]
    public Transform customerStartLeft;
    public Transform customerStartRight;

    [Header("4 Service Points")]
    public Transform[] buyPoints = new Transform[4];

    [Header("Waiting Points")]
    public Transform[] leftWaitPoints = new Transform[2];
    public Transform[] rightWaitPoints = new Transform[2];

    [Header("Spawn Settings")]
    public float spawnDelay = 2f;

    private readonly Dictionary<MonoBehaviour, int> slots = new();
    private readonly Dictionary<MonoBehaviour, bool> sides = new();
    private readonly List<MonoBehaviour> waiting = new();

    private bool initialized;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetupCustomers();
    }


    private void SetupCustomers()
    {
        StartCoroutine(SetupCustomersGradually());
    }

    private System.Collections.IEnumerator SetupCustomersGradually()
    {
        initialized = false;
        slots.Clear();
        sides.Clear();
        waiting.Clear();

        if (customerStartLeft == null ||
            customerStartRight == null ||
            buyPoints == null || buyPoints.Length < 4)
        {
            Debug.LogError("Isi Spawn Points dan 4 Buy Points.");
            yield break;
        }

        List<MonoBehaviour> list = new();

        foreach (MonoBehaviour c in customers)
        {
            if (c != null && !list.Contains(c))
                list.Add(c);
        }

        if (list.Count == 0)
        {
            Debug.LogError("Daftar customer kosong.");
            yield break;
        }

        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        // Daftarkan seluruh customer terlebih dahulu,
        // tetapi jangan aktifkan semuanya sekaligus.
        foreach (MonoBehaviour c in list)
        {
            if (c.gameObject.activeSelf)
                c.gameObject.SetActive(false);
        }

        for (int i = 0; i < list.Count; i++)
        {
            MonoBehaviour c = list[i];
            bool fromRight = Random.Range(0, 2) == 1;

            sides[c] = fromRight;
            waiting.Add(c);

            Transform start = fromRight
                ? customerStartRight
                : customerStartLeft;

            Vector3 pos = start.position;
            pos.y -= (i % 3) * 0.35f;
            c.transform.position = pos;
        }

        initialized = true;
        Debug.Log($"Customer terdaftar: {waiting.Count} dari {list.Count}.");

        // Spawn satu per satu.
        for (int i = 0; i < list.Count; i++)
        {
            MonoBehaviour c = list[i];

            if (c == null)
                continue;

            c.gameObject.SetActive(true);

            Debug.Log($"Spawn customer: {c.name}");

            if (i < list.Count - 1)
                yield return new WaitForSeconds(spawnDelay);
        }
    }

    private bool GetSide(MonoBehaviour c)
    {
        return c != null &&
               sides.TryGetValue(c, out bool right) && right;
    }

    public bool IsSpawnFromRight(float spawnX)
    {
        if (customerStartLeft == null || customerStartRight == null)
            return false;

        float middle =
            (customerStartLeft.position.x +
             customerStartRight.position.x) / 2f;

        return spawnX > middle;
    }

    public bool RequestEntry(MonoBehaviour c)
    {
        return RequestEntry(c, out _);
    }

    public bool RequestEntry(MonoBehaviour c, out Transform buyPoint)
    {
        return RequestEntry(c, GetSide(c), out buyPoint);
    }

    public bool RequestEntry(
        MonoBehaviour c,
        bool spawnFromRight,
        out Transform buyPoint)
    {
        buyPoint = null;

        if (!initialized || c == null)
            return false;

        if (slots.TryGetValue(c, out int existing))
        {
            if (existing >= 0 && existing < buyPoints.Length)
            {
                buyPoint = buyPoints[existing];
                return buyPoint != null;
            }

            return false;
        }

        if (!waiting.Contains(c))
            return false;

        for (int i = 0; i < 4; i++)
        {
            if (buyPoints[i] == null || slots.ContainsValue(i))
                continue;

            slots[c] = i;
            waiting.Remove(c);
            buyPoint = buyPoints[i];

            Debug.Log($"{c.name} mendapat Buy Point {i + 1}");
            return true;
        }

        return false;
    }

    public Transform GetWaitPoint(MonoBehaviour c)
    {
        if (!initialized || c == null)
            return null;

        int index = waiting.IndexOf(c);
        if (index < 0)
            return null;

        bool right = GetSide(c);
        Transform[] points = right ? rightWaitPoints : leftWaitPoints;

        int sideIndex = 0;

        for (int i = 0; i < index; i++)
        {
            if (waiting[i] != null && GetSide(waiting[i]) == right)
                sideIndex++;
        }

        if (points == null || points.Length == 0)
            return right ? customerStartRight : customerStartLeft;

        return points[Mathf.Min(sideIndex, points.Length - 1)];
    }

    public void ReleaseSlot(MonoBehaviour c)
    {
        if (c == null)
            return;

        if (slots.Remove(c))
            Debug.Log($"{c.name} melepas Buy Point.");
    }

    public void RemoveCustomer(MonoBehaviour c)
    {
        if (c == null)
            return;

        ReleaseSlot(c);
        waiting.Remove(c);
        sides.Remove(c);
    }
}