using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Graph {

    public List<GraphVertex> vertices;
    public Dictionary<int, GraphVertex> meshToGraphVertexDict; // Mesh vertex index to graph vertex

    public Graph()
    {
        vertices = new List<GraphVertex>();
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

			g0.AddNeighbour(g1);
			g0.AddNeighbour(g2);

			g1.AddNeighbour(g0);
			g1.AddNeighbour(g2);

			g2.AddNeighbour(g0);
			g2.AddNeighbour(g1);
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
					meshToGraphVertexDict[i].neighbours.UnionWith(meshToGraphVertexDict[j].neighbours);
					meshToGraphVertexDict[i].AddIndex(j);
					meshToGraphVertexDict[i].neighbours.Remove(meshToGraphVertexDict[i]);
					meshToGraphVertexDict[j] = meshToGraphVertexDict[i];

                    vertices[j] = null;
				}
			}
		}
		vertices.RemoveAll(x => x == null);
    }

    public List<int> GetSpanningTree(int index, int length)
    {
        if (length == 0)
        {
            return meshToGraphVertexDict[index].meshIndices.ToList();
        }
        return null;
    }
}
