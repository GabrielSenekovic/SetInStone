using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MaterialReplacer : MonoBehaviour
{
    [System.Serializable]
    public class MaterialPair
    {
        public Material old;
        public Material novel;
    }

    public List<MaterialPair> list = new List<MaterialPair>();

    private void Update() 
    {
        MeshRenderer[] temp = FindObjectsOfType<MeshRenderer>();
        for(int i = 0; i < temp.Length; i++)
        {
            for(int j = 0; j < list.Count; j++)
            {
                if(temp[i].sharedMaterial == list[j].old)
                {
                    temp[i].sharedMaterial = list[j].novel;
                }
            }
        }
    }
}
