
using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class CustomerReceiveBag : MonoBehaviour, IPointerClickHandler
{
    private MonoBehaviour customerScript;

    private void Awake()
    {
        FindCustomerScript();
    }

    private void FindCustomerScript()
    {
        customerScript = null;

        MonoBehaviour[] scripts = GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            if (script == null || script == this)
                continue;

            if (script.GetType().Name == "CustomerMovement2")
            {
                customerScript = script;
                break;
            }
        }

        if (customerScript == null)
        {
            Debug.LogWarning(
                "CustomerMovement2 tidak ditemukan pada " + gameObject.name,
                gameObject
            );
        }
    }

    // Untuk customer UI di dalam Canvas.
    public void OnPointerClick(PointerEventData eventData)
    {
        TryServeCustomer();
    }

    // Untuk customer berupa objek di Scene.
    private void OnMouseDown()
    {
        TryServeCustomer();
    }

    private void TryServeCustomer()
    {
        // Pastikan sistem kantong tersedia.
        if (FoodBagSystem.Instance == null)
        {
            Debug.LogWarning("FoodBagSystem belum tersedia.", gameObject);
            return;
        }

        // Pastikan mode pengantaran aktif.


        Debug.Log("Klik customer terdeteksi: " + gameObject.name);

        // Cari ulang jika script customer belum ditemukan.
        if (customerScript == null)
        {
            FindCustomerScript();
        }

        if (customerScript == null)
        {
            Debug.LogWarning(
                "CustomerMovement2 tidak ditemukan pada " + gameObject.name,
                gameObject
            );
            return;
        }

        Debug.Log(
            "Script customer ditemukan: " + customerScript.GetType().Name
        );

        // TODO: Hubungkan validasi pesanan dan proses penyerahan makanan.
    }
}
