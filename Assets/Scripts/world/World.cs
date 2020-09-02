using System;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    private GameObject _worldTransform;
    private List<Chunk> loaded_chunks;

    private void Start()
    {
        _worldTransform = new GameObject("World Transform");
        _worldTransform.transform.parent = transform;
        
        InitializeLoadedChunks();
        UpdateLoadedChunks();
    }

    private void InitializeLoadedChunks()
    {
        loaded_chunks = new List<Chunk>();

        for (int i = 0; i < 1; i++)
        {
            Chunk chunk = new Chunk(Vector2.zero);
            chunk.Generate();
            loaded_chunks.Add(chunk);
        }
    }

	/**
	* This is needed to rebuild entire loaded chunks
	* would be heavy. 
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
}