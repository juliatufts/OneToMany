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
		var epsilon = 0.01f;
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
                    }
                    adjList[v1].Add(v2);
                    adjList[v1].Remove(v1);
                    v2 = v1;

                    vertices[j] = null;
				}
			}
		}
		vertices.RemoveAll(x => x == null);
    }

    public SpanningTree GetSpanningTree(int index, int maxDist)
    {
        GraphVertex root = meshToGraphVertexDict[index];
        SpanningTree tree = new SpanningTree(root);
        tree.meshToGraphVertexDict = meshToGraphVertexDict;

		foreach (var gv in vertices)
		{
			gv.visited = false;
		}

        tree.adjList[root] = adjList[root];
        HashSet<GraphVertex> withinReach = new HashSet<GraphVertex>(adjList[root]);

        var reach = 1;
        foreach (var v in withinReach)
        {
            tree.vertices.Add(v);
            tree.adjList.Add(v, new HashSet<GraphVertex>());
            tree.adjList[v].Add(root);

			v.dist = reach;
			v.visited = true;
        }
        reach++;


        while (withinReach.Count > 0)
		{
            var v = withinReach.First();
            foreach (var n in adjList[v])
            {
                if (!n.visited)
                {
                    withinReach.Add(n);
					tree.vertices.Add(n);
                    tree.adjList.Add(n, new HashSet<GraphVertex>());
                    n.dist = reach;
                    n.visited = true;

					tree.adjList[v].Add(n);
                    tree.adjList[n].Add(v);
                }
            }
            withinReach.Remove(v);

            reach++;
            if (reach == maxDist) { break; }
        }

        return tree;
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
