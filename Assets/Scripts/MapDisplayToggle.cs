using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MapDisplayToggle : MonoBehaviour
{
    public GameObject theWholeLevel;
    public GameObject visualAssetsSprites;
    public GameObject animationsAndAnimators;
    public GameObject manualLevelLayout;
    public Button manualMapButton;
    public Button playButton;  
    public Button mapDisplayButton;
    public TextMeshProUGUI animationText;  
    public TextMeshProUGUI visualAssetsText;  
    public TextMeshProUGUI topLeftCornerPieceText;
    public Button backButton;
    public GameObject levelContainer;
    public Button procedurallevelButton;
    void Start()
    {
        if (manualMapButton != null)
        {
            manualMapButton.onClick.AddListener(ToggleMapDisplay);
        }
        else
        {
            Debug.LogError("Manual Map Button is not assigned.");
        }

        if (backButton != null)
        {
            backButton.onClick.AddListener(ReturnToPreviousView); 
            backButton.gameObject.SetActive(false);  
        }
        else
        {
            Debug.LogError("Back Button is not assigned.");
        }

        // Initially, set 'theWholeLevel' to inactive, and others to active.
        SetObjectsActive(false, true, true, true, true, true, true, true, true, true,true,true);
    }

    public void ToggleMapDisplay()
    {
        SetObjectsActive(true, false, false, false, false, false, false, false, false, false,false,false);
        if (backButton != null) backButton.gameObject.SetActive(true);  
    }

    public void ReturnToPreviousView()  
    {
        SetObjectsActive(false, true, true, true, true, true, true, true, true, true,true,true);
        if (backButton != null) backButton.gameObject.SetActive(false);  
    }

    void SetObjectsActive(bool mapActive, bool visualAssetsActive, bool animationsActive, bool levelLayoutActive,
                          bool manualButtonActive, bool playButtonActive, bool mapDisplayButtonActive,
                          bool animationTextActive, bool visualAssetsTextActive, bool topLeftCornerPieceTextActive, bool levelContainer, bool procedurallevelButton)
    {
        if (theWholeLevel != null) theWholeLevel.SetActive(mapActive);
        if (visualAssetsSprites != null) visualAssetsSprites.SetActive(visualAssetsActive);
        if (animationsAndAnimators != null) animationsAndAnimators.SetActive(animationsActive);
        if (manualLevelLayout != null) manualLevelLayout.SetActive(levelLayoutActive);
        if (manualMapButton != null) manualMapButton.gameObject.SetActive(manualButtonActive);
        if (playButton != null) playButton.gameObject.SetActive(playButtonActive);
        if (mapDisplayButton != null) mapDisplayButton.gameObject.SetActive(mapDisplayButtonActive);
        if (animationText != null) animationText.gameObject.SetActive(animationTextActive);
        if (visualAssetsText != null) visualAssetsText.gameObject.SetActive(visualAssetsTextActive);
        if (topLeftCornerPieceText != null) topLeftCornerPieceText.gameObject.SetActive(topLeftCornerPieceTextActive);
        if (this.levelContainer != null) this.levelContainer.SetActive(levelContainer);
        if (this.procedurallevelButton != null) this.procedurallevelButton.gameObject.SetActive(procedurallevelButton);
    }
}


