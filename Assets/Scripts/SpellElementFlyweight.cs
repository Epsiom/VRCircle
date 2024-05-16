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
    [SerializeField] public float PrimaryMeshScale = 1f;
    [SerializeField] public Mesh SecondaryMesh;
    [SerializeField] public float SecondaryMeshScale = 1f;
    [SerializeField] public Mesh TertiaryMesh;
    [SerializeField] public float TertiaryMeshScale = 1f;

    [SerializeField] public Color PrimaryColor;
    [SerializeField] public Color SecondaryColor;
    [SerializeField] public Color TertiaryColor;

    [SerializeField] public bool IsWithGravity;
    [SerializeField] public float GravityMass = 1f;

    [SerializeField] public float DamageMultiplier = 1f;
}
