using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mouse : MonoBehaviour
{
   public GameObject mouseItemUI;
   public Image mouseCursor;
   public ItemSlotInfo itemSlot;
   public Image itemImage;
   public TextMeshProUGUI stacksText;

   void Update()
   {
      transform.position = Input.mousePosition;
      if (Cursor.lockState == CursorLockMode.Locked)
      {
          mouseCursor.enabled = false;
          mouseItemUI.SetActive(false);
      }
      else
      {
          mouseCursor.enabled = true;

          if (itemSlot.item != null)
          {
              mouseItemUI.SetActive(true);
          }
          else
          {
              mouseItemUI.SetActive(false);
          }
      }
   }

   public void SetUI()
   {
       stacksText.text = "" + itemSlot.stacks;
       itemImage.sprite = itemSlot.item.GiveItemImage();
   }
   public void EmptySlot()
   {
       itemSlot = new ItemSlotInfo(null, 0);
   }

}
