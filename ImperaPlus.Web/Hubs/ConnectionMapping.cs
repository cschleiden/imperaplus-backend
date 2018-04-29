using System.Collections.Generic;
using System.Linq;

namespace ImperaPlus.Web.Hubs
{
    internal class ConnectionInfo
    {
        public ConnectionInfo()
        {
            this.ConnectionIds = new HashSet<string>();
            this.GroupNames = new HashSet<string>();
        }

        public HashSet<string> ConnectionIds { get; private set; }

        public HashSet<string> GroupNames { get; private set; } 
    }

    internal class ConnectionMapping<T>
    {
        private readonly Dictionary<T, ConnectionInfo> connections = new Dictionary<T, ConnectionInfo>();        
        private readonly Dictionary<string, T> connectionIds = new Dictionary<string, T>();

        public void JoinGroup(T key, string groupName)
        {
            lock (this.connections)
            {
                ConnectionInfo connectionInfo;
                if (!this.connections.TryGetValue(key, out connectionInfo))
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
            lock (this.connections)
            {
                if (!this.connections.TryGetValue(key, out ConnectionInfo connectionInfo))
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
            lock (this.connections)
            {
                if (!this.connections.TryGetValue(key, out ConnectionInfo connectionInfo))
                {
                    connectionInfo = new ConnectionInfo();
                    this.connections.Add(key, connectionInfo);
                }

                lock (connectionInfo)
                {
                    if (!this.connectionIds.ContainsKey(connectionId))
                    {
                        this.connectionIds.Add(connectionId, key);
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

            lock (this.connectionIds)
            {
                if (this.connections.TryGetValue(key, out connectionInfo))
                {
                    return connectionInfo.ConnectionIds;
                }
            }

            return Enumerable.Empty<string>();
        }

        public bool Remove(string connectionId, out IEnumerable<string> groups, out T key)
        {
            lock (this.connections)
            {
                if (!this.connectionIds.ContainsKey(connectionId))
                {
                    groups = null;
                    key = default(T);

                    return false;
                }

                key = this.connectionIds[connectionId];

                ConnectionInfo connectionInfo;
                if (!this.connections.TryGetValue(key, out connectionInfo))
                {
                    groups = null;

                    return false;
                }

                lock (connectionInfo)
                {
                    this.connectionIds.Remove(connectionId);

                    connectionInfo.ConnectionIds.Remove(connectionId);

                    if (connectionInfo.ConnectionIds.Count == 0)
                    {
                        groups = connectionInfo.GroupNames;

                        this.connections.Remove(key);

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
            lock (this.connections)
            {
                ConnectionInfo connectionInfo;
                if (!this.connections.TryGetValue(key, out connectionInfo))
                {
                    return;
                }

                lock (connectionInfo)
                {
                    connectionInfo.ConnectionIds.Remove(connectionId);

                    if (connectionInfo.ConnectionIds.Count == 0)
                    {
                        this.connections.Remove(key);
                    }
                }
            }
        }

        public IEnumerable<T> GetUsersForGroup(string groupName)
        {
            lock (this.connections)
            {
                return this.connections.Where(x => x.Value.GroupNames.Contains(groupName)).Select(x => x.Key);
            }
        }

        public IEnumerable<string> GetGroupsForUser(T key)
        {
            lock (this.connections)
            {
                if (!this.connections.ContainsKey(key))
                {
                    return Enumerable.Empty<string>();
                }

                var connectionInfo = this.connections[key];

                return connectionInfo.GroupNames;
            }
        }
    }
}