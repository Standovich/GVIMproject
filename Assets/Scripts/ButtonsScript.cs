using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsScript : MonoBehaviour
{
    [SerializeField] private Sprite[] destroyButtonSprites;
    [SerializeField] private Image destroyButton;

    [SerializeField] private Sprite[] woodButtonSprites;
    [SerializeField] private Image woodButton;

    [SerializeField] private Sprite[] plankButtonSprites;
    [SerializeField] private Image plankButton;

    [SerializeField] private Sprite[] cobbleButtonSprites;
    [SerializeField] private Image cobbleButton;

    public void ToggleDestroyButton()
    {
        if(destroyButton.sprite == destroyButtonSprites[0])
        {
            destroyButton.sprite = destroyButtonSprites[1];
            return;
        }
        destroyButton.sprite = destroyButtonSprites[0];
    }

    public void ChangeSelectedBlock(int id)
    {
        switch (id)
        {
            case 0:
                woodButton.sprite = woodButtonSprites[1];
                plankButton.sprite = plankButtonSprites[0];
                cobbleButton.sprite = cobbleButtonSprites[0];
                break;
            case 1:
                plankButton.sprite = plankButtonSprites[1];
                woodButton.sprite = woodButtonSprites[0];
                cobbleButton.sprite = cobbleButtonSprites[0];
                break;
            case 2:
                cobbleButton.sprite = cobbleButtonSprites[1];
                woodButton.sprite = woodButtonSprites[0];
                plankButton.sprite = plankButtonSprites[0];
                break;
        }
    }
}
