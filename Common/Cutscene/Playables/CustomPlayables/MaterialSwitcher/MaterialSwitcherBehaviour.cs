using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

[Serializable]
public class MaterialSwitcherBehaviour : PlayableBehaviour {
    public Material material;
    public Color color;
}