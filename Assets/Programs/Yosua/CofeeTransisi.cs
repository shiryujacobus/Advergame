
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CoffeeBeanTransition : MonoBehaviour
{
    public static CoffeeBeanTransition Instance;

    [Header("UI References")]
    public Canvas transitionCanvas;
    public RectTransform beanContainer;
    public Image beanPrefab;

    [Header("Bean Appearance")]
    public Vector2 beanSize = new Vector2(220, 260);

    [Range(0.2f, 0.9f)]
    public float spacingX = 0.28f;

    [Range(0.2f, 0.9f)]
    public float spacingY = 0.28f;

    [Header("Falling Animation")]
    public Vector2 fallDuration = new Vector2(1.2f, 2f);
    public Vector2 fallSpeed = new Vector2(900, 1400);
    public Vector2 startHeight = new Vector2(400, 1200);

    public float spawnDelay = 0.6f;
    public float revealDelay = 0.3f;

    private class BeanData
    {
        public RectTransform rect;

        public Vector2 startPosition;
        public Vector2 targetPosition;

        public Quaternion startRotation;
        public Quaternion targetRotation;

        public float delay;
        public float duration;
        public float timer;
        public float speed;

        public bool settled;
    }

    private readonly List<BeanData> beans = new List<BeanData>();

    private bool isTransitioning;
    private bool isRevealing;

    private float screenHeight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (transitionCanvas != null)
        {
            transitionCanvas.overrideSorting = true;
            transitionCanvas.sortingOrder = 1000;
        }
    }

    private void Update()
    {
        if (isTransitioning && !isRevealing)
        {
            UpdateCoverAnimation();
        }
        else if (isRevealing)
        {
            UpdateRevealAnimation();
        }
    }

    // DIPANGGIL UNTUK PINDAH SCENE
    public void LoadScene(string sceneName)
    {
        if (isTransitioning)
            return;

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError("Scene tidak ditemukan: " + sceneName);
            return;
        }

        StartCoroutine(TransitionRoutine(sceneName));
    }

    private IEnumerator TransitionRoutine(string sceneName)
    {
        isTransitioning = true;
        isRevealing = false;

        if (transitionCanvas != null)
            transitionCanvas.enabled = true;

        yield return null;

        Canvas.ForceUpdateCanvases();

        if (beanPrefab == null || beanContainer == null)
        {
            Debug.LogError("Bean Prefab atau Bean Container belum diisi!");
            isTransitioning = false;
            yield break;
        }

        if (beanPrefab.sprite == null)
        {
            Debug.LogError("Bean Prefab belum memiliki Sprite!");
            isTransitioning = false;
            yield break;
        }

        beans.Clear();

        screenHeight = beanContainer.rect.height;

        if (beanContainer.rect.width <= 0 || screenHeight <= 0)
        {
            Debug.LogError("Ukuran BeanContainer tidak valid!");
            isTransitioning = false;
            yield break;
        }

        SpawnBeans();

        // Tunggu semua biji menutupi layar
        yield return new WaitUntil(AllBeansSettled);

        Debug.Log("Layar berhasil tertutup biji kopi!");

        // Load scene baru
        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        if (operation == null)
        {
            Debug.LogError("Gagal memuat scene: " + sceneName);
            isTransitioning = false;
            yield break;
        }

        while (!operation.isDone)
            yield return null;

        yield return new WaitForSeconds(revealDelay);

        // Mulai membuka layar
        isRevealing = true;
    }

    private void SpawnBeans()
    {
        float width = beanContainer.rect.width;
        float height = beanContainer.rect.height;

        // Hitung rasio asli sprite
        float aspect =
            beanPrefab.sprite.rect.width /
            beanPrefab.sprite.rect.height;

        float gapX = beanSize.x * spacingX;
        float gapY = beanSize.y * spacingY;

        int columns = Mathf.CeilToInt(width / gapX) + 4;
        int rows = Mathf.CeilToInt(height / gapY) + 4;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Image newBean = Instantiate(beanPrefab, beanContainer);

                RectTransform rect = newBean.rectTransform;

                // Pertahankan bentuk asli gambar
                newBean.preserveAspect = true;

                float scale = Random.Range(0.75f, 1.1f);

                float beanHeight = beanSize.y * scale;
                float beanWidth = beanHeight * aspect;

                rect.sizeDelta = new Vector2(
                    beanWidth,
                    beanHeight
                );

                // Posisi target dengan sedikit variasi
                float targetX =
                    -width / 2f - gapX +
                    col * gapX;

                float targetY =
                    height / 2f + gapY -
                    row * gapY;

                Vector2 targetPosition = new Vector2(
                    targetX + Random.Range(-gapX * 0.12f, gapX * 0.12f),
                    targetY + Random.Range(-gapY * 0.12f, gapY * 0.12f)
                );

                // Posisi awal di atas layar
                Vector2 startPosition = new Vector2(
                    targetPosition.x,
                    targetPosition.y +
                    height +
                    Random.Range(startHeight.x, startHeight.y)
                );

                Quaternion startRotation = Quaternion.Euler(
                    0, 0, Random.Range(0f, 360f)
                );

                Quaternion targetRotation = Quaternion.Euler(
                    0, 0, Random.Range(-180f, 180f)
                );

                rect.anchoredPosition = startPosition;
                rect.localRotation = startRotation;

                BeanData data = new BeanData();

                data.rect = rect;

                data.startPosition = startPosition;
                data.targetPosition = targetPosition;

                data.startRotation = startRotation;
                data.targetRotation = targetRotation;

                data.delay = Random.Range(0f, spawnDelay);

                data.duration = Random.Range(
                    fallDuration.x,
                    fallDuration.y
                );

                data.speed = Random.Range(
                    fallSpeed.x,
                    fallSpeed.y
                );

                beans.Add(data);
            }
        }
    }

    private void UpdateCoverAnimation()
    {
        foreach (BeanData bean in beans)
        {
            if (bean.settled || bean.rect == null)
                continue;

            bean.timer += Time.deltaTime;

            if (bean.timer < bean.delay)
                continue;

            float t = Mathf.Clamp01(
                (bean.timer - bean.delay) / bean.duration
            );

            // Gerakan jatuh lebih halus
            float smoothT = Mathf.SmoothStep(0f, 1f, t);

            bean.rect.anchoredPosition = Vector2.Lerp(
                bean.startPosition,
                bean.targetPosition,
                smoothT
            );

            bean.rect.localRotation = Quaternion.Slerp(
                bean.startRotation,
                bean.targetRotation,
                smoothT
            );

            if (t >= 1f)
            {
                bean.rect.anchoredPosition = bean.targetPosition;
                bean.rect.localRotation = bean.targetRotation;

                bean.settled = true;
            }
        }
    }

    private bool AllBeansSettled()
    {
        foreach (BeanData bean in beans)
        {
            if (!bean.settled)
                return false;
        }

        return beans.Count > 0;
    }

    private void UpdateRevealAnimation()
    {
        for (int i = beans.Count - 1; i >= 0; i--)
        {
            BeanData bean = beans[i];

            if (bean.rect == null)
            {
                beans.RemoveAt(i);
                continue;
            }

            // Semua biji bergerak turun
            bean.rect.anchoredPosition +=
                Vector2.down * bean.speed * Time.deltaTime;

            // Rotasi sedikit saat turun
            bean.rect.Rotate(
                0f,
                0f,
                45f * Time.deltaTime
            );

            // Hapus jika sudah keluar layar
            if (bean.rect.anchoredPosition.y <
                -screenHeight / 2f - bean.rect.rect.height * 2f)
            {
                Destroy(bean.rect.gameObject);
                beans.RemoveAt(i);
            }
        }

        // Transisi selesai
        if (beans.Count == 0)
        {
            isRevealing = false;
            isTransitioning = false;

            if (transitionCanvas != null)
                transitionCanvas.enabled = false;

            Debug.Log("Transisi scene selesai!");
        }
    }
}
