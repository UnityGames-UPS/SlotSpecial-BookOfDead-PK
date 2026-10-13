using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardFlip : MonoBehaviour
{
    [SerializeField] internal Sprite cardImage;
    [SerializeField] internal Button Card_Button;
    [SerializeField] internal Image Card_Img;

    [SerializeField] private SocketIOManager SocketManager;
    [SerializeField] private GambleController gambleController;

    private RectTransform Card_transform;

    internal bool once = false;

    private void Start()
    {
        Card_transform = gameObject.GetComponent<RectTransform>();
        // if (Card_Button) Card_Button.onClick.RemoveAllListeners();
        // if (Card_Button) Card_Button.onClick.AddListener(FlipMainCard);
    }

    internal void FlipMyObject(Sprite img)
    {

        Card_transform.localEulerAngles = new Vector3(0, 180, 0);
        Card_transform.DOLocalRotate(Vector3.zero, 1, RotateMode.FastBeyond360);
        once = true;
        cardImage = img;
        DOVirtual.DelayedCall(0.3f, changeSprite);

    }

    // private void FlipMainCard()
    // {
    //     gambleController.AllCardToggle(false);
    //     StartCoroutine(FlipMainObject());
    // }

    // private IEnumerator FlipMainObject()
    // {
    //     //gambleController.RunOnCollect();

    //     // SocketManager.GambleDraw();
    //     yield return new WaitUntil(() => SocketManager.isResultdone);
    //     gambleController.ComputeCards(); // Compute card sprites
    //     cardImage = gambleController.GetCard();
    //     //  FlipMyObject();
    //     yield return null;
    // }

    private void changeSprite()
    {

        Card_Img.sprite = cardImage;



    }

    internal void Reset()
    {
        Card_Button.interactable = true;
        once = false;
    }
}
