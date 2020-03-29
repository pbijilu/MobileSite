using System.Collections.Generic;
using System.Text;
using MobileSite.Core;

namespace MobileSite.Data
{
    public interface IItemData
    {
        IEnumerable<Item> GetItemsByName(string name);
        IEnumerable<Item> GetItems();
        Item GetById(int id);
        Item Update(Item updatedItem);
        Item Add(Item newItem);
        Item Delete(int id);
        int Commit();
    }
}
