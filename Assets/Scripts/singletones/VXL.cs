public class VXL
{
  public static VXL instance = new VXL();


  public Player player;

  public void SetPlayer(Player _player)
  {
    if (player == null)
    {
      player = _player;
    }
  }
}
