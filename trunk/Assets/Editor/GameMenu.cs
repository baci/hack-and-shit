using UnityEngine;
using UnityEditor;

public class GameMenu : MonoBehaviour 
{
    [MenuItem("Custom/CreateBoard")]
    static void CreateBoard()
    {
        var game = Selection.activeTransform.gameObject.GetComponent<Game>();
        var cornerPiece = game.corner90prefab;
        var straightPiece = game.straightPrefab;
        var forkPiece = game.forkPrefab;

        Vector2 boardSize = game.BoardSize;
        Vector2 topCorner = new Vector2(-(boardSize.x - 1) * 0.5f, -(boardSize.y - 1) * 0.5f);
        for (int x = (int)topCorner.x; x < topCorner.x+boardSize.x; x++)
        {
            for (int y = (int)topCorner.y; y < topCorner.y+boardSize.y; y++)
            {
                if (game.transform.FindChild("Node " + x + " " + y) != null) continue;

                if (x == 0 && y == 0) continue;
                if (x == (int)topCorner.x && Mathf.Abs(y) > boardSize.y*0.5-2) continue;
                if (x == topCorner.x + boardSize.x - 1 && Mathf.Abs(y) > boardSize.y*0.5-2) continue;
                if (y == (int)topCorner.y && Mathf.Abs(x) > boardSize.x * 0.5 - 2) continue;
                if (y == topCorner.y + boardSize.y - 1 && Mathf.Abs(x) > boardSize.x * 0.5 - 2) continue;

                GameObject nodePrefab;
                int rand = Random.Range(0, 5);
                if (rand == 0) nodePrefab = straightPiece;
                else if (rand == 1) nodePrefab = forkPiece;
                else nodePrefab = cornerPiece;
                var newNode = PrefabUtility.InstantiatePrefab(nodePrefab) as GameObject;
                newNode.transform.parent = game.transform;
                newNode.transform.position = new Vector3(x * 2, y * 2, 0);

                int rot = Random.Range(0, 4);
                newNode.GetComponent<ConnectionNode>().SetRotation(rot, true);

                newNode.name = "Node " + x + " " + y;
            }
        }

        //for (int x = (int)topCorner.x; x < topCorner.x + boardSize.x; x++)
        //{
        //    for (int y = (int)topCorner.y; y < topCorner.y + boardSize.y; y++)
        //    {
        //        var nodeObj = game.transform.FindChild("Node " + x + " " + y);
        //        if (nodeObj == null) continue;

        //        var node = nodeObj.GetComponent<ConnectionNode>();
        //        node.rightConnection = GetConnectionNode(x - 1, y);
        //        node.leftConnection = GetConnectionNode(x + 1, y);
        //        node.downConnection = GetConnectionNode(x, y - 1);
        //        node.upConnection = GetConnectionNode(x, y + 1);
        //    }
        //}

        var fileSender = game.transform.FindChild("FileSender").GetComponent<FileSender>();
        while (fileSender.connections.Count < 4) fileSender.connections.Add(null);
        if (fileSender.connections[0]==null) fileSender.connections[0] = (GetConnectionNode(-1, 0));
        if (fileSender.connections[1] == null) fileSender.connections[1] = (GetConnectionNode(1, 0));
        if (fileSender.connections[2] == null) fileSender.connections[2] = (GetConnectionNode(0, -1));
        if (fileSender.connections[3] == null) fileSender.connections[3] = (GetConnectionNode(0, 1));
    }

    static NetworkNode GetConnectionNode(int aX, int aY)
    {
        var game = Selection.activeTransform.gameObject;
        var connection = game.transform.FindChild("Node " + aX + " " + aY);
        if (connection == null) return null;
        return connection.GetComponent<ConnectionNode>();
    }

    [MenuItem("Custom/NextType")]
    static void NextType()
    {
        var game = FindObjectOfType<Game>();

        var piece = Selection.activeTransform.gameObject.GetComponent<ConnectionNode>();
        if (piece == null) return;

        GameObject nodePrefab;
        if (piece.type == ConnectionNode.ConnectionNodeType.corner)
            nodePrefab = game.straightPrefab;
        else if (piece.type == ConnectionNode.ConnectionNodeType.straight)
            nodePrefab = game.forkPrefab;
        else
            nodePrefab = game.corner90prefab;

        Debug.Log("Type " + piece.type);

        var newNode = PrefabUtility.InstantiatePrefab(nodePrefab) as GameObject;
        newNode.transform.parent = game.transform;
        newNode.transform.position = piece.transform.position;

        newNode.GetComponent<ConnectionNode>().SetRotation(piece.rotation, true);

        newNode.name = piece.name;

        DestroyImmediate(piece.gameObject);
    }
}
