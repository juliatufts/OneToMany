using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpanningTree : Graph {

    public GraphVertex root;

    public SpanningTree(GraphVertex root) : base()
    {
        this.root = root;
        vertices.Add(root);
        root.dist = 0;
        root.visited = true;
    }
}
