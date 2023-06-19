using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    private Slider slider;
    private Animator animator;
    private GraphicRaycaster graphicRaycaster;
    private GameObject currentLoadingImage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        slider = GetComponentInChildren<Slider>();
        graphicRaycaster = GetComponent<GraphicRaycaster>();
        currentLoadingImage = transform.GetChild(0).GetChild(0).gameObject;
    }
    public void SetImage(int imageIndex)
    {
        //TODO: change loadingImage depending on place
        currentLoadingImage.SetActive(false);
        currentLoadingImage = transform.GetChild(0).GetChild(imageIndex).gameObject;
        currentLoadingImage.SetActive(true);
    }
    public void FadeIn()
    {
        animator.SetBool("LoadScene", false);
        graphicRaycaster.enabled = false;
    }
    public void FadeOut()
    {
        animator.SetBool("LoadScene", true);
        graphicRaycaster.enabled = true;
    }
    public void SetProgress(float progress)
    {
        slider.value = progress;
    }
}
