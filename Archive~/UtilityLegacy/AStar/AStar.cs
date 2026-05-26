// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using XiheFramework.Core.Utility;
//
// namespace XiheFramework.Utility.AStar {
//     public static class AStar {
//         private static List<AStarNode> GetNeighbours(Dictionary<int, AStarNode> source, AStarNode node, bool includeDiagonal) {
//             var neighbours = new List<AStarNode>();
//
//             if (includeDiagonal) {
//                 for (var x = -1; x <= 1; x++)
//                 for (var y = -1; y <= 1; y++) {
//                     if (x == 0 && y == 0)
//                         continue;
//
//                     var checkX = node.gridX + x;
//                     var checkY = node.gridY + y;
//
//                     var key = CantorPairUtility.CantorPair(checkX, checkY);
//                     if (source.ContainsKey(key)) neighbours.Add(source[key]);
//                 }
//             }
//             else {
//                 var keys = new List<int>();
//                 keys.Add(CantorPairUtility.CantorPair(node.gridX - 1, node.gridY));
//                 keys.Add(CantorPairUtility.CantorPair(node.gridX + 1, node.gridY));
//                 keys.Add(CantorPairUtility.CantorPair(node.gridX, node.gridY + 1));
//                 keys.Add(CantorPairUtility.CantorPair(node.gridX, node.gridY - 1));
//
//                 neighbours.AddRange(from key in keys where source.ContainsKey(key) select source[key]);
//             }
//
//             return neighbours;
//         }
//
//         public static AStarNode[] GetPath(Dictionary<int, AStarNode> source, AStarNode startNode, AStarNode targetNode, bool includeDiagonalPath) {
//             var openSet = new List<AStarNode>();
//             var closedSet = new HashSet<AStarNode>();
//             openSet.Add(startNode);
//
//             while (openSet.Count > 0) {
//                 var node = openSet[0];
//                 for (var i = 1; i < openSet.Count; i++)
//                     if (openSet[i].TotalDst < node.TotalDst || openSet[i].TotalDst == node.TotalDst)
//                         if (openSet[i].toDst < node.toDst)
//                             node = openSet[i];
//
//                 openSet.Remove(node);
//                 closedSet.Add(node);
//
//                 if (node == targetNode) {
//                     var path = RetracePath(startNode, targetNode);
//                     return path;
//                 }
//
//                 foreach (var neighbour in GetNeighbours(source, node, includeDiagonalPath)) {
//                     if (!neighbour.walkable || closedSet.Contains(neighbour)) continue;
//
//                     var newCostToNeighbour = node.fromDst + GetDistance(node, neighbour);
//                     if (newCostToNeighbour < neighbour.fromDst || !openSet.Contains(neighbour)) {
//                         neighbour.fromDst = newCostToNeighbour;
//                         neighbour.toDst = GetDistance(neighbour, targetNode);
//                         neighbour.parent = node;
//
//                         if (!openSet.Contains(neighbour))
//                             openSet.Add(neighbour);
//                     }
//                 }
//             }
//
//             //cant find path
//             return null;
//         }
//
//         private static AStarNode[] RetracePath(AStarNode startNode, AStarNode endNode) {
//             var path = new List<AStarNode>();
//             var currentNode = endNode;
//
//             while (currentNode != startNode) {
//                 path.Add(currentNode);
//                 currentNode = currentNode.parent;
//             }
//
//             path.Reverse();
//
//             return path.ToArray();
//         }
//
//         private static int GetDistance(AStarNode nodeA, AStarNode nodeB) {
//             var dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
//             var dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
//
//             if (dstX > dstY)
//                 return 14 * dstY + 10 * (dstX - dstY);
//             return 14 * dstX + 10 * (dstY - dstX);
//         }
//     }
// }