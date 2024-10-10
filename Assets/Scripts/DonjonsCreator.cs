using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DonjonsCreator : MonoBehaviour {
   public int Seed = 666;
   public int WidthX;
   public int HeightY;
   public int CutQtt = 1;
   
   
   private List<int[]> _donjon = new List<int[]>();
   private void Start() {
      DisplayDonjon(BSP(_donjon, CutQtt));
   }

   /**
    * Les salles du donjon sont des tableaux de Int sous la forme:
    * TailleX | TailleY | AncreX | AncreY | AirSalle
    */
   public List<int[]> BSP(List<int[]> donjon, int cutQtt) {
      if (donjon.Count == 0) {
         int[] premièreSalle = new []{WidthX, HeightY, 0, 0, WidthX*HeightY};
         donjon.Add(premièreSalle);
      }

      if (cutQtt <= 0) return donjon;
      else {
         /*
          * On cherche la salle la plus grande qui sera aussi celle que l'on divisera en deux.
          */
         int tmp = 0;
         if (donjon.Count > 1) {
            for (int i = 0; i < donjon.Count; i++) {
               if (tmp != i && donjon[tmp][4] < donjon[i][4]) tmp = i;
            }
         }
         int[] cutRoom = donjon[tmp];
         donjon.Remove(donjon[tmp]);
         
         /*
          * On défini si on la coupe sur sa longueur ou sa largeur.
          * Dans ce cas je décide de choisir de couper la plus grande longueur en deux à un endroit aléatoire.
          */
         if (cutRoom[0] > cutRoom[1])
         {
            cutQtt -= 1;
            return BSP(Cut2DRoomIn2DDungeon(donjon, cutRoom, 'X'), cutQtt);
         }
         cutQtt -= 1;
         return BSP(Cut2DRoomIn2DDungeon(donjon, cutRoom, 'Y'), cutQtt);
      }
   }

   /**
    * Coupe la room, en fonction de l'axe choisi, en deux et renvoie le donjon contenant les room nouvellement créés
    */
   private List<int[]> Cut2DRoomIn2DDungeon(List<int[]> dungeon,int[] cutedRoom, char axe) {
      System.Random rand = new System.Random(Seed);
      int[] newRoom1 = new int[cutedRoom.Length];
      int[] newRoom2 = new int[cutedRoom.Length];
      switch (axe) {
         case 'X':
            int cutPosition = rand.Next(cutedRoom[2], cutedRoom[2]+cutedRoom[0]); //niveau de la coupure
            newRoom1[0] = /*tailleX*/ cutedRoom[2] + cutPosition - 1;
            newRoom1[1] = /*tailleY*/ cutedRoom[1];
            newRoom1[2] = /*ancreX*/ cutedRoom[2];
            newRoom1[3] = /*ancreY*/ cutedRoom[3];
            newRoom1[4] = /*airSalle*/ cutedRoom[0] * cutedRoom[1];

            newRoom2[0] = /*tailleX*/ cutedRoom[0] - (cutedRoom[2] + cutPosition + 1);
            newRoom2[1] = /*tailleY*/ cutedRoom[1];
            newRoom2[2] = /*ancreX*/ cutedRoom[2] + cutPosition + 1;
            newRoom2[2] = /*ancreY*/ cutedRoom[3];
            newRoom2[4] = /*airSalle*/ cutedRoom[0] * cutedRoom[1];
            break;
         case 'Y':
            cutPosition = rand.Next(cutedRoom[3], cutedRoom[3] + cutedRoom[1]); //niveau de la coupure
            newRoom1[0] = /*tailleX*/ cutedRoom[0];
            newRoom1[1] = /*tailleY*/ cutedRoom[3] + cutPosition - 1;
            newRoom1[2] = /*ancreX*/ cutedRoom[2];
            newRoom1[3] = /*ancreY*/ cutedRoom[3];
            newRoom1[4] = /*airSalle*/ cutedRoom[0] * cutedRoom[1];

            newRoom2[0] = /*tailleX*/ cutedRoom[0];
            newRoom2[1] = /*tailleY*/ cutedRoom[1] - (cutedRoom[3] + cutPosition + 1);
            newRoom2[2] = /*ancreX*/ cutedRoom[2];
            newRoom2[3] = /*ancreY*/ cutedRoom[3] + cutPosition + 1;
            newRoom2[4] = /*airSalle*/ cutedRoom[0] * cutedRoom[1];
            break;
      }
      dungeon.Add(newRoom1);
      dungeon.Add(newRoom2);
      return dungeon;
   }

   public void DisplayDonjon(List<int[]> donjon)
   {
      for (int i = 0; i < donjon.Count; i++)
      {
         Debug.Log("Salle " + i + ": TailleX = " + donjon[i][0] + " TailleY = " + donjon[i][1] + " AncreX= " + donjon[i][2] + 
                   " AncreY= "+ donjon[i][3] + ".");
      }
   }
}
