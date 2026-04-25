using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneInitiator : MonoBehaviour
{
   private CutsceneHandler cutsceneHandler;

   public void Start()
   {
      cutsceneHandler = GetComponent<CutsceneHandler>();
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if(collision.gameObject.tag == "Player")
      {
         Debug.Log("Triggered");
         cutsceneHandler.PlayNextElement();
      }
   }
}
