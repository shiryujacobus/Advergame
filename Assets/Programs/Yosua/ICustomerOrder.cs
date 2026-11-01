public interface ICustomerOrder
{
    bool CanAcceptOrder();
    bool IsWaitingForOrder();
    void AcceptOrder();

    bool AddItemToBag(string itemName);
    bool DeliverBag();
}