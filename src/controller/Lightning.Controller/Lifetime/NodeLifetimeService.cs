using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Lightning.Controller.Lifetime
{
    public class NodeLifetimeService : INodeLifetimeService
    {
        private readonly Dictionary<string, Channel<NodeState>> _nodeChannels;
        private readonly Dictionary<string, NodeState> _nodes;
        private readonly ILogger<NodeLifetimeService>? _logger;
        private readonly Channel<(string nodeId, NodeState state)> _allUpdatesChannel;

        public NodeLifetimeService(ILogger<NodeLifetimeService>? logger = null)
        {
            _nodeChannels = new Dictionary<string, Channel<NodeState>>();
            _nodes = new Dictionary<string, NodeState>();
            _allUpdatesChannel = Channel.CreateUnbounded<(string, NodeState)>();
        }

        public bool AuthenticateNode(string nodeId)
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<NodeState> GetNodeStatesAllAsync(string? nodeId = null)
        {
            //throw new System.NotImplementedException();
            return null!;
        }

        public async Task UpdateNodeStateAsync(NodeState state, string? nodeId = null)
        {
            if (nodeId is not null)
            {
                if (!_nodes.ContainsKey(nodeId))
                {
                    _nodes.Add(nodeId, state);
                    _nodeChannels.Add(nodeId, Channel.CreateUnbounded<NodeState>());
                }

                _nodes[nodeId] = state;
                await _nodeChannels[nodeId].Writer.WriteAsync(state);
                await _allUpdatesChannel.Writer.WriteAsync((nodeId, state));
                _logger?.LogDebug("NodeState from node '{}' changed to '{}'", nodeId, state);
            }
            else
            {
                foreach (var node in _nodes.Keys)
                {
                    await UpdateNodeStateAsync(state, node);
                }
            }
        }

        bool INodeLifetimeService.AuthenticateNode(string nodeId)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<(string NodeId, NodeState State)> GetAllNodeStates() => _nodes.Select(ns => (ns.Key, ns.Value));
    }
}
