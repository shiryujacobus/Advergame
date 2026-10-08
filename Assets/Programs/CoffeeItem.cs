using UnityEngine;

public class CoffeeItem : MonoBehaviour
{
    public bool coffeeReady = false;

    [Header("Mug Visual")]
    public Sprite emptyMugSprite;
    public Sprite coffeeMugSprite;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer =
            GetComponent<SpriteRenderer>();

        SetCoffeeEmpty();
    }

    public void SetCoffeeReady()
    {
        coffeeReady = true;

        if (spriteRenderer != null &&
            coffeeMugSprite != null)
        {
            spriteRenderer.sprite = coffeeMugSprite;
        }

        Debug.Log("MUG SEKARANG BERISI COFFEE!");
    }

    public void SetCoffeeEmpty()
    {
        coffeeReady = false;
        Debug.Log("MUG SEKARANG KOSONG!");
    }

    void OnMouseDown()
    {
        Debug.Log(
            "!!! COFFEE ITEM DIKLIK !!! READY: "
            + coffeeReady
        );
    }
}