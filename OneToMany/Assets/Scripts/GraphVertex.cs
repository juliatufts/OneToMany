using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVertex {

	public HashSet<int> meshIndices;
	public HashSet<GraphVertex> neighbours;

	public GraphVertex(int meshIndex)
	{
		meshIndices = new HashSet<int>();
		neighbours = new HashSet<GraphVertex>();
		meshIndices.Add(meshIndex);
	}

	public void AddIndex(int i)
	{
		meshIndices.Add(i);
	}

	public void RemoveIndex(int i)
	{
		meshIndices.Remove(i);
	}

	public void AddNeighbour(GraphVertex g)
	{
		neighbours.Add(g);
	}

	public void RemoveNeighbour(GraphVertex g)
	{
		neighbours.Remove(g);
	}

	public void ListNeighbours()
	{
        var str = "neighbours: ";
        foreach (var v in neighbours)
		{
			str += (v + " ");
		}
		Debug.Log(str);
	}

    public void ListMeshIndices()
    {
		var str = "mesh indices: ";
        foreach (var i in meshIndices)
		{
			str += (i + " ");
		}
		Debug.Log(str);
    }
}
