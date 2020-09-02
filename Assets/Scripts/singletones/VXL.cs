public class VXL
{
  public static VXL instance = new VXL();

  public int CHUNK_SIZE = 16;
  public int RENDER_DISTANCE = 14;


  public float NOISE_BASE_FREQUENCY = 3.0f;
  public int NOISE_OCTAVES = 3;
  public float NOISE_ROGHFNESS = 0.2f;
  public int MAX_FLOOR_HEIGHT = 10;
  public float UNITY_NOISE_OFFSET = 999999.0f;
}
