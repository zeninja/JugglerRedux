using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{

    public string label;
    public Image image;
    public TextMeshPro text;
    public Image innerBlock;
    public Image outerEdge;

    public enum ButtonState { prep, on, off, click, uninteractable };
    public ButtonState state;
    [System.Serializable]
    public class ButtonSetting
    {
        public Vector2 position;
        public Color text, innerBlock, outerBlock;
    }
    public Transform button;
    ButtonSetting targetSetting;

    public ButtonSetting prep, on, off, click, uninteractable;

    void Start()
    {
    }

    void OnEnable() {
        InitBounce();
    }

    public bool interactable;

   public void InitBounce() {
        if (!interactable) {
            SetSettings(ButtonState.uninteractable);
        } else {
            SetSettings(ButtonState.prep);
        }

        text.text = label;
        targetSetting = off;
   		button.transform.localPosition = targetSetting.position;

        UpdateSettings();

        StartCoroutine(AnimateButton(targetSetting));
   }

   public void SetButtonState(bool isOn, bool interactable = true) {

        currentValue = isOn;
        if(currentValue) {
            SetSettings(ButtonState.on);
        } else {
            SetSettings(ButtonState.off);
        }

        if (!interactable) {
           SetSettings(ButtonState.uninteractable);
        }

        text.text = label;
   		button.transform.localPosition = targetSetting.position;

        UpdateSettings();

        StartCoroutine(AnimateButton(targetSetting));
   }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {	
			GetSetting();
            StartCoroutine(AnimateButton(targetSetting));
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int index = (int)state;
            index = ((int)state + 1) % 5;
            state = (ButtonState)index;
			GetSetting();
        }
    }

	public int clickDelay;

    public float moveToClickDuration = .125f;
    public float moveToSettingDuration = .125f;

    IEnumerator AnimateButton(ButtonSetting setting)
    {
        yield return StartCoroutine(MoveButtonToPosition(click.position, moveToClickDuration));
		yield return StartCoroutine(Extensions.Wait(clickDelay * Time.fixedDeltaTime));
		UpdateSettings(setting);
        yield return StartCoroutine(MoveButtonToPosition(setting.position, moveToSettingDuration));
    }

    IEnumerator MoveButtonToPosition(Vector2 target, float d = .25f)
    {
        float t = 0;

        Vector2 origin = button.transform.localPosition;
        Vector2 diff = target - origin;

        while (t < d)
        {
            t += Time.fixedDeltaTime;
            float p = t / d;
            button.localPosition = origin + diff * EZEasings.SmoothStop3(p);
            yield return new WaitForFixedUpdate();
        }
   }

	void UpdateSettings() {
		text.color       = targetSetting.text;
		innerBlock.color = targetSetting.innerBlock;
		outerEdge.color  = targetSetting.outerBlock;

        if(image != null) { image.color = targetSetting.text; }
	}

	void UpdateSettings(ButtonSetting newSetting) {
		targetSetting	 = newSetting;
		text.color       = targetSetting.text;
		innerBlock.color = targetSetting.innerBlock;
		outerEdge.color  = targetSetting.outerBlock;

        if(image != null) { image.color = targetSetting.text; }
	}

    // Play Button Sound based on single sound
    // public AudioManager.Sound buttonSound;
    // bool ready = false;

    // void PlayButtonSound() {
    //     if(!ready) { return; }
    //     switch (buttonSound) {
    //         case AudioManager.Sound.select:
    //             EventManager.TriggerEvent("Select", true);
    //             break;
    //         case AudioManager.Sound.undo:
    //             EventManager.TriggerEvent("Undo", true);
    //             break;
    //     }
    // }

    void PlayButtonSound(AudioManager.ButtonSound sound) {
        switch(sound) {
            case AudioManager.ButtonSound.select:
                AudioManager.PlaySelect();
                break;
            case AudioManager.ButtonSound.undo:
                AudioManager.PlayUndo();
                break;
            case AudioManager.ButtonSound.bounce:
                AudioManager.PlayBounce();
                break;
        }
    }

    public bool currentValue;

    public void InvertValue() {
        InvertState();
        StartCoroutine(AnimateButton(targetSetting));
    }

    void InvertState() {
        currentValue = !currentValue;

        if(currentValue) {
            SetSettings(ButtonState.on);
            PlayButtonSound(AudioManager.ButtonSound.select);
        } else {
            SetSettings(ButtonState.off);
            PlayButtonSound(AudioManager.ButtonSound.undo);
        }
    }

    public void Bounce() {
        SetSettings(ButtonState.off);
        StartCoroutine(AnimateButton(targetSetting));
        PlayButtonSound(AudioManager.ButtonSound.bounce);
    }

	public void SetSettings(ButtonState newState) {
		state = newState;

        switch (state)
        {
            case ButtonState.on:
                targetSetting = on;
                currentValue = true;
                break;
            case ButtonState.off:
                targetSetting = off;
                currentValue = false;
                break;
            case ButtonState.prep:
                targetSetting = prep;
                break;
            case ButtonState.uninteractable:
                targetSetting = uninteractable;
                interactable = false;
                break;
        }
	}


	// Debug
    void GetSetting()
    {
        switch (state)
        {
            case ButtonState.on:
                targetSetting = on;
                break;
            case ButtonState.off:
                targetSetting = off;
                break;
            case ButtonState.prep:
                targetSetting = prep;
                break;
            case ButtonState.uninteractable:
                targetSetting = uninteractable;
                break;
        }
    }
}