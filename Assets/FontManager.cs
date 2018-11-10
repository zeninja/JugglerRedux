using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FontManager : MonoBehaviour {

    #region instance
    private static FontManager instance;
    public static FontManager GetInstance()
    {
        return instance;
    }
    #endregion

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

	public TMP_FontAsset inGame, postGame;
	public TextMeshPro[] texts;

	public void SetFont(bool isInGame) {
		for(int i = 0; i < texts.Length; i++) {
			if(isInGame) {
				texts[i].font = inGame;
			} else {
				texts[i].font = postGame;
			}
		}
	}
}
