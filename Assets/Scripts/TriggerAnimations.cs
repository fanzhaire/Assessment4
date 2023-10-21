using UnityEngine;
using UnityEngine.UI;

public class TriggerAnimations : MonoBehaviour
{
    public Animator pacStudentAnimator;
    public Animator powerPelletsAnimator;
    public Animator walkingStateAnimator;
    public Animator scaredStateAnimator;
    public Animator recoveringStateAnimator;
    public Animator deadStateAnimator;
    public Animator deathpacAnimator;
    public Button playButton;

    void Start()
    {
        DisableAllAnimators();

        if (playButton != null)
        {
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
    }

    void DisableAllAnimators()
    {
        if (pacStudentAnimator != null) pacStudentAnimator.enabled = false;
        if (powerPelletsAnimator != null) powerPelletsAnimator.enabled = false;
        if (walkingStateAnimator != null) walkingStateAnimator.enabled = false;
        if (scaredStateAnimator != null) scaredStateAnimator.enabled = false;
        if (recoveringStateAnimator != null) recoveringStateAnimator.enabled = false;
        if (deadStateAnimator != null) deadStateAnimator.enabled = false;
        if (deathpacAnimator != null) deathpacAnimator.enabled = false;
    }

    public void OnPlayButtonClicked()
    {
        PlayAnimations();
    }

    void PlayAnimations()
    {
        if (pacStudentAnimator != null)
        {
            pacStudentAnimator.enabled = true;
            pacStudentAnimator.Play("WalkUp");  
        }
        if (powerPelletsAnimator != null)
        {
            powerPelletsAnimator.enabled = true;
            powerPelletsAnimator.Play("PowerPelletBlink");
        }
        if (walkingStateAnimator != null)
        {
            walkingStateAnimator.enabled = true;
            walkingStateAnimator.Play("GhostWalk");
        }
        if (scaredStateAnimator != null)
        {
            scaredStateAnimator.enabled = true;
            scaredStateAnimator.Play("A Scared state");
        }
        if (recoveringStateAnimator != null)
        {
            recoveringStateAnimator.enabled = true;
            recoveringStateAnimator.Play("A Recovering state");
        }
        if (deadStateAnimator != null)
        {
            deadStateAnimator.enabled = true;
            deadStateAnimator.Play("A Dead state");
        }
        if (deathpacAnimator != null)  
        {
            deathpacAnimator.enabled = true;
            deathpacAnimator.Play("deadthpac");  
        }
    }
}

