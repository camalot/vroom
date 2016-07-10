using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LapManager : MonoBehaviour
{
  public Text LapText;

  public int TotalLaps { get; set; }

  public int CurrentLap { get; set; }

  public LapManager()
  {
    this.CurrentLap = 0;
    this.TotalLaps = 5;
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  public void UpdateLap(int currentLap)
  {
    this.CurrentLap = currentLap;
    this.LapText.text = string.Format("{0} of {1}", (object) currentLap, (object) this.TotalLaps);
    if (currentLap < this.TotalLaps + 1)
      return;
    this.GameOver();
  }

  public void GameOver()
  {
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }
}
