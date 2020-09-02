using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
  private GameObject _gameObject;
  private Vector2 _position;

  public Chunk(Vector2 position)
  {
    _gameObject = new GameObject($"Chunk {position.x}:{position.y}");
    _gameObject.AddComponent<MeshFilter>();
    _gameObject.AddComponent<MeshRenderer>().material = new Material(Shader.Find("Standard"));
    _gameObject.AddComponent<MeshCollider>();

    _position = position;
    _gameObject.transform.position = new Vector3(position.x * VXL.instance.CHUNK_SIZE, 0, position.y * VXL.instance.CHUNK_SIZE);
  }

  public void Generate(NoiseSettings settings)
  {
    float[,] heightMap = GenerateHeightMap(settings);

    List<Vector3> verts = new List<Vector3>();
    List<int> indices = new List<int>();
    List<Vector3> normals = new List<Vector3>();

    int lastQuadStartingPoint = 0;

    for (int y = 0; y < heightMap.GetLength(0); y++)
    {
      for (int x = 0; x < heightMap.GetLength(1); x++)
      {
        float height = heightMap[y, x];
        Vector3 centerPoint = new Vector3(x, height, y);

        verts.Add(centerPoint + new Vector3(-.5f, 0, .5f));
        verts.Add(centerPoint + new Vector3(.5f, 0, .5f));
        verts.Add(centerPoint + new Vector3(-.5f, 0, -.5f));
        verts.Add(centerPoint + new Vector3(.5f, 0, -.5f));

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        indices.Add(lastQuadStartingPoint);
        indices.Add(lastQuadStartingPoint + 1);
        indices.Add(lastQuadStartingPoint + 2);

        indices.Add(lastQuadStartingPoint + 1);
        indices.Add(lastQuadStartingPoint + 3);
        indices.Add(lastQuadStartingPoint + 2);

        lastQuadStartingPoint += 4;

        if (settings.drawDebugPoints)
        {
          GameObject debugPoint = GameObject.Instantiate(settings.pointPrefab);
          debugPoint.name = $"Debug Point {x}:{y}";
          debugPoint.transform.position = centerPoint;
          debugPoint.transform.localScale = new Vector3(.2f, .2f, .2f);
          debugPoint.transform.parent = _gameObject.transform;
        }

        if (settings.drawDebugCorners)
        {
          GameObject debugPoint = GameObject.Instantiate(settings.pointPrefab);
          debugPoint.name = $"Debug Corner {x}:{y}:0";
          debugPoint.transform.position = centerPoint + new Vector3(-.5f, 0, .5f);
          debugPoint.transform.parent = _gameObject.transform;


          GameObject debugPoint2 = GameObject.Instantiate(settings.pointPrefab);
          debugPoint2.name = $"Debug Corner {x}:{y}:1";
          debugPoint2.transform.position = centerPoint + new Vector3(.5f, 0, .5f);
          debugPoint2.transform.parent = _gameObject.transform;


          GameObject debugPoint3 = GameObject.Instantiate(settings.pointPrefab);
          debugPoint3.name = $"Debug Corner {x}:{y}:2";
          debugPoint3.transform.position = centerPoint + new Vector3(-.5f, 0, -.5f);
          debugPoint3.transform.parent = _gameObject.transform;


          GameObject debugPoint4 = GameObject.Instantiate(settings.pointPrefab);
          debugPoint4.name = $"Debug Corner {x}:{y}:3";
          debugPoint4.transform.position = centerPoint + new Vector3(.5f, 0, -.5f);
          debugPoint4.transform.parent = _gameObject.transform;
        }
      }
    }


    int vertexOffset = verts.Count;
    for (int y = 0; y < heightMap.GetLength(0); y++)
    {
      for (int x = 0; x < heightMap.GetLength(1); x++)
      {
        // current quad height
        float currentQuadHeight = heightMap[y, x];
        int currentQuadFirstPoint = (x + y * VXL.instance.CHUNK_SIZE) * 4;
        Vector3[] currentQuad = new[]
        {
                    verts[currentQuadFirstPoint],
                    verts[currentQuadFirstPoint + 1],
                    verts[currentQuadFirstPoint + 2],
                    verts[currentQuadFirstPoint + 3]
                };

        if (y != heightMap.GetLength(0) - 1 && x != heightMap.GetLength(1) - 1)
        {
          float rightQuadHeight = heightMap[y, x + 1];
          int rightQuadFirstPoint = (x + 1 + y * VXL.instance.CHUNK_SIZE) * 4;
          Vector3[] rightQuad = new[]
          {
                        verts[rightQuadFirstPoint],
                        verts[rightQuadFirstPoint + 1],
                        verts[rightQuadFirstPoint + 2],
                        verts[rightQuadFirstPoint + 3]
                    };

          if (rightQuadHeight != currentQuadHeight)
          {
            verts.Add(currentQuad[1]);
            verts.Add(currentQuad[3]);
            verts.Add(rightQuad[0]);
            verts.Add(rightQuad[2]);

            normals.Add(Vector3.right);
            normals.Add(Vector3.right);
            normals.Add(Vector3.right);
            normals.Add(Vector3.right);

            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 1);

            indices.Add(vertexOffset + 1);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 3);

            vertexOffset += 4;
          }

          float bottomQuadHeight = heightMap[y + 1, x];
          int bottomQuadFirstPoint = (x + (y + 1) * VXL.instance.CHUNK_SIZE) * 4;
          Vector3[] bottomQuad = new[]
          {
                        verts[bottomQuadFirstPoint],
                        verts[bottomQuadFirstPoint + 1],
                        verts[bottomQuadFirstPoint + 2],
                        verts[bottomQuadFirstPoint + 3]
                    };

          if (bottomQuadHeight != currentQuadHeight)
          {
            verts.Add(currentQuad[0]);
            verts.Add(currentQuad[1]);
            verts.Add(bottomQuad[2]);
            verts.Add(bottomQuad[3]);

            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);

            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 1);

            indices.Add(vertexOffset + 1);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 3);

            vertexOffset += 4;
          }
        }
        else if (y == VXL.instance.CHUNK_SIZE - 1 && x != VXL.instance.CHUNK_SIZE - 1)
        {
          float rightQuadHeight = heightMap[y, x + 1];
          int rightQuadFirstPoint = (x + 1 + y * VXL.instance.CHUNK_SIZE) * 4;
          Vector3[] rightQuad = new[]
          {
                        verts[rightQuadFirstPoint],
                        verts[rightQuadFirstPoint + 1],
                        verts[rightQuadFirstPoint + 2],
                        verts[rightQuadFirstPoint + 3]
                    };

          if (rightQuadHeight != currentQuadHeight)
          {
            verts.Add(currentQuad[1]);
            verts.Add(currentQuad[3]);
            verts.Add(rightQuad[0]);
            verts.Add(rightQuad[2]);

            normals.Add(Vector3.right);
            normals.Add(Vector3.right);
            normals.Add(Vector3.right);
            normals.Add(Vector3.right);

            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 1);

            indices.Add(vertexOffset + 1);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 3);

            vertexOffset += 4;
          }
        }
        else if (y != VXL.instance.CHUNK_SIZE - 1 && x == VXL.instance.CHUNK_SIZE - 1)
        {
          float bottomQuadHeight = heightMap[y + 1, x];
          int bottomQuadFirstPoint = (x + (y + 1) * VXL.instance.CHUNK_SIZE) * 4;
          Vector3[] bottomQuad = new[]
          {
                        verts[bottomQuadFirstPoint],
                        verts[bottomQuadFirstPoint + 1],
                        verts[bottomQuadFirstPoint + 2],
                        verts[bottomQuadFirstPoint + 3]
                    };

          if (bottomQuadHeight != currentQuadHeight)
          {
            verts.Add(currentQuad[0]);
            verts.Add(currentQuad[1]);
            verts.Add(bottomQuad[2]);
            verts.Add(bottomQuad[3]);

            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);
            normals.Add(Vector3.back);

            indices.Add(vertexOffset);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 1);

            indices.Add(vertexOffset + 1);
            indices.Add(vertexOffset + 2);
            indices.Add(vertexOffset + 3);

            vertexOffset += 4;
          }
        }
      }
    }

    Mesh mesh = new Mesh();
    mesh.Clear();
    mesh.vertices = verts.ToArray();
    mesh.triangles = indices.ToArray();
    mesh.normals = normals.ToArray();
    // mesh.Optimize();

    _gameObject.GetComponent<MeshFilter>().sharedMesh = mesh;
    _gameObject.GetComponent<MeshCollider>().sharedMesh = mesh;
  }

  private float[,] GenerateHeightMap(NoiseSettings settings)
  {
    float[,] heightMap = new float[VXL.instance.CHUNK_SIZE, VXL.instance.CHUNK_SIZE];

    for (int y = 0; y < VXL.instance.CHUNK_SIZE; y++)
    {
      for (int x = 0; x < VXL.instance.CHUNK_SIZE; x++)
      {
        heightMap[y, x] = GetElevationForPoint(x, y, settings);
      }
    }

    return heightMap;
  }

  private float GetElevationForPoint(int x, int y, NoiseSettings settings)
  {
    float baseOffset = VXL.instance.UNITY_NOISE_OFFSET;
    float baseFrequency = settings.baseFrequency;
    float basePersistance = settings.basePersistance;
    float elevation = 0;

    for (int i = 0; i < settings.octaves; i++)
    {

      //   float xSample = baseOffset + _position.x * VXL.instance.CHUNK_SIZE + x * baseFrequency;
      //   float ySample = baseOffset + _position.y * VXL.instance.CHUNK_SIZE + y * baseFrequency;

      float xSample = baseOffset + _position.x * VXL.instance.CHUNK_SIZE + x;
      float ySample = baseOffset + _position.y * VXL.instance.CHUNK_SIZE + y;

      elevation += basePersistance * Mathf.PerlinNoise(xSample * baseFrequency, ySample * baseFrequency);

      baseFrequency *= settings.frequencyMultiplier;
      basePersistance *= settings.basePersistance;
    }

    elevation = Mathf.Pow(elevation, settings.pow);

    if (settings.useRounding)
    {
      elevation = Mathf.Round(elevation);
    }

    return elevation;
  }

  public GameObject GameObject
  {
    get => _gameObject;
    set => _gameObject = value;
  }
}