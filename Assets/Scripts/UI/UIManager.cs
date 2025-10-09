using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using System;


public class UIManager : MonoBehaviour
{

    [Header("Menu UI")]
    [SerializeField]
    private Button Info_Button;

    [Header("Popus UI")]
    [SerializeField]
    private GameObject MainPopup_Object;
    [SerializeField]
    private GameObject PayPopup_Object;


    [Header("info Popup")]
    [SerializeField]
    private GameObject PaytablePopup_Object;
    [SerializeField]
    private Button PaytableExit_Button;
    [SerializeField]
    private Button Next_Button;
    [SerializeField]
    private Button Previous_Button;
    private int paginationCounter = 0;
    [SerializeField] private GameObject[] PageList;
    [SerializeField] private GameObject[] LightList;
    [SerializeField] private Button InfoButton;
    [SerializeField] private Button Infoback_button;
    [SerializeField]
    private TMP_Text[] SymbolsText;
    [SerializeField]
    private TMP_Text FreeSpin_Text;
    [SerializeField]
    private TMP_Text Scatter_Text;

    [SerializeField] private TMP_Text Bonus_Text;

    [SerializeField] private TMP_Text Wild_Text;

    [Header("Settings Popup")]
    [SerializeField] private Button Setting_button;
    [SerializeField] private Button SettingExit_button;
    [SerializeField] private Button Setting_back_button;
    [SerializeField] private GameObject Setting_panel;
    [SerializeField] private Button Sound_slider;
    [SerializeField] private GameObject SoundOn;
    [SerializeField] private GameObject SoundOFF;

    [SerializeField] private Button Music_slider;
    [SerializeField] private GameObject MusicOn;
    [SerializeField] private GameObject MusicOFF;

    [Header("Splash Screen")]
    [SerializeField]
    private GameObject Loading_Object;
    [SerializeField]
    private Image Loading_Image;
    [SerializeField]
    private TMP_Text LoadPercent_Text;
    [SerializeField]
    private TMP_Text Loading_Text;

    [Header("LowBalance Popup")]
    [SerializeField]
    private Button LBExit_Button;
    [SerializeField]
    private Button LBBack_Button;
    [SerializeField]
    private GameObject LBPopup_Object;

    [Header("Disconnection Popup")]
    [SerializeField]
    private Button CloseDisconnect_Button;
    [SerializeField]
    private GameObject DisconnectPopup_Object;

    [Header("Reconection Popup")]
    [SerializeField]
    private GameObject ReconectingPopup_Object;

    [Header("AnotherDevice Popup")]
    [SerializeField]
    private Button CloseAD_Button;
    [SerializeField]
    private GameObject ADPopup_Object;

    [Header("Quit Popup")]
    [SerializeField]
    private GameObject QuitPopup_Object;
    [SerializeField]
    private Button YesQuit_Button;
    [SerializeField]
    private Button NoQuit_Button;
    [SerializeField]
    private Button CrossQuit_Button;
    [SerializeField]
    private Button BackQuit_Button;

    [Header("Megawin Popup")]
    [SerializeField] private GameObject megawin;
    [SerializeField] private TMP_Text megawin_text;
    [SerializeField] private TMP_Text HederMiscWin;
    [SerializeField] private ImageAnimation Win_Image;
    [SerializeField] private Sprite HugeWin_Sprite;
    [SerializeField] private Sprite BigWin_Sprite;
    [SerializeField] private Sprite MegaWin_Sprite;
    [SerializeField] private Sprite Scater_Sprite;
    [SerializeField] private Button MegaWinHideBtn;
    private bool music = true;
    private bool sound = true;

    [Header("FreeSpins Popup")]
    [SerializeField]
    private GameObject FreeSpinPopup_Object;
    [SerializeField]
    private TMP_Text Free_Text;
    [SerializeField]
    private Button FreeSpin_Button;

    //[Header("gamble game")]
    //[SerializeField] private Button Gamble_button;
    //[SerializeField] private Button GambleExit_button;
    //[SerializeField] private GameObject Gamble_game;

    [Header("Audio")]
    [SerializeField] private AudioController audioController;

    [SerializeField]
    private Button GameExit_Button;

    [SerializeField]
    private Button GameExitSplash_Button;

    [SerializeField]
    private Button GameExitBonus_Button;

    [SerializeField]
    private SlotBehaviour slotManager;

    [SerializeField]
    private SocketIOManager socketManager;

    private bool isExit = false;

    internal int FreeSpins;

    [SerializeField] internal GameObject RaycastBlocker;

    [SerializeField] private Button m_AwakeGameButton;

    private void Awake()
    {
        // if (Loading_Object) Loading_Object.SetActive(true);
        // StartCoroutine(LoadingRoutine());
    }


    private IEnumerator LoadingRoutine()
    {
        float imageFill = 0f;
        DOTween.To(() => imageFill, (val) => imageFill = val, 0.7f, 2f).OnUpdate(() =>
        {
            if (Loading_Image) Loading_Image.fillAmount = imageFill;
            if (LoadPercent_Text) LoadPercent_Text.text = (100 * imageFill).ToString("f0") + "%";
        });
        yield return new WaitForSecondsRealtime(2);
        yield return new WaitUntil(() => socketManager.isLoaded);
        DOTween.To(() => imageFill, (val) => imageFill = val, 1, 1f).OnUpdate(() =>
        {
            if (Loading_Image) Loading_Image.fillAmount = imageFill;
            if (LoadPercent_Text) LoadPercent_Text.text = (100 * imageFill).ToString("f0") + "%";
        });
        yield return new WaitForSecondsRealtime(1f);
        if (Loading_Object) Loading_Object.SetActive(false);
    }



