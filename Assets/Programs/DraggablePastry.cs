using UnityEngine;

public class DraggablePastry : MonoBehaviour
{
    private GameObject heldPastry;
    private string pastryName;

    void OnMouseDown()
    {
        PastryItems pastry = GetComponent<PastryItems>();

        if (pastry == null)
            pastry = GetComponentInChildren<PastryItems>();

        if (pastry == null)
            pastry = GetComponentInParent<PastryItems>();

        if (pastry == null)
        {
            Debug.LogError(
                "PastryItems TIDAK DITEMUKAN: " + gameObject.name
            );
            return;
        }

        pastryName = pastry.pastryName;

        Debug.Log("PASTRY DIAMBIL: " + pastryName);

        heldPastry = Instantiate(
            gameObject,
            transform.position,
            transform.rotation
        );

        DraggablePastry heldScript =
            heldPastry.GetComponent<DraggablePastry>();

        if (heldScript != null)
            Destroy(heldScript);
    }

    void Update()
    {
        if (heldPastry == null)
            return;

        if (Camera.main == null)
            return;

        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition.z = heldPastry.transform.position.z;
        heldPastry.transform.position = mousePosition;

        if (Input.GetMouseButtonUp(0))
            DropPastry();
    }

    void DropPastry()
    {
        Debug.Log("MENCOBA MEMBERIKAN: " + pastryName);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            heldPastry.transform.position,
            0.5f
        );

        foreach (Collider2D hit in hits)
        {
            CustomerMovement1 c1 =
                hit.GetComponentInParent<CustomerMovement1>();

            if (c1 != null && c1.IsWaitingForOrder())
            {
                DeliverPastry(c1.ReceivePastry, "CUSTOMER 1");
                return;
            }

            CustomerMovement2 c2 =
                hit.GetComponentInParent<CustomerMovement2>();

            if (c2 != null && c2.IsWaitingForOrder())
            {
                DeliverPastry(c2.ReceivePastry, "CUSTOMER 2");
                return;
            }

            CustomerMovement3 c3 =
                hit.GetComponentInParent<CustomerMovement3>();

            if (c3 != null && c3.IsWaitingForOrder())
            {
                DeliverPastry(c3.ReceivePastry, "CUSTOMER 3");
                return;
            }

            CustomerMovement4 c4 =
                hit.GetComponentInParent<CustomerMovement4>();

            if (c4 != null && c4.IsWaitingForOrder())
            {
                DeliverPastry(c4.ReceivePastry, "CUSTOMER 4");
                return;
            }

            CustomerMovement5 c5 =
                hit.GetComponentInParent<CustomerMovement5>();

            if (c5 != null && c5.IsWaitingForOrder())
            {
                DeliverPastry(c5.ReceivePastry, "CUSTOMER 5");
                return;
            }

            CustomerMovement6 c6 =
                hit.GetComponentInParent<CustomerMovement6>();

            if (c6 != null && c6.IsWaitingForOrder())
            {
                DeliverPastry(c6.ReceivePastry, "CUSTOMER 6");
                return;
            }
        }

        Debug.Log("PASTRY TIDAK DIBERIKAN KE CUSTOMER.");
        Destroy(heldPastry);
    }

    void DeliverPastry(
        System.Action<string> receivePastry,
        string customerName)
    {
        receivePastry.Invoke(pastryName);

        Debug.Log(
            pastryName + " DISERAHKAN KE " + customerName + "!"
        );

        Destroy(heldPastry);
    }
}