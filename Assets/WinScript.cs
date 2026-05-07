using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WinScript : MonoBehaviour
{
   private int pointsToWin;
   private int currentPoints;
   public GameObject Scales;
   

   void Start()
   {
      pointsToWin = Scales.transform.childCount;
   }
   void Update()
   {
      if (currentPoints >= pointsToWin)
      {
         //WIN
         transform.GetChild(0).gameObject.SetActive(true);
      }
   }

   public void AddPoints()
   {
      currentPoints++;
   }
   
}
