
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomerBagDrop : MonoBehaviour, IDropHandler
{
    [Header("Pilih Movement Customer")]
    [SerializeField] private MonoBehaviour customerMovement;

    private ICustomerOrder customer;

    private void Awake()
    {
        if (customerMovement != null)
        {
            customer = customerMovement as ICustomerOrder;
        }

        if (customer == null)
        {
            Debug.LogError(
                "CustomerBagDrop: Movement belum dipilih " +
                "atau belum implement ICustomerOrder!",
                this
            );
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Drop terdeteksi pada: " + gameObject.name);

        if (customer == null || FoodBagSystem.Instance == null)
            return;

        FoodBagSystem.Instance.TryDeliverBagToCustomer(customer);
    }
}
