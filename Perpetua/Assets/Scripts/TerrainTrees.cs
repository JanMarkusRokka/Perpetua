using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTrees : MonoBehaviour
{
    public GameObject[] TreePrefabs;
    Terrain terrain;
    TreeInstance[] trees;

    void Awake()
    {
        terrain = GetComponent<Terrain>();
        TerrainData data = terrain.terrainData;
        trees = terrain.terrainData.treeInstances;
        terrain.treeDistance = 0;
        foreach (TreeInstance tree in trees)
        {
            var localPos = new Vector3(tree.position.x * data.size.x, tree.position.y * data.size.y, tree.position.z * data.size.z);
            var worldPos = Terrain.activeTerrain.transform.TransformPoint(localPos);
            GameObject treeInstance = Instantiate(TreePrefabs[tree.prototypeIndex], worldPos, Quaternion.Euler(0, Mathf.Rad2Deg * tree.rotation, 0), transform);
            treeInstance.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
        }
        //terrain.terrainData.treeInstances = new TreeInstance[0];
    }
    void OnApplicationQuit()
    {
        terrain.terrainData.treeInstances = trees;
    }
}
