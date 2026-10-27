
public interface ICustomerOrder
{
    bool IsWaitingForOrder();
    bool AddItemToBag(string itemName);
    bool DeliverBag();
}
