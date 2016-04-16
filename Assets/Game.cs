using UnityEngine;
using System.Collections.Generic;
using Valve.VR;


public class Game : MonoBehaviour {

    HmdQuad_t _playArea;

    Vector3[] corners = new Vector3[4];

    public GameObject testText;

    string _path = "Prefabs/";

    public GameObject[] _worldObjects;

	public int minWidth;
	public int minLength;

	// Use this for initialization
	void Start () {
        InitGameArea();
    }

    void InitGameArea()
    {
        SteamVR_PlayArea.GetBounds(SteamVR_PlayArea.Size.Calibrated, ref _playArea);

        corners[0] = new Vector3(_playArea.vCorners0.v0, _playArea.vCorners0.v1, _playArea.vCorners0.v2);
        corners[1] = new Vector3(_playArea.vCorners1.v0, _playArea.vCorners1.v1, _playArea.vCorners1.v2);
        corners[2] = new Vector3(_playArea.vCorners2.v0, _playArea.vCorners2.v1, _playArea.vCorners2.v2);
        corners[3] = new Vector3(_playArea.vCorners3.v0, _playArea.vCorners3.v1, _playArea.vCorners3.v2);

        //int i = 0;
        //foreach (Vector3 v in corners)
        //{
        //    GameObject test = GameObject.Instantiate(testText);
        //    test.transform.position = v;
        //    test.GetComponent<TextMesh>().text = "index: " + i;
        //    i++;
            
        //}

        GameObject wallObject = Resources.Load<GameObject>(_path + "Wall");

        GameObject wallEast = GameObject.Instantiate(wallObject);
        GameObject wallWest = GameObject.Instantiate(wallObject);
        GameObject wallNorth = GameObject.Instantiate(wallObject);
        GameObject wallSouth = GameObject.Instantiate(wallObject);
        GameObject ground = GameObject.Instantiate(Resources.Load<GameObject>(_path + "Ground"));
		GameObject ceiling = GameObject.Instantiate(Resources.Load<GameObject>(_path + "Ceiling"));

		int Width = Mathf.Max(Mathf.CeilToInt(corners[3].x * 2), minWidth) ;
        int Length = Mathf.Max(Mathf.CeilToInt(corners[2].z * 2), minLength) ;
        int Height = 3;

        MeshRenderer renderer = wallSouth.GetComponent<MeshRenderer>();

        wallSouth.transform.SetLocalPosition(0, 1.5f, Length * -0.5f);
        wallSouth.transform.localRotation = Quaternion.Euler(0, 180, 0);
        wallSouth.transform.localScale = new Vector3(Width, Height, 1);
        renderer.material.mainTextureScale = new Vector2(Width, Height);

        renderer = wallWest.GetComponent<MeshRenderer>();
        wallWest.transform.SetLocalPosition(Width * -0.5f, 1.5f, 0);
        wallWest.transform.localRotation = Quaternion.Euler(0, 270, 0);
        wallWest.transform.localScale = new Vector3(Length, Height, 1);
        renderer.material.mainTextureScale = new Vector2(Length, Height);

        renderer = wallNorth.GetComponent<MeshRenderer>();
        wallNorth.transform.SetLocalPosition(0, 1.5f, Length * 0.5f);
        wallNorth.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wallNorth.transform.localScale = new Vector3(Width, Height, 1);
        renderer.material.mainTextureScale = new Vector2(Width, Height);

        renderer = wallEast.GetComponent<MeshRenderer>();
        wallEast.transform.SetLocalPosition(Width * 0.5f, 1.5f, 0);
        wallEast.transform.localRotation = Quaternion.Euler(0, 90, 0);
        wallEast.transform.localScale = new Vector3(Length, Height, 1);
        renderer.material.mainTextureScale = new Vector2(Length, Height);

        ground.transform.localScale = new Vector3(Width, Length, 1f);
        renderer = ground.GetComponent<MeshRenderer>();
        // Don't use shared material in editor (it modifies the material permanently)
        renderer.material.mainTextureScale = new Vector2(Width, Length);

		ceiling.transform.localScale = new Vector3(Width, Length, 1f);
		ceiling.transform.localPosition = new Vector3(0, Height, 0);
		renderer = ceiling.GetComponent<MeshRenderer>();
		// Don't use shared material in editor (it modifies the material permanently)
		renderer.material.mainTextureScale = new Vector2(Width, Length);

		Chunk chunk = new Chunk(Width, Length, Height);

        PopulateWall(chunk, 0, 0, 0, 0, 0, 1, 0, 1, 0, Length, Height, Quaternion.Euler(0, -90, 0));
        PopulateWall(chunk, 0, 0, Length-1, 1, 0, 0, 0, 1, 0, Width, Height, Quaternion.Euler(0, 0, 0));
        PopulateWall(chunk, Width-1, 0, Length-1, 0, 0, -1, 0, 1, 0, Length, Height, Quaternion.Euler(0, 90, 0));
        PopulateWall(chunk, Width-1, 0, 0, -1, 0, 0, 0, 1, 0, Width, Height, Quaternion.Euler(0, 180, 0));

        // instantiate gameobjects
        // place corner objects
        // place wall objects
    }

