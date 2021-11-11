using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Web.Hubs
{
    internal class ConnectionInfo
    {
        public ConnectionInfo()
        {
            ConnectionIds = new HashSet<string>();
            GroupNames = new HashSet<string>();
        }

        public HashSet<string> ConnectionIds { get; private set; }

        public HashSet<string> GroupNames { get; private set; }
    }

    internal class ConnectionMapping<T>
    {
        private readonly Dictionary<T, ConnectionInfo> connections = new();
        private readonly Dictionary<string, T> connectionIds = new();

        public void JoinGroup(T key, string groupName)
        {
            lock (connections)
            {
                ConnectionInfo connectionInfo;
                if (!connections.TryGetValue(key, out connectionInfo))
                {
                    return;
                }

                lock (connectionInfo)
                {
                    connectionInfo.GroupNames.Add(groupName);
                }
            }
        }

        public void LeaveGroup(T key, string groupName)
        {
            lock (connections)
            {
                if (!connections.TryGetValue(key, out var connectionInfo))
                {
                    return;
                }

                lock (connectionInfo)
                {
                    connectionInfo.GroupNames.Remove(groupName);
                }
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (connections)
            {
                if (!connections.TryGetValue(key, out var connectionInfo))
                {
                    connectionInfo = new ConnectionInfo();
                    connections.Add(key, connectionInfo);
                }

                lock (connectionInfo)
                {
                    if (!connectionIds.ContainsKey(connectionId))
                    {
                        connectionIds.Add(connectionId, key);
                    }

                    if (!connectionInfo.ConnectionIds.Contains(connectionId))
                    {
                        connectionInfo.ConnectionIds.Add(connectionId);
                    }
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            ConnectionInfo connectionInfo;

            lock (connectionIds)
            {
                if (connections.TryGetValue(key, out connectionInfo))
                {
                    return connectionInfo.ConnectionIds;
                }
            }

            return Enumerable.Empty<string>();
        }

        public bool Remove(string connectionId, out IEnumerable<string> groups, out T key)
        {
            lock (connections)
            {
                if (!connectionIds.ContainsKey(connectionId))
                {
                    groups = null;
                    key = default;

                    return false;
                }

                key = connectionIds[connectionId];

                ConnectionInfo connectionInfo;
                if (!connections.TryGetValue(key, out connectionInfo))
                {
                    groups = null;

                    return false;
                }

                lock (connectionInfo)
                {
                    connectionIds.Remove(connectionId);

                    connectionInfo.ConnectionIds.Remove(connectionId);

                    if (connectionInfo.ConnectionIds.Count == 0)
                    {
                        groups = connectionInfo.GroupNames;

                        connections.Remove(key);

                        // That was the last connection for this user
                        return true;
                    }
                }
            }

            groups = null;

            return false;
        }

        public void Remove(T key, string connectionId)
        {
            lock (connections)
            {
                ConnectionInfo connectionInfo;
                if (!connections.TryGetValue(key, out connectionInfo))
                {
                    return;
                }

                lock (connectionInfo)
                {
                    connectionInfo.ConnectionIds.Remove(connectionId);

                    if (connectionInfo.ConnectionIds.Count == 0)
                    {
                        connections.Remove(key);
                    }
                }
            }
        }

        public IEnumerable<T> GetUsersForGroup(string groupName)
        {
            lock (connections)
            {
                return connections.Where(x => x.Value.GroupNames.Contains(groupName)).Select(x => x.Key);
            }
        }

        public IEnumerable<string> GetGroupsForUser(T key)
        {
            lock (connections)
            {
                if (!connections.ContainsKey(key))
                {
                    return Enumerable.Empty<string>();
                }

                var connectionInfo = connections[key];

                return connectionInfo.GroupNames;
            }
        }
    }
}
