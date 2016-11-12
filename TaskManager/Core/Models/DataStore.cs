using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class DataStore
    {        
        private static List<Event> eventsStore = new List<Event>();
        private static Queue<Tuple<Event, DataStoreAction>> dataStoreQueue = new Queue<Tuple<Event, DataStoreAction>>();

        private DataStore()
        {
            eventsStore.Add(new Event { Id = 1, Title = "Do this", Description = "Because this 1" });
            eventsStore.Add(new Event { Id = 2, Title = "Do that", Description = "Because this 2" });
            eventsStore.Add(new Event { Id = 3, Title = "Say this", Description = "Because this 3" });
            eventsStore.Add(new Event { Id = 4, Title = "Say that", Description = "Because this 4" });
            eventsStore.Add(new Event { Id = 5, Title = "Say nothing", Description = "Because this 5" });
            eventsStore.Add(new Event { Id = 6, Title = "Do nothing", Description = "Because this 6" });
            eventsStore.Add(new Event { Id = 7, Title = "Plan this", Description = "Because this 7" });
            eventsStore.Add(new Event { Id = 8, Title = "Plan that", Description = "Because this 8" });
        }

        public static DataStore GetInstance()
        {
            return Singleton.instance;
        }

        internal IQueryable<Event> Find(int id)
        {
            return eventsStore.Where(e => e.Id == id).AsQueryable();
        }

        internal IQueryable<Event> GetAll()
        {
            return eventsStore.AsQueryable();
        }

        internal Task PostAsync(Event newEvent)
        {
            return Task.Factory.StartNew
                (
                    () => eventsStore.Add(newEvent)
                );
        }

        internal Task<IQueryable<Event>> FindAsync(int key)
        {
            return Task.Factory.StartNew(() => { return Find(key); });
        }

        internal EventEntry Entry(Event newItem)
        {
            dataStoreQueue.Enqueue(Tuple.Create(newItem, DataStoreAction.Add));
            return new EventEntry();
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        internal void Remove(Event itemToRemove)
        {
            dataStoreQueue.Enqueue(Tuple.Create(itemToRemove, DataStoreAction.Delete));
        }

        private class Singleton
        {
            static Singleton()
            { /* required for compiler reasons */ }

            internal static readonly DataStore instance = new DataStore();
        }
    }
}