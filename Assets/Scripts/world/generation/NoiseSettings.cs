using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu()]
public class NoiseSettings : ScriptableObject
{
  public float baseFrequency = 3.0f;
  public float frequencyMultiplier = 2.0f;

  [Range(1, 10)]
  public int octaves = 1;

  public float basePersistance = 1.0f;
  public float persistanceMultiplier = 0.5f;

  public int maxFloorHeight = 10;
  public float pow = 1.0f;

  public bool useRounding = false;
  public bool clamValues = false;

  public bool drawDebugPoints = false;
  public bool drawDebugCorners = false;

  [SerializeField]
  public GameObject pointPrefab;
}