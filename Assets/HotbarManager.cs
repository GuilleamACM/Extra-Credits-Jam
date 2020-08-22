using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HotbarManager : MonoBehaviour
{
    public Image[] hotbarSlots;
    private int currentSelectedSlot = 0;

    private void Start()
    {
        ResetAllSlots();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            ResetAllSlots();
    }

    public void SelectSlot(int slot)
    {
        var index = slot - 1;
        Sequence hotbarSequence = DOTween.Sequence();

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i == index)
            {
                hotbarSequence.Join(hotbarSlots[i].transform.DOScale(1.2f, 0.5f));
            }
            else
            {
                hotbarSequence.Join(hotbarSlots[i].transform.DOScale(1f, 0.5f));
            }
        }

        hotbarSequence.Play();
        //hotbarSequence.OnComplete();
    }

    public void ResetAllSlots()
    {
        Sequence hotbarSequence = DOTween.Sequence();
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSequence.Join(hotbarSlots[i].transform.DOScale(1f, 0.5f));
        }
    }

}
