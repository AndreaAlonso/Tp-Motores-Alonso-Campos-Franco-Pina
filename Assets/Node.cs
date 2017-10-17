using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public Rect window;
    public string windowName;
    public List<int> allConnections;
    public int currentNodeToConnect;

    public Node(string n, int posX = 50, int posY = 50, int width = 400, int height = 100)
    {
        windowName = n;
        allConnections = new List<int>();
        window = new Rect(posX, posY, width, height);
    }
}
