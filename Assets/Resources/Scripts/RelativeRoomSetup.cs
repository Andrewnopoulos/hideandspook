using UnityEngine;
using System.Collections;

public class RelativeRoomSetup : MonoBehaviour {
    [SerializeField]
    private SteamVR_PlayArea playArea;

    [SerializeField]
    private GameObject[] cornerObjects;

    [SerializeField]
    private GameObject[] wallObjects;
	
	// Update is called once per frame
	void Start ()
    {
        //Set positions for each prefab
        float rotation = 0f;

        Vector3[] vertices = playArea.vertices;
        if (vertices == null || vertices.Length == 0)
            return;
        
        for (int i = 0; i < 4; i++)
        {
            //Next vertex
            int next = (i + 1) % 4;

            //Vertices
            var a = transform.TransformPoint(vertices[i]);
            var c = transform.TransformPoint(vertices[next]);

            Debug.DrawLine(a, c);
            //Set corner bit
            GameObject corner = Instantiate<GameObject>(cornerObjects[i]);
            corner.transform.position = a;
            corner.transform.eulerAngles = new Vector3(0f, rotation, 0f);


            //Position for the center bit(get distance)
            Vector3 wallPos = Vector3.Lerp(a, c, 0.5f);
            GameObject wall = Instantiate<GameObject>(wallObjects[i]);
            wall.transform.position = wallPos;
            wall.transform.eulerAngles = new Vector3(0f, rotation, 0f);

            //Increment rotation
            rotation += 90f;
        }
    }
}
