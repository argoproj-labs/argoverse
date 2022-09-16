using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _minimapGO;
    [SerializeField] private GameObject _pauseMenuGO;
    [SerializeField] private Player _player;
    [SerializeField] private InputManager _playerInput;
    [SerializeField] private GravitySource _planet;

    [SerializeField] private Slider _moveSpeed;
    [SerializeField] private Slider _jumpHeight;
    [SerializeField] private Toggle _doubleJump;
    [SerializeField] private Slider _throwStrength;
    [SerializeField] private Toggle _minimap;
    [SerializeField] private Slider _gravity;

    public bool isPaused { get; private set; } = false;

    private void Start()
    {
        Time.timeScale = 1f;
        _minimapGO.SetActive(_minimap.isOn);
        UpdateSliderValues();
        _pauseMenuGO.SetActive(false);
        _player = Player.Instance;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }

    public void ClickPause()
    {
        isPaused = !isPaused;
        _pauseMenuGO.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
        if (isPaused)
            _playerInput.Deactivate();
        else
            _playerInput.Activate();
    }

    public void UpdateSliderValues()
    {
        _moveSpeed.value = _player.maxSpeed;
        _jumpHeight.value = _player.jumpForce;
        _doubleJump.isOn = _player.doubleJumpAllowed;
        _throwStrength.value = _player.throwStrength;
        _gravity.value = _planet.Gravity;
    }

    public void ClickSpeed() => _player.maxSpeed = _moveSpeed.value;
    public void ClickJump() => _player.jumpForce = _jumpHeight.value;
    public void ClickDoubleJ() => _player.doubleJumpAllowed = _doubleJump.isOn;
    public void ClickThrow() => _player.throwStrength = _throwStrength.value;
    public void ClickMinimap() => _minimapGO.SetActive(_minimap.isOn);
    public void ClickGravity() => _planet._gravityForce = _gravity.value;

    public void ClickQuit() => SceneManager.LoadScene(1); //1 is main menu
}
