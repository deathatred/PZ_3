using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Colors")]
public class ColorsSO : ScriptableObject
{
    public List<Color> ColorsList;
}