    private void Start()
    {
        if (Info_Button) Info_Button.onClick.RemoveAllListeners();
        if (Info_Button) Info_Button.onClick.AddListener(delegate { OpenPopup(PaytablePopup_Object); });

        if (PaytableExit_Button) PaytableExit_Button.onClick.RemoveAllListeners();
        if (PaytableExit_Button) PaytableExit_Button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });

        if (Next_Button) Next_Button.onClick.RemoveAllListeners();
        if (Next_Button) Next_Button.onClick.AddListener(delegate { TurnPage(true); });

        if (Previous_Button) Previous_Button.onClick.RemoveAllListeners();
        if (Previous_Button) Previous_Button.onClick.AddListener(delegate { TurnPage(false); });

        //   if (Previous_Button) Previous_Button.interactable = false;

        if (Infoback_button) Infoback_button.onClick.RemoveAllListeners();
        if (Infoback_button) Infoback_button.onClick.AddListener(delegate { ClosePopup(PaytablePopup_Object); });


        if (Setting_button) Setting_button.onClick.RemoveAllListeners();
        if (Setting_button) Setting_button.onClick.AddListener(delegate { OpenPopup(Setting_panel); });

        if (Sound_slider) Sound_slider.onClick.RemoveAllListeners();
        if (Sound_slider) Sound_slider.onClick.AddListener(delegate { ChangeSound(); });

        if (Music_slider) Music_slider.onClick.RemoveAllListeners();
        if (Music_slider) Music_slider.onClick.AddListener(delegate { ChangeMusic(); });

        if (FreeSpin_Button) FreeSpin_Button.onClick.RemoveAllListeners();
        if (FreeSpin_Button) FreeSpin_Button.onClick.AddListener(delegate { StartFreeSpins(FreeSpins); });

        if (SettingExit_button) SettingExit_button.onClick.RemoveAllListeners();
        if (SettingExit_button) SettingExit_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

        if (Setting_back_button) Setting_back_button.onClick.RemoveAllListeners();
        if (Setting_back_button) Setting_back_button.onClick.AddListener(delegate { ClosePopup(Setting_panel); });

        if (MegaWinHideBtn) MegaWinHideBtn.onClick.RemoveAllListeners();
        if (MegaWinHideBtn) MegaWinHideBtn.onClick.AddListener(OnClickMegaWinHide);
        //if (Gamble_button) Gamble_button.onClick.RemoveAllListeners();
        //if (Gamble_button) Gamble_button.onClick.AddListener(delegate { OpenPopup(Gamble_game); });

        //if (GambleExit_button) GambleExit_button.onClick.RemoveAllListeners();
        //if (GambleExit_button) GambleExit_button.onClick.AddListener(delegate { ClosePopup(Gamble_game); });

        if (GameExit_Button) GameExit_Button.onClick.RemoveAllListeners();
        if (GameExit_Button) GameExit_Button.onClick.AddListener(delegate { OpenPopup(QuitPopup_Object); });

        if (GameExitSplash_Button) GameExitSplash_Button.onClick.RemoveAllListeners();
        if (GameExitSplash_Button) GameExitSplash_Button.onClick.AddListener(delegate { if (!isExit) { OpenPopup(QuitPopup_Object); } });

        if (GameExitBonus_Button) GameExitBonus_Button.onClick.RemoveAllListeners();
        if (GameExitBonus_Button) GameExitBonus_Button.onClick.AddListener(delegate { if (!isExit) { OpenPopup(QuitPopup_Object); } });

        if (NoQuit_Button) NoQuit_Button.onClick.RemoveAllListeners();
        if (NoQuit_Button) NoQuit_Button.onClick.AddListener(delegate { ClosePopup(QuitPopup_Object); });

        if (CrossQuit_Button) CrossQuit_Button.onClick.RemoveAllListeners();
        if (CrossQuit_Button) CrossQuit_Button.onClick.AddListener(delegate { if (!isExit) { ClosePopup(QuitPopup_Object); } });

        if (BackQuit_Button) BackQuit_Button.onClick.RemoveAllListeners();
        if (BackQuit_Button) BackQuit_Button.onClick.AddListener(delegate { if (!isExit) { ClosePopup(QuitPopup_Object); } });

        if (LBExit_Button) LBExit_Button.onClick.RemoveAllListeners();
        if (LBExit_Button) LBExit_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

        if (LBBack_Button) LBBack_Button.onClick.RemoveAllListeners();
        if (LBBack_Button) LBBack_Button.onClick.AddListener(delegate { ClosePopup(LBPopup_Object); });

        if (YesQuit_Button) YesQuit_Button.onClick.RemoveAllListeners();
        if (YesQuit_Button) YesQuit_Button.onClick.AddListener(CallOnExitFunction);

        if (CloseAD_Button) CloseAD_Button.onClick.RemoveAllListeners();
        if (CloseAD_Button) CloseAD_Button.onClick.AddListener(CallOnExitFunction);

        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.RemoveAllListeners();
        if (CloseDisconnect_Button) CloseDisconnect_Button.onClick.AddListener(delegate { CallOnExitFunction(); socketManager.closeSocketReactnativeCall(); });

    }

    internal void LowBalPopup()
    {
        OpenPopup(LBPopup_Object);
    }

    internal void DisconnectionPopup()
    {
        //if (isReconnection)
        //{
        //    OpenPopup(ReconnectPopup_Object);
        //}
        //else
        //{
        //    ClosePopup(ReconnectPopup_Object);
        if (!isExit)
        {
            OpenPopup(DisconnectPopup_Object);
        }
        //}
    }

    private void StartFreeSpins(int spins)
    {
        Debug.Log("DevTest" + "here4");
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
        if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(false);
        slotManager.FreeSpin(spins);
    }

    internal void FreeSpinProcess(int spins)
    {
        Debug.Log("DevTest" + "here3");
        int ExtraSpins = spins - FreeSpins;
        FreeSpins = spins;


        if (FreeSpinPopup_Object) FreeSpinPopup_Object.SetActive(true);
        if (Free_Text) Free_Text.text = "You are awarded with " + ExtraSpins.ToString() + " extra free spins.";
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        DOVirtual.DelayedCall(2f, () =>
        {
            StartFreeSpins(spins);
        });
    }

    internal void PopulateWin(int type, double amount)
    {
        if (megawin_text)
        {
            // Make text initially transparent
            var color = megawin_text.color;
            color.a = 0f;
            megawin_text.color = color;
        }
        double initAmount = 0;
        double originalAmount = amount;
        HederMiscWin.gameObject.SetActive(false);
        switch (type)
        {
            case 1:
                if (HederMiscWin) HederMiscWin.text = "Big Win";
                break;
            case 2:
                if (HederMiscWin) HederMiscWin.text = "Huge Win";
                break;
            case 3:
                if (HederMiscWin) HederMiscWin.text = "Mega Win";
                break;
            case 4:
                if (HederMiscWin) HederMiscWin.text = "Total Win";
                break;
        }
        if (megawin) megawin.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
        Win_Image.StartAnimation();


        StartCoroutine(ShowMegaWin(amount));
        // // Animate the number and fade-in together
        // DOTween.To(() => initAmount, x => initAmount = x, amount, 1f)
        //     .OnUpdate(() =>
        //     {
        //         if (megawin_text)
        //         {
        //             // Update the number
        //             megawin_text.text = initAmount.ToString("F2");
        //         }
        //     });

        // // Fade-in text
        // if (megawin_text)
        // {
        //     megawin_text.DOFade(1f, 1f).From(0f);
        // }

        // DOVirtual.DelayedCall(3.5f, OnClickMegaWinHide);
    }
    IEnumerator ShowMegaWin(double amount)
    {
        float duration = 1f;      // animation duration
        float displayTime = 3.5f; // how long to stay before hiding
        float initAmount = 0f;
        float timer = 0f;

        if (megawin_text)
        {
            // Start fully transparent
            megawin_text.alpha = 0f;
            megawin_text.gameObject.SetActive(false);
            yield return new WaitForSeconds(1f);
            megawin_text.gameObject.SetActive(true);
            HederMiscWin.gameObject.SetActive(true);
            // Fade in text
            megawin_text.DOFade(1f, duration).From(0f);

            // Animate the number manually over time
            while (timer < duration)
            {
                timer += Time.deltaTime;
                float t = Mathf.Clamp01(timer / duration);

                // Use float for interpolation, then cast back to double for precision
                double current = Mathf.Lerp(initAmount, (float)amount, t);
                megawin_text.text = current.ToString("F2");

                yield return null;
            }

            // Ensure final value is exact
            megawin_text.text = amount.ToString("F2");

            // Wait for display time (not using DOTween)
            yield return new WaitForSeconds(displayTime);

            // Hide after delay
            OnClickMegaWinHide();
        }
    }


    private void OnClickMegaWinHide()
    {
        if (MainPopup_Object) MainPopup_Object.SetActive(false);
        if (megawin) megawin.SetActive(false);
        if (megawin_text) megawin_text.text = "0";
        slotManager.CheckPopups = false;
    }
    internal void ADfunction()
    {
        OpenPopup(ADPopup_Object);
    }

    private void CallOnExitFunction()
    {
        isExit = true;
        audioController.PlayButtonAudio();
        slotManager.CallCloseSocket();
        // Application.ExternalCall("window.parent.postMessage", "onExit", "*");
    }

    internal void InitialiseUIData(Paylines symbolsText)
    {
        PopulateSymbolsPayout(symbolsText);
    }

    private void PopulateSymbolsPayout(Paylines paylines)
    {
        double multiplyer = socketManager.InitialData.bets[slotManager.BetCounter];
        for (int i = 0; i < SymbolsText.Length; i++)
        {
            string text = null;
            if (paylines.symbols[i].multiplier[0] != 0)
            {
                text += "5x - " + paylines.symbols[i].multiplier[0] * multiplyer;
            }
            if (paylines.symbols[i].multiplier[1] != 0)
            {
                text += "\n4x - " + paylines.symbols[i].multiplier[1] * multiplyer;
            }
            if (paylines.symbols[i].multiplier[2] != 0)
            {
                text += "\n3x - " + paylines.symbols[i].multiplier[2] * multiplyer;
            }
            if (SymbolsText[i]) SymbolsText[i].text = text;
        }

        for (int i = 0; i < paylines.symbols.Count; i++)
        {
            if (paylines.symbols[i].name.ToUpper() == "FREESPIN")
            {
                if (FreeSpin_Text) FreeSpin_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].name.ToUpper() == "SCATTER")
            {
                if (Scatter_Text) Scatter_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].name.ToUpper() == "JACKPOT")
            {
                // if (Jackpot_Text) Jackpot_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].name.ToUpper() == "BONUS")
            {
                if (Bonus_Text) Bonus_Text.text = paylines.symbols[i].description.ToString();
            }
            if (paylines.symbols[i].name.ToUpper() == "WILD")
            {
                if (Wild_Text) Wild_Text.text = paylines.symbols[i].description.ToString();
            }
        }



    }
    internal void ReconnectionPopup()
    {
        OpenPopup(ReconectingPopup_Object);
    }

    private void OpenPopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();
        if (Popup) Popup.SetActive(true);
        if (MainPopup_Object) MainPopup_Object.SetActive(true);
    }
    internal void CheckAndClosePopups()
    {

        if (ReconectingPopup_Object.activeInHierarchy)
        {
            ClosePopup(ReconectingPopup_Object);
        }
        if (DisconnectPopup_Object.activeInHierarchy)
        {
            ClosePopup(DisconnectPopup_Object);
        }
    }


    private void ClosePopup(GameObject Popup)
    {
        if (audioController) audioController.PlayButtonAudio();

        if (Popup) Popup.SetActive(false);
        if (!DisconnectPopup_Object.activeSelf)
        {
            if (MainPopup_Object) MainPopup_Object.SetActive(false);
        }
    }

    private void TurnPage(bool type)
    {
        if (audioController) audioController.PlayButtonAudio();

        if (type)
        {
            if (paginationCounter < 2)
            {

                PageList[paginationCounter].SetActive(false);
                LightList[paginationCounter].SetActive(false);
                paginationCounter++;
            }

        }
        else
        {
            if (paginationCounter > 0)
            {

                PageList[paginationCounter].SetActive(false);
                LightList[paginationCounter].SetActive(false);

                paginationCounter--;
            }
        }


        LightList[paginationCounter].SetActive(true);

        PageList[paginationCounter].SetActive(true);


    }

    // private void GoToPage(int index)
    // {

    //     paginationCounter = index + 1;

    //     paginationCounter = Mathf.Clamp(paginationCounter, 1, 6);

    //     if (Next_Button) Next_Button.interactable = !(paginationCounter >= 6);

    //     if (Previous_Button) Previous_Button.interactable = !(paginationCounter <= 1);

    //     for (int i = 0; i < PageList.Length; i++)
    //     {
    //         PageList[i].SetActive(false);
    //     }

    //     for (int i = 0; i < paginationButtonGrp.Length; i++)
    //     {
    //         paginationButtonGrp[i].interactable = true;
    //         paginationButtonGrp[i].transform.GetChild(0).gameObject.SetActive(false);
    //     }

    //     PageList[paginationCounter - 1].SetActive(true);
    //     paginationButtonGrp[paginationCounter - 1].interactable = false;
    //     paginationButtonGrp[paginationCounter - 1].transform.GetChild(0).gameObject.SetActive(true);
    // }

    private void ChangeSound()
    {
        if (sound)
        {
            sound = false;

        }
        else
        {
            sound = true;
        }

        SoundOFF.SetActive(!sound);
        SoundOn.SetActive(sound);
        audioController.ToggleMute(!sound, "wl");
        audioController.ToggleMute(!sound, "button");
    }

    private void ChangeMusic()
    {
        if (music)
        {
            music = false;

        }
        else
        {
            music = true;
        }

        MusicOFF.SetActive(!music);
        MusicOn.SetActive(music);
        audioController.ToggleMute(!music, "bg");

    }
}
