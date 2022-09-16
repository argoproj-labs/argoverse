using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPopup : MonoBehaviour
{
    public static BoardPopup Instance;

    [SerializeField] Transform frame, content;
    [SerializeField] Material contentRenderer;

    private Coroutine currentAnimation;

    private Transform _lastOrigin, _newOrigin;
    private bool returning, active;

    [SerializeField] float _duration = 1f, _returnDuration = 0.5f;

    private float _timestamp;

    #region Animation
    public void ClickOnThis(Transform newOrigin, Texture _newSprite)
    {
        if (_timestamp > Time.time - 0.2f)
            return;
        _timestamp = Time.time;

        _newOrigin = newOrigin;
        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(AnimationRoutine(_newSprite));
    }

    public bool Unclick()
    {
        if (_timestamp > Time.time - 0.2f)
            return false;
        _timestamp = Time.time;

        if (currentAnimation != null)
            StopCoroutine(currentAnimation);
        currentAnimation = StartCoroutine(ReturnAnimation());
        return true;
    }

    private IEnumerator ReturnAnimation()
    {
        transform.parent = null;
        float progress = 0f;
        float progress1;
        float progress2;
        Vector3 initialPos;
        Vector3 initialScale1;
        Vector3 initialScale2;
        Quaternion initialRot;

        if (active && _lastOrigin != null)
        {
            returning = true;
            initialPos = transform.position;
            initialScale1 = frame.localScale;
            initialScale2 = content.localScale;
            initialRot = transform.rotation;

            while (progress < 1f)
            {
                yield return new WaitForFixedUpdate();
                progress = Mathf.Clamp01(progress + Time.fixedDeltaTime / _returnDuration);
                float overallprogress = EasingTool.InOutSine(progress);
                transform.position = Vector3.Lerp(initialPos, _lastOrigin.position, overallprogress);
                transform.rotation = Quaternion.Slerp(initialRot, _lastOrigin.rotation, overallprogress);
                progress1 = Mathf.Clamp01(EasingTool.Remap(progress, 0.3f, 1.0f, 0f, 1f));
                progress1 = EasingTool.InQuad(progress1);
                frame.localScale = Vector3.Lerp(initialScale1, Vector3.zero, progress1);
                progress2 = Mathf.Clamp01(EasingTool.Remap(progress, 0.0f, 0.7f, 0f, 1f));
                progress2 = EasingTool.InQuad(progress2);
                content.localScale = Vector3.Lerp(initialScale2, Vector3.zero, progress2);
            }
        }
        active = false;
        returning = false;
        currentAnimation = null;
    }

    private IEnumerator AnimationRoutine(Texture _newSprite)
    {
        transform.parent = null;
        float progress = 0f;
        float progress1;
        float progress2;
        Vector3 initialPos;
        Vector3 initialScale1;
        Vector3 initialScale2;
        Quaternion initialRot;

        if (active && _lastOrigin != null)
        {
            returning = true;
            initialPos = transform.position;
            initialScale1 = frame.localScale;
            initialScale2 = content.localScale;
            initialRot = transform.rotation;

            while (progress < 1f)
            {
                yield return new WaitForFixedUpdate();
                progress = Mathf.Clamp01(progress + Time.fixedDeltaTime / _returnDuration);
                float overallprogress = EasingTool.InOutSine(progress);
                transform.position = Vector3.Lerp(initialPos, _lastOrigin.position, overallprogress);
                transform.rotation = Quaternion.Slerp(initialRot, _lastOrigin.rotation, overallprogress);
                progress1 = Mathf.Clamp01(EasingTool.Remap(progress, 0.3f, 1.0f, 0f, 1f));
                progress1 = EasingTool.InQuad(progress1);
                frame.localScale = Vector3.Lerp(initialScale1, Vector3.zero, progress1);
                progress2 = Mathf.Clamp01(EasingTool.Remap(progress, 0.0f, 0.7f, 0f, 1f));
                progress2 = EasingTool.InQuad(progress2);
                content.localScale = Vector3.Lerp(initialScale2, Vector3.zero, progress2);
            }
        }

        active = true;
        returning = false;
        _lastOrigin = _newOrigin;

        progress = 0f;
        transform.position = _newOrigin.position;
        transform.rotation = _newOrigin.rotation;
        frame.localScale = Vector3.zero;
        content.localScale = Vector3.zero;
        contentRenderer.SetTexture("_MainTex", _newSprite);

        Transform pTr = Player.Instance.screenProjectionTransform;

        while (progress < 1f)
        {
            yield return new WaitForFixedUpdate();
            progress = Mathf.Clamp01(progress + Time.fixedDeltaTime / _duration);
            float overallprogress = EasingTool.InOutSine(progress);
            transform.position = Vector3.Lerp(_newOrigin.position, pTr.position, overallprogress);
            transform.rotation = Quaternion.Slerp(_newOrigin.rotation, pTr.rotation, overallprogress);
            progress1 = Mathf.Clamp01(EasingTool.Remap(progress, 0f, 0.7f, 0f, 1f));
            progress1 = EasingTool.OutQuad(progress1);
            frame.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress1);
            progress2 = Mathf.Clamp01(EasingTool.Remap(progress, 0.3f, 1.0f, 0f, 1f));
            progress2 = EasingTool.OutQuad(progress2);
            content.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, progress2);
        }
        transform.parent = pTr;
        currentAnimation = null;
    }
    #endregion

    #region UnityFuncs
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        transform.parent = null;
        frame.localScale = Vector3.zero;
        content.localScale = Vector3.zero;
    }
    #endregion
}
