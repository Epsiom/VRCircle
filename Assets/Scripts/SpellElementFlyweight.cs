using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpellElementFlyweight
{
    [SerializeField] public SpellElement Element;

    [SerializeField] public Material PrimaryMaterial;
    [SerializeField] public Material SecondaryMaterial;
    [SerializeField] public Material TertiaryMaterial;

    [SerializeField] public Mesh PrimaryMesh;
    [SerializeField] public Mesh SecondaryMesh;
    [SerializeField] public Mesh TertiaryMesh;

    [SerializeField] public Color PrimaryColor;
    [SerializeField] public Color SecondaryColor;
    [SerializeField] public Color TertiaryColor;

    [SerializeField] public bool IsWithGravity;
    [SerializeField] public float GravityMass;

    [SerializeField] public float DamageMultiplier;
}
