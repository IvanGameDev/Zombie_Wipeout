using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "New Map", menuName = "ScriptableObjects/Map")]
public class Map : ScriptableObject
{
    public int mapIndex;
    public string mapName;
    public string mapDescription;
    public Color nameColor;
    public Sprite mapImage;
    public Object sceneToLoad;
}
