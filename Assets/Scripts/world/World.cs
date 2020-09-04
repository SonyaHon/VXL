using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
  private GameObject _worldTransform;
  private List<Chunk> loaded_chunks;

  [HideInInspector]
  public bool noiseSettingsFoldout = true;
  public NoiseSettings noiseSettings;

  public int renderDistance = 15;

  private void Start()
  {
    _worldTransform = new GameObject("World Transform");
    _worldTransform.transform.parent = transform;

    InitializeLoadedChunks();
    UpdateLoadedChunks();
  }

  private void ClearLoadedChunks()
  {
    if (loaded_chunks == null) return;

    loaded_chunks.Clear();

    foreach (Transform child in _worldTransform.transform)
    {
      GameObject.Destroy(child.gameObject);
    }
  }

  private void InitializeLoadedChunks()
  {
    if (loaded_chunks == null)
    {
      loaded_chunks = new List<Chunk>();
    }

    for (int x = -renderDistance; x <= renderDistance; x++)
    {
      for (int y = -renderDistance; y <= renderDistance; y++)
      {
        if (x * x + y * y <= renderDistance * renderDistance)
        {
          Chunk chunk = new Chunk(new Vector2(x, y), noiseSettings);
          chunk.Generate();
          loaded_chunks.Add(chunk);
        }
      }
    }
  }

  /**
	* This is needed to rebuild entire loaded chunks
	* would be heavy. While in the game this should be called only when 
    * teleported etc.
	**/
  private void UpdateLoadedChunks()
  {
    foreach (Transform child in _worldTransform.transform)
    {
      GameObject.Destroy(child.gameObject);
    }

    foreach (Chunk loadedChunk in loaded_chunks)
    {
      loadedChunk.GameObject.transform.parent = _worldTransform.transform;
    }
  }


  public void DebugRegenerate()
  {
    ClearLoadedChunks();
    InitializeLoadedChunks();
    UpdateLoadedChunks();
  }
}