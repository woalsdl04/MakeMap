using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BLOCK_STYLE //블럭 스타일
{
    AIR, //블럭이 없다.
    BLOCK, //블럭이 있다.
}

public class Chunk : MonoBehaviour //청크 클래스 
{
    public BLOCK_STYLE[,,] BlockStyle = new BLOCK_STYLE[V.width * 2 + 2, V.height * 2 + 2, V.length*2 + 2];

    public GameObject Block;

    public void InitChunk() //청크 초기화
    {
        for (int x = -V.width; x <= V.width; x++)
        {
            for (int y = -V.height; y <= V.height; y++)
            {   
                for (int z = -V.length; z <= V.length; z++)
                {
                    SetBlockStyle(x , y, z, BLOCK_STYLE.AIR);// BlockStyle[x, y, z] = BLOCK_STYLE.AIR; 
                }
            }
        }

    }

    public void CreateBlock(float scale, float depth) //블럭 생성
    {
        InitChunk();

        for (int x = -V.width + 1; x < V.width; x++)
        {
            for (int z = -V.length + 1; z < V.length; z++)
            {
                float y = Mathf.PerlinNoise((transform.position.x + x) / scale, (transform.position.z + z) / scale);
                var pos = new Vector3Int(x, Mathf.RoundToInt(y * depth), z);
                var block_obj = Instantiate(Block, transform.position + pos, Quaternion.identity); // 블럭 생성
                CreateMesh(block_obj, this);
                
                SetBlockStyle(pos.x, pos.y, pos.z, BLOCK_STYLE.BLOCK);// BlockStyle[] = ;
            }
        }

        MeshCombine(); //메쉬 합치기
    }

    public void CreateMesh(GameObject block, Chunk parent) //메쉬 생성
    {
        block.transform.SetParent(parent.transform); //부모 설정

        Mesh mesh = new Mesh();
        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int numFaces = 0;

        var pos = block.transform.position - parent.transform.position;

        if(parent.GetBlockStyle(pos.x, pos.y + 1, pos.z) == BLOCK_STYLE.AIR) //위
        {
            verts.Add( new Vector3(0, 1, 0));
            verts.Add( new Vector3(0, 1, 1));
            verts.Add( new Vector3(1, 1, 1));
            verts.Add( new Vector3(1, 1, 0));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x, pos.y, pos.z - 1) == BLOCK_STYLE.AIR) //앞
        {
            verts.Add( new Vector3(0, 0, 0));
            verts.Add( new Vector3(0, 1, 0));
            verts.Add( new Vector3(1, 1, 0));
            verts.Add( new Vector3(1, 0, 0));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x + 1, pos.y, pos.z) == BLOCK_STYLE.AIR) //오른쪽
        {
            verts.Add( new Vector3(1, 0, 0));
            verts.Add( new Vector3(1, 1, 0));
            verts.Add( new Vector3(1, 1, 1));
            verts.Add( new Vector3(1, 0, 1));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x, pos.y, pos.z + 1) == BLOCK_STYLE.AIR) //뒤
        {
            verts.Add( new Vector3(1, 0, 1));
            verts.Add( new Vector3(1, 1, 1));
            verts.Add( new Vector3(0, 1, 1));
            verts.Add( new Vector3(0, 0, 1));
            numFaces++;
        }

        if(parent.GetBlockStyle(pos.x - 1, pos.y, pos.z) == BLOCK_STYLE.AIR) //왼쪽
        {
            verts.Add( new Vector3(0, 0, 1));
            verts.Add( new Vector3(0, 1, 1));
            verts.Add( new Vector3(0, 1, 0));
            verts.Add( new Vector3(0, 0, 0));
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

        block.GetComponent<MeshFilter>().mesh = mesh;
    }

    public void MeshCombine() //메쉬 합치기
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        
        var BackUpPos = transform.position; //chunk 위치 백업
        transform.position = Vector3.zero; // 위치 초기화

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh; //메쉬 추가
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            
            if(meshFilters[i].name != this.name) //부모가 아닐때만, 부모메쉬로 보여지기 때문에 자식 메쉬는 렌더링 하지 않음
            meshFilters[i].gameObject.SetActive(false); 
        }

        GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.position = BackUpPos; //로드
    }
    
    public BLOCK_STYLE GetBlockStyle(float x, float y, float z) //블럭 스타일 가져오기
    {
        return BlockStyle[(int)x + V.width, (int)y + V.height, (int)z + V.length];
    }

    public void SetBlockStyle(float x, float y, float z, BLOCK_STYLE value) //블럭 스타일 설정
    {
        BlockStyle[(int)x + V.width, (int)y + V.height, (int)z + V.length] = value;
    }

    
}
