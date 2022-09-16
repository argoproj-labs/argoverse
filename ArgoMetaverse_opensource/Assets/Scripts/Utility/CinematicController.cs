using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class CinematicController : MonoBehaviour
{
    private enum states { playing, paused, forward, rewind }

    [SerializeField] private PlayableDirector _director;
    private bool isPaused = false;
    [SerializeField] private TextMeshProUGUI _uiIndicator;
    private Coroutine uiFadeOutRoutine;
    private states currentState;

    private void Start()
    {
        Cursor.visible = false;
    }

    public void PressSkip(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        SceneManager.LoadScene(1);
    }
    public void PressForward(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        StateChange(states.forward);
        if (isPaused)
            PressPause(context);
        _director.time += 5f;
    }
    public void PressRewind(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        StateChange(states.rewind);
        if (isPaused)
            PressPause(context);
        _director.time -= 5f;
    }
    public void PressPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        if (!isPaused)
        {
            _director.Pause();
            StateChange(states.paused);
            isPaused = true;
        }
        else
        {
            _director.Play();
            if (currentState == states.paused)
                StateChange(states.playing);
            isPaused = false;
        }
    }

    private void StateChange(states next)
    {
        currentState = next;
        switch (next)
        {
            case states.playing:
                _uiIndicator.text = "Play";
                FadeUI(0.5f);
                break;
            case states.paused:
                _uiIndicator.text = "Pause";
                FadeUI(0.5f, true);
                break;
            case states.forward:
                _uiIndicator.text = ">>>>>";
                FadeUI(0.5f);
                break;
            case states.rewind:
                _uiIndicator.text = "<<<<<";
                FadeUI(0.5f);
                break;
            default:
                break;
        }
    }

    private void FadeUI(float duration, bool keepVisible = false)
    {
        _uiIndicator.color = Color.white;
        if (uiFadeOutRoutine != null)
            StopCoroutine(uiFadeOutRoutine);
        uiFadeOutRoutine = StartCoroutine(UIFadeOut(duration, keepVisible));
    }

    private IEnumerator UIFadeOut(float duration, bool keepVisible = false)
    {
        Color colour = _uiIndicator.color;
        yield return new WaitForSeconds(0.3f);
        while (_uiIndicator.color.a > 0f)
        {
            yield return new WaitForFixedUpdate();
            if (keepVisible)
                break;
            _uiIndicator.color = new Color(colour.r, colour.g, colour.b, colour.a - Time.fixedDeltaTime / duration);
            colour = _uiIndicator.color;
        }
        uiFadeOutRoutine = null;
    }
}
