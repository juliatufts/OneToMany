using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph {

    public List<GraphVertex> vertices;
    public Dictionary<GraphVertex, HashSet<GraphVertex>> adjList;
    public Dictionary<int, GraphVertex> meshToGraphVertexDict; // Mesh vertex index to graph vertex

    public Graph()
    {
        vertices = new List<GraphVertex>();
        adjList = new Dictionary<GraphVertex, HashSet<GraphVertex>>();
		meshToGraphVertexDict = new Dictionary<int, GraphVertex>();
    }

    public void Initialize(Mesh mesh)
    {
        // Create vertices from mesh vertices
        var meshVertices = mesh.vertices;
		for (var i = 0; i < meshVertices.Length; i++)
		{
            GraphVertex v = new GraphVertex(i);
			vertices.Add(v);
            adjList.Add(v, new HashSet<GraphVertex>());
			meshToGraphVertexDict.Add(i, v);
		}

        // Compute adjacency
		for (var i = 0; i < mesh.triangles.Length; i += 3)
		{
			int v0 = mesh.triangles[i];
			int v1 = mesh.triangles[i + 1];
			int v2 = mesh.triangles[i + 2];

			GraphVertex g0 = meshToGraphVertexDict[v0];
			GraphVertex g1 = meshToGraphVertexDict[v1];
			GraphVertex g2 = meshToGraphVertexDict[v2];

            adjList[g0].Add(g1);
            adjList[g0].Add(g2);

            adjList[g1].Add(g0);
            adjList[g1].Add(g2);

            adjList[g2].Add(g0);
            adjList[g2].Add(g1);
		}

		// Identify vertices at the same position
		var epsilon = 0.001f;
		for (var i = 0; i < meshVertices.Length - 1; i++)
		{
			if (vertices[i] == null) { continue; }

			for (var j = i + 1; j < meshVertices.Length; j++)
			{
				if (vertices[j] == null) { continue; }

				if (Vector3.Distance(meshVertices[i], meshVertices[j]) <= epsilon)
				{
                    var v1 = meshToGraphVertexDict[i];
                    var v2 = meshToGraphVertexDict[j];

                    foreach (var v in adjList[v2])
                    {
                        adjList[v1].Add(v);
                        adjList[v].Add(v1);
                        adjList[v].Remove(v2);
                    }
                    
                    if (adjList[v1].Contains(v2))
                    {
                        adjList[v1].Remove(v2);
                    }

                    foreach (var index in v2.meshIndices)
                    {
                        v1.meshIndices.Add(index);
                    }
                    
                    meshToGraphVertexDict[j] = v1;
                    vertices[j] = null;
                    adjList[v2] = null; 
                }
			}
		}
        vertices.RemoveAll(x => x == null);
        adjList = adjList.Where(x => x.Value != null).ToDictionary(p => p.Key, p => p.Value);
    }

    public SpanningTree GetBFSTree(int index, int maxDist)
    {
        GraphVertex root = meshToGraphVertexDict[index];
        SpanningTree tree = new SpanningTree(root);
        tree.meshToGraphVertexDict = meshToGraphVertexDict;

        tree.vertices = vertices;
        foreach (var gv in vertices)
		{
			gv.visited = false;
            tree.adjList.Add(gv, new HashSet<GraphVertex>());
		}

        List<GraphVertex> withinReach = new List<GraphVertex>();
        HashSet<GraphVertex> visited = new HashSet<GraphVertex>();

        withinReach.Add(root);
        visited.Add(root);
        root.dist = 0;
        while (visited.Count < vertices.Count)
		{
            var v = withinReach[0];
            withinReach.RemoveAt(0);

            foreach (var n in adjList[v])
            {
                if (!visited.Contains(n))
                {
                    withinReach.Add(n);
                    visited.Add(n);
                    n.dist = v.dist + 1;

                    tree.adjList[v].Add(n);
                    tree.adjList[n].Add(v);
                }
            }
        }

        return tree;
    }

    public List<KeyValuePair<Vector3, Vector3>> FindMinSpanTreeEuclideanDistance(Vector3[] points)
    {
        float[] minCost = new float[points.Length];
        int[] minPair = new int[points.Length];

        for (int i = 0; i < points.Length; i++)
        {
            minCost[i] = Mathf.Infinity;
            minPair[i] = -1;
        }
        List<int> pointIndexQueue = new List<int>();
        for (int i = 0; i < points.Length; i++)
        {
            pointIndexQueue.Add(i);
        }

        while (pointIndexQueue.Count > 0)
        {
            pointIndexQueue = pointIndexQueue.OrderBy(x => minCost[x]).ToList();
            int i = pointIndexQueue.First();
            pointIndexQueue.RemoveAt(0);
            foreach (int j in pointIndexQueue)
            {
                float dist = Vector3.Distance(points[i], points[j]);
                if (dist < minCost[j])
                {
                    minCost[j] = dist;
                    minPair[j] = i;
                }
            }
        }

        var pairs = new List<KeyValuePair<Vector3, Vector3>>();
        for (int i = 0; i < points.Length; i++)
        {
            if (minPair[i] != -1)
                pairs.Add(new KeyValuePair<Vector3, Vector3>(points[i], points[minPair[i]]));
        }
        return pairs;
    }

    public void ListNeighbours(int meshIndex)
	{
		var str = "neighbours: ";
        var gv = meshToGraphVertexDict[meshIndex];
        foreach (var v in adjList[gv])
		{
			str += (v + " ");
		}
		Debug.Log(str);
	}
}
