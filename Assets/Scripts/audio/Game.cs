using UnityEngine;

public class Game : MonoBehaviour {
  public AudioManager sndPlayer;

  public static Game globalInstance;

  void Start() {
    globalInstance = this;
  }

  void Update() {

  }
}
