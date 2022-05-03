using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    [System.Serializable] public struct MyLightEntry
    {
        public Light light;
        Transform source;
        public int index;

        public MyLightEntry(Light light_in, Transform transform_in, int index_in)
        {
            light = light_in;
            source = transform_in;
            index = index_in;
        }
        public void Follow()
        {
            light.transform.position = source.position;
        }
    }
    [System.Serializable] public struct VFXEntry
    {
        public VisualEffect VFX;
        Transform source;
        public bool followSource; 
        //If on, the the VFX will follow the projectile. 
        //If off, it will go off and linger around one point, for instance where something is hit

        public int index;
        public VFXEntry(VisualEffect VFX_in, Transform transform_in,int index_in, bool follow_in)
        {
            VFX = VFX_in;
            source = transform_in;
            index = index_in;
            followSource = follow_in;
        }
        public void Follow()
        {
            if(followSource)
            {
                VFX.transform.position = source.position;
            }
        }
        public void ChangePosition(Vector3 position)
        {
            VFX.transform.position = position;
        }

        public void SetFollow(bool value)
        {
            followSource = value;
        }
    }
    List<MyLightEntry> lights = new List<MyLightEntry>();
    public List<VFXEntry> visualEffects = new List<VFXEntry>();

    private void FixedUpdate() 
    {
        //int limit = lights.Count >= visualEffects.Count ? lights.Count : visualEffects.Count;
        for(int i = 0; i < lights.Count; i++)
        {
            lights[i].Follow();
        }
        for (int i = 0; i < visualEffects.Count; i++)
        {
            visualEffects[i].Follow();
        }
    }
    public void Add(LightEntry light)
    {
        lights.Add(new MyLightEntry(light.light, light.light.transform.parent, light.index));
        light.light.transform.parent = transform;
    }
    public void Add(VisualEffectEntry VFX, bool follow)
    {
        int index = GetFreeIndex();
        visualEffects.Add(new VFXEntry(VFX.effect, VFX.effect.transform.parent, index, follow));
        VFX.effect.transform.parent = transform;
        VFX.index = index;
    }

    public int GetFreeIndex()
    {
        return visualEffects.Count +1;
    }
    public void ChangePosition(VisualEffectEntry VFX, Vector3 position)
    {
        for(int i = 0; i < visualEffects.Count; i++)
        {
            if(visualEffects[i].index == VFX.index)
            {
                visualEffects[i].ChangePosition(position);
            }
        }
    }
    public void ToggleVFXAndLight(VisualEffectEntry VFX, LightEntry light, bool value)
    {
        for(int i = 0; i < visualEffects.Count; i++)
        {
            if(visualEffects[i].index == VFX.index)
            {
                visualEffects[i].VFX.gameObject.SetActive(value);
            }
        }
        for(int i = 0; i < lights.Count; i++)
        {
            if(lights[i].index == light.index)
            {
                lights[i].light.gameObject.SetActive(value);
            }
        }
    }
    public void SetFollowSource(VisualEffectEntry VFX, bool value)
    {
        for(int i = 0; i < visualEffects.Count; i++)
        {
            if(visualEffects[i].index == VFX.index)
            {
                visualEffects[i].SetFollow(value);
            }
        }
    }
    public void Add(LightEntry light, VisualEffectEntry VFX)
    {
        Add(light);
        Add(VFX, true);
    }
}
