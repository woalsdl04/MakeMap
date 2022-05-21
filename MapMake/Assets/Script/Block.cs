using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    public void CreateMesh(Chunk parent) //메쉬 생성
    {
        gameObject.transform.SetParent(parent.transform); //부모 설정

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int numFaces = 0;

        var pos = transform.position - parent.transform.position;

        if(parent.GetBlockStyle(pos.x, pos.y + 1, pos.z) == BLOCK_STYLE.AIR) //위
        {
            verts.Add(new Vector3(0, 1, 0));
            verts.Add(new Vector3(0, 1, 1));
            verts.Add(new Vector3(1, 1, 1));
            verts.Add(new Vector3(1, 1, 0));
            numFaces++;
        }

        // if(parent.GetBlockStyle(pos.x, pos.y - 1, pos.z) == BLOCK_STYLE.AIR) //아래
        // {
        //     verts.Add(new Vector3(0, 0, 0));
        //     verts.Add(new Vector3(1, 0, 0));
        //     verts.Add(new Vector3(1, 0, 1));
        //     verts.Add(new Vector3(0, 0, 1));
        //     numFaces++;
        // }

        if(parent.GetBlockStyle(pos.x, pos.y, pos.z - 1) == BLOCK_STYLE.AIR) //앞
        {
            verts.Add(new Vector3(0, 0, 0));
            verts.Add(new Vector3(0, 1, 0));
            verts.Add(new Vector3(1, 1, 0));
            verts.Add(new Vector3(1, 0, 0));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x + 1, pos.y, pos.z) == BLOCK_STYLE.AIR) //오른쪽
        {
            verts.Add(new Vector3(1, 0, 0));
            verts.Add(new Vector3(1, 1, 0));
            verts.Add(new Vector3(1, 1, 1));
            verts.Add(new Vector3(1, 0, 1));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x, pos.y, pos.z + 1) == BLOCK_STYLE.AIR) //뒤
        {
            verts.Add(new Vector3(1, 0, 1));
            verts.Add(new Vector3(1, 1, 1));
            verts.Add(new Vector3(0, 1, 1));
            verts.Add(new Vector3(0, 0, 1));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x - 1, pos.y, pos.z) == BLOCK_STYLE.AIR) //왼쪽
        {
            verts.Add(new Vector3(0, 0, 1));
            verts.Add(new Vector3(0, 1, 1));
            verts.Add(new Vector3(0, 1, 0));
            verts.Add(new Vector3(0, 0, 0));
            numFaces++;
        }

        int tl = verts.Count - 4 * numFaces;
        for(int i = 0; i < numFaces; i++) 
        {
            tris.AddRange(new int[] { tl + i * 4, tl + i * 4 + 1, tl + i * 4 + 2, tl + i * 4, tl + i * 4 + 2, tl + i * 4 + 3 });
        }

        mesh.vertices = verts.ToArray();
        mesh.triangles = tris.ToArray();

        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
