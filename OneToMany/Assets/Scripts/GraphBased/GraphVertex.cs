using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphVertex {

	public HashSet<int> meshIndices;
    public int dist; // for spanning tree
    public bool visited; // for graph traversal

	public GraphVertex(int meshIndex)
	{
		meshIndices = new HashSet<int>();
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
