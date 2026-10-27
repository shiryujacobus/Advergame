
using UnityEngine;

public class CoffeeItem : MonoBehaviour
{
    [Header("Status Kopi")]
    public bool coffeeReady = false;

    [Header("Visual Mug")]
    public Sprite emptyMugSprite;
    public Sprite coffeeMugSprite;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Awal game, mug dalam keadaan kosong.
        SetCoffeeEmpty();
    }

    // Dipanggil oleh mesin atau tombol saat kopi selesai dibuat.
    public void SetCoffeeReady()
    {
        coffeeReady = true;

        if (spriteRenderer != null && coffeeMugSprite != null)
            spriteRenderer.sprite = coffeeMugSprite;

        Debug.Log(name + ": Kopi siap diambil!");
    }

    // Mug kembali kosong setelah kopi berhasil diambil.
    public void SetCoffeeEmpty()
    {
        coffeeReady = false;

        if (spriteRenderer != null && emptyMugSprite != null)
            spriteRenderer.sprite = emptyMugSprite;

        Debug.Log(name + ": Mug kosong, siap di-refill.");
    }


    private void OnMouseDown()
    {
        if (!coffeeReady)
        {
            Debug.Log("Kopi belum siap!");
            return;
        }

        GameManager manager = FindFirstObjectByType<GameManager>();

        if (manager != null)
            manager.TryTakeCoffee(this);
    }

}
