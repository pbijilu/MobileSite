using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MobileSite.Core;

namespace MobileSite.Data
{
    public class SqlItemData : IItemData
    {
        private readonly MobileSiteDbContext db;

        public SqlItemData(MobileSiteDbContext db)
        {
            this.db = db;
        }

        public Item Add(Item newItem)
        {
            db.Items.Add(newItem);

            return newItem;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Item Delete(int id)
        {
            var item = GetById(id);
            if (item != null)
            {
                db.Items.Remove(item);
            }
            return item;
        }

        public Item GetById(int id)
        {
            return db.Items.Find(id);
        }

        public IEnumerable<Item> GetItemsByName(string name)
        {
            var query = db.Items.Where(i => i.Name.StartsWith(name)|| string.IsNullOrEmpty(name)).OrderBy(i => i.Name);
            return query;
        }

        public IEnumerable<Item> GetItems()
        {
            return db.Items;
        }

        public Item Update(Item updatedItem)
        {
            var entity = db.Items.Attach(updatedItem);
            entity.State = EntityState.Modified;
            return updatedItem;
        }
    }
}
