using UnityEngine;

public class DraggablePastry : MonoBehaviour
{
    private GameObject heldPastry;
    private string pastryName;

    void OnMouseDown()
    {
        PastryItems pastry =
            GetComponent<PastryItems>();

        if (pastry == null)
        {
            pastry =
                GetComponentInChildren<PastryItems>();
        }

        if (pastry == null)
        {
            pastry =
                GetComponentInParent<PastryItems>();
        }

        if (pastry == null)
        {
            Debug.LogError(
                "PastryItems TIDAK DITEMUKAN DI HIERARCHY: "
                + gameObject.name
            );

            return;
        }

        pastryName = pastry.pastryName;

        Debug.Log(
            "PASTRY DIAMBIL: "
            + pastryName
        );

        heldPastry = Instantiate(
            gameObject,
            transform.position,
            transform.rotation
        );

        DraggablePastry heldScript =
            heldPastry.GetComponent<DraggablePastry>();

        if (heldScript != null)
        {
            Destroy(heldScript);
        }
    }

    void Update()
    {
        if (heldPastry == null)
            return;

        Vector3 mousePosition =
            Camera.main.ScreenToWorldPoint(
                Input.mousePosition
            );

        mousePosition.z =
            heldPastry.transform.position.z;

        heldPastry.transform.position =
            mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            DropPastry();
        }
    }

    void DropPastry()
    {
        Debug.Log(
            "MENCOBA MEMBERIKAN: " +
            pastryName
        );

        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                heldPastry.transform.position,
                0.5f
            );

        foreach (Collider2D hit in hits)
        {
            CustomerMovement1 customer1 =
                hit.GetComponentInParent<CustomerMovement1>();

            if (customer1 != null &&
                customer1.IsWaitingForOrder())
            {
                customer1.ReceivePastry(pastryName);

                Debug.Log(
                    pastryName +
                    " DIBERIKAN KE CUSTOMER 1!"
                );

                Destroy(heldPastry);
                return;
            }

            CustomerMovement2 customer2 =
                hit.GetComponentInParent<CustomerMovement2>();

            if (customer2 != null &&
                customer2.IsWaitingForOrder())
            {
                customer2.ReceivePastry(pastryName);

                Debug.Log(
                    pastryName +
                    " DIBERIKAN KE CUSTOMER 2!"
                );

                Destroy(heldPastry);
                return;
            }

            CustomerMovement3 customer3 =
                hit.GetComponentInParent<CustomerMovement3>();

            if (customer3 != null &&
                customer3.IsWaitingForOrder())
            {
                customer3.ReceivePastry(pastryName);

                Debug.Log(
                    pastryName +
                    " DIBERIKAN KE CUSTOMER 3!"
                );

                Destroy(heldPastry);
                return;
            }

            CustomerMovement4 customer4 =
                hit.GetComponentInParent<CustomerMovement4>();

            if (customer4 != null &&
                customer4.IsWaitingForOrder())
            {
                customer4.ReceivePastry(pastryName);

                Debug.Log(
                    pastryName +
                    " DIBERIKAN KE CUSTOMER 4!"
                );

                Destroy(heldPastry);
                return;
            }
        }

        Debug.Log(
            "PASTRY TIDAK DIBERIKAN KE CUSTOMER."
        );

        Destroy(heldPastry);
    }
}