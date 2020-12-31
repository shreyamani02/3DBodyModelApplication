using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public Image controls;
    public Image Sone;
    public Image Stwo;
    public Image Sthree;
    // public GameObject img;
    // public void buttonEvent()
    // {
    //   if (img.active)
    //     img.SetActive(false);
    //   else
    //     img.SetActive(true);
    // }
    public void controlsStart()
    {
      if (!controls.gameObject.active) {
        controls.gameObject.SetActive(true);
        StartCoroutine(startSlide(0.2f));
      }
      else if (controls.gameObject.active) {
        StartCoroutine(startSlide(-0.2f));
      }
      // controls.gameObject.SetActive(true);
      // StartCoroutine(startSlide());
    }
    IEnumerator startSlide(float i) {
      yield return new WaitForSeconds(0.01f);
      controls.fillAmount = controls.fillAmount + i;
      // Sone.fillAmount = Sone.fillAmount + i;
      // Stwo.fillAmount = Stwo.fillAmount + i;
      // Sthree.fillAmount = Sthree.fillAmount + i;
      if (controls.fillAmount<1 && controls.fillAmount>0)
        StartCoroutine(startSlide(i));
      else if (controls.fillAmount==0)
        controls.gameObject.SetActive(false);
    }
}
