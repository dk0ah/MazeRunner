using System.Collections.Generic;
using UnityEngine;

public class ChildObjectRearrange : MonoBehaviour
{
    void Start()
    {
        Invoke("RearrangeChildObjects",1);
    }

    void RearrangeChildObjects()
    {
        Transform parentTransform = transform;
        List<Transform> childTransforms = new List<Transform>();

        // Get all child transforms
        foreach (Transform child in parentTransform)
        {
            childTransforms.Add(child);
        }

        // Rearrange child transforms
        List<Transform> result = new List<Transform>();

        for (int i = 0; i < childTransforms.Count; i += 8)
        {
            // Add 4 values in ascending order
            if (i < childTransforms.Count) result.Add(childTransforms[i]);
            if (i + 1 < childTransforms.Count) result.Add(childTransforms[i + 1]);
            if (i + 2 < childTransforms.Count) result.Add(childTransforms[i + 2]);
            if (i + 3 < childTransforms.Count) result.Add(childTransforms[i + 3]);

            // Add 4 values in descending order
            if (i + 7 < childTransforms.Count) result.Add(childTransforms[i + 7]);
            if (i + 6 < childTransforms.Count) result.Add(childTransforms[i + 6]);
            if (i + 5 < childTransforms.Count) result.Add(childTransforms[i + 5]);
            if (i + 4 < childTransforms.Count) result.Add(childTransforms[i + 4]);
        }

        // Set the new order of child transforms
        for (int i = 0; i < childTransforms.Count; i++)
        {
            if (i < result.Count) childTransforms[i].SetSiblingIndex(result.IndexOf(childTransforms[i]));
        }
    }
}