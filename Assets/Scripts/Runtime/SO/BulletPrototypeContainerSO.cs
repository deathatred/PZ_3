using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = ("Bullets Prototypes Container"))]
public class BulletPrototypeContainerSO : ScriptableObject
{
    public List<BulletPrototypeEntry> BulletPrototypes;
}
