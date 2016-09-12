using System.Collections.Generic;
using System.Linq;
using TotalApi.Utils.Extensions;

namespace WebApp.Hubs
{
    public class ConnectionMapping<T>
    {
        private readonly IDictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            if (key.IsNull()) return;
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            if (key.IsNull())
                return Enumerable.Empty<string>();
            lock (_connections)
            {
                HashSet<string> connections;
                return _connections.TryGetValue(key, out connections) ? connections : Enumerable.Empty<string>();
            }
        }

        public void Remove(T key, string connectionId)
        {
            if (key.IsNull()) return;
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                    return;

                lock (connections)
                {
                    connections.Remove(connectionId);
                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}