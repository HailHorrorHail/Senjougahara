using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class DataStore
    {
        private static Dictionary<int, Event> eventsStore = new Dictionary<int, Event>();
        private static Queue<Tuple<Event, DataStoreAction>> dataStoreQueue = new Queue<Tuple<Event, DataStoreAction>>();
        private static int ids = 0;
        private static readonly TaskFactory TF = new TaskFactory();

        private DataStore()
        {
            eventsStore.Add(1, new Event { Id = 1, Title = "Do this", Description = "Because this 1" });
            eventsStore.Add(2, new Event { Id = 2, Title = "Do that", Description = "Because this 2" });
            eventsStore.Add(3, new Event { Id = 3, Title = "Say this", Description = "Because this 3" });
            eventsStore.Add(4, new Event { Id = 4, Title = "Say that", Description = "Because this 4" });
            eventsStore.Add(5, new Event { Id = 5, Title = "Say nothing", Description = "Because this 5" });
            eventsStore.Add(6, new Event { Id = 6, Title = "Do nothing", Description = "Because this 6" });
            eventsStore.Add(7, new Event { Id = 7, Title = "Plan this", Description = "Because this 7" });
            eventsStore.Add(8, new Event { Id = 8, Title = "Plan that", Description = "Because this 8" });
            ids = 8;
        }

        public static DataStore GetInstance()
        {
            return Singleton.instance;
        }

        internal IQueryable<Event> Find(int id)
        {
            return eventsStore.Where(e => e.Key == id).Select(d => d.Value).AsQueryable();
        }

        internal IQueryable<Event> GetAll()
        {
            return eventsStore.Select(d => d.Value).AsQueryable();
        }

        internal Task PostAsync(Event newEvent)
        {
            return Task.Factory.StartNew
                (
                    () =>
                    {
                        newEvent.Id = ids++;
                        eventsStore.Add(newEvent.Id, newEvent);
                    }
                );
        }

        internal Task<IQueryable<Event>> FindAsync(int key)
        {
            return Task.Factory.StartNew(() => { return Find(key); });
        }

        internal EventEntry UpdateEntry(Event newItem)
        {
            dataStoreQueue.Enqueue(Tuple.Create(newItem, DataStoreAction.Update));
            return new EventEntry();
        }

        internal Task SaveChangesAsync()
        {
            // lame implementation
            return TF.StartNew(() => 
            {
                try
                {
                    while (dataStoreQueue.Count > 0)
                    {
                        var tp = dataStoreQueue.Dequeue();
                        Event e = tp.Item1;

                        switch (tp.Item2)
                        {
                            case DataStoreAction.Add:
                                eventsStore.Add(e.Id, e);
                                break;

                            case DataStoreAction.Delete:
                                eventsStore.Remove(e.Id);
                                break;

                            case DataStoreAction.Update:
                                if (!string.IsNullOrWhiteSpace(e.Title))
                                {
                                    eventsStore[e.Id].Title = e.Title;
                                }

                                if (!string.IsNullOrWhiteSpace(e.Description))
                                {
                                    eventsStore[e.Id].Description = e.Description;
                                }

                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Shit fucked up: {0}", ex.ToString());
                }
            });
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