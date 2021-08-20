using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room Gates Configuration")]
public class GatesConfig : ScriptableObject
{
    // Config Params
    [SerializeField] GameObject gatePositions;

    
    public List<Transform> GetGatePositions()
    {
        List<Transform> positions = new List<Transform>();

        foreach(Transform child in gatePositions.transform)
        {
            positions.Add(child);
        }

        return positions;
    }

    public List<string> GetGateTags()
    {
        List<string> tags = new List<string>();

        foreach (Transform child in gatePositions.transform)
        {
            tags.Add(child.tag);
        }

        return tags;
    }
}