    void SetOccupied(Chunk _chunk, ObjectMeta meta, int x, int y, int z, int wdx, int wdy, int wdz, int ldx, int ldy, int ldz)
    {
        int cx, cy, cz;

        bool isValid = true;

        for (int i = 0; i < meta.height && isValid; ++i)
        {
            cx = x + ldx * i;
            cy = y + ldy * i;
            cz = z + ldz * i;

            for (int j = 0; j < meta.width; ++j, cx += wdx, cy += wdy, cz += wdz)
            {
                _chunk.setOccupied(cx, cy, cz);
            }
        }
    }

    GameObject GetValidObject(Chunk _chunk, int x, int y, int z, int wdx, int wdy, int wdz, int ldx, int ldy, int ldz)
    {
        if (_worldObjects == null)
        {
            return null;
        }

        List<ObjectMeta> valid = new List<ObjectMeta>();

        foreach (GameObject prefab in _worldObjects)
        {
            if (prefab == null)
            {
                continue;
            }

            ObjectMeta objectmeta = prefab.GetComponent<ObjectMeta>();
            if (objectmeta == null)
            {
                Debug.LogError("Object meta is null: " + prefab.name);
                continue;
            }

            int cx, cy, cz;

            bool isValid = true;
            bool grounded = false;
            bool centerY = false;
            bool ceiling = false;

            for (int i = 0; i < objectmeta.height && isValid; ++i)
            {
                cx = x + ldx * i;
                cy = y + ldy * i;
                cz = z + ldz * i;

                for (int j = 0; j < objectmeta.width; ++j, cx += wdx, cy += wdy, cz += wdz)
                {
                    if (!_chunk.isValidPosition(cx, cy, cz) || _chunk.occupied(cx, cy, cz))
                    {
                        isValid = false;
                        break;
                    }

                    bool isGrounded = cy == 0;
                    bool isCeiling = cy == _chunk.height - 1;

                    if (isGrounded)
                    {
                        grounded = true;
                    }

                    if (!isGrounded && !isCeiling)
                    {
                        centerY = true;
                    }

                    if (isCeiling)
                    {
                        ceiling = true;
                    }
                }
            }

            if (objectmeta._requireGrounded && !grounded)
            {
                isValid = false;
            }

			if (objectmeta._requireCenterY && (grounded || ceiling))
			{
				isValid = false;
			}

			if (objectmeta._requireCeiling && !ceiling)
			{
				isValid = false;
			}

            if (isValid)
            {
                valid.Add(objectmeta);
            }
        }

        if (valid.Count == 0)
        {
            return null;
        }

        return valid[Random.Range(0, valid.Count)].gameObject;
    }

    void PopulateWall(Chunk _chunk, int x, int y, int z, int wdx, int wdy, int wdz, int ldx, int ldy, int ldz, int width, int length, Quaternion rotation)
    {
        int cx, cy, cz;

        for (int i = 0; i < length; ++i)
        {
            cx = x + ldx * i;
            cy = y + ldy * i;
            cz = z + ldz * i;

            for (int j = 0; j < width; ++j, cx += wdx, cy += wdy, cz += wdz)
            {

                if(_chunk.occupied(cx, cy, cz))
                {
                    continue;
                }

                float rand = Random.Range(0, 1.0f);

                if (rand < 0.5f)
                {
                    continue;
                }

                GameObject prefab = GetValidObject(_chunk, cx, cy, cz, wdx, wdy, wdz, ldx, ldy, ldz);

                if (prefab == null)
                {
                    continue;
                }

                ObjectMeta prefabMeta = prefab.GetComponent<ObjectMeta>();

                GameObject obj = GameObject.Instantiate(prefab);
                obj.transform.localRotation = rotation;

                Vector3 pos = _chunk.mapToWorldSpace(cx, cy, cz);
                Vector3 widthDelta = new Vector3(wdx, wdy, wdz);
                Vector3 lengthDelta = new Vector3(ldx, ldy, ldz);

                pos += (prefabMeta.width - 1) * widthDelta * 0.5f;
                pos += (prefabMeta.height - 1) * lengthDelta * 0.5f;

                obj.transform.localPosition = pos;

                SetOccupied(_chunk, prefabMeta, cx, cy, cz, wdx, wdy, wdz, ldx, ldy, ldz);
            }
        }
    }

	// Update is called once per frame
	void Update () {
        
	}
}

public class Chunk
{
    public int width, length, height;
    bool[] map;

    public Chunk(int width, int length, int height)
    {
        this.width = width;
        this.length = length;
        this.height = height;

        map = new bool[width * length * height];
    }

    public Vector3 mapToWorldSpace(int x, int y, int z)
    {
        return new Vector3(width * -0.5f + x + 0.5f, y, length * -0.5f + z + 0.5f);
    }

    public int index(int x, int y, int z)
    {
        return (z * width * height) + (x * height) + y;
    }

    public bool occupied(int x, int y, int z)
    {
        return map[index(x, y, z)];
    }

    public void setOccupied(int x, int y, int z)
    {
        map[index(x, y, z)] = true;
    }

    public bool isValidPosition(int x, int y, int z)
    {
        return x >= 0 && x < width && y >= 0 && y < height && z >= 0 && z < length;
    }
}

public static class TransformExtensions
{
    public static void SetLocalPosition(this Transform transform, float? x, float? y, float? z)
    {
        Vector3 pos = transform.localPosition;
        if (x.HasValue)
        {
            pos.x = x.Value;
        }
        if (y.HasValue)
        {
            pos.y = y.Value;
        }
        if (z.HasValue)
        {
            pos.z = z.Value;
        }
        transform.localPosition = pos;
    }
}