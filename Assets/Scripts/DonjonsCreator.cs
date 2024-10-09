using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonjonsCreator : MonoBehaviour {
   public int Seed = 666;
   public int Width;
   public int Height;
   public int CutQtt = 1;
   
   
   private List<int[]> _donjon = new List<int[]>();
   private void Start()
   {
      
      int[] a = new[] { 1, 1, 1, 1 };
      int[] b = new[] { 2, 2, 3, 3 };
      List<int[]> ab = new List<int[]>();
      ab.Add(a);
      ab.Add(b);
      DisplayDonjon(ab);
      DisplayDonjon(BSP(_donjon, CutQtt));
   }

   /**
    * Les salles du donjon sont des tableaux de Int sous la forme:
    * TailleX | TailleY | AncreX | AncreY
    */
   public List<int[]> BSP(List<int[]> donjon, int cutQtt) {
      if (donjon.Count == 0) {
         int[] premièreSalle = new []{Width,Height,0,0};
         donjon.Add(premièreSalle);
      }

      if (cutQtt <= 0) return donjon;
      else {
         /*
          * On cherche la salle la plus grande qui sera aussi celle que l'on divisera en deux.
          */
         int tmp = 0;
         if (donjon.Count > 1) {
            for (int i = 0; i < donjon.Count-1; i++) {
               if (tmp != i && donjon[tmp][0] * donjon[tmp][1] < donjon[i][0] * donjon[i][1]) tmp = i;
            }
         }
         int[] cutRoom = donjon[tmp];
         donjon.Remove(donjon[tmp]);
         
         /*
          * On défini si on la coupe sur sa longueur ou sa largeur.
          * Dans ce cas je décide de choisir de couper la plus grande longueur en deux à un endroit aléatoire.
          */
         if (cutRoom[0] >= cutRoom[1]) {
            System.Random rand = new System.Random(Seed);
            int cutPosition = rand.Next(cutRoom[2], cutRoom[2]+cutRoom[0]); //niveau de la coupure
            /*
             * On créer donc les deux salles nouvellements existantes, avant et après la coupure.
             * La première est celle d'avant, la seconde est celle d'après.
             */
            int[] newRoom1 = { /*tailleX*/ cutRoom[2] + cutPosition -1,
               /*tailleY*/ cutRoom[1],
               /*ancreX*/ cutRoom[2],
               /*ancreY*/ cutRoom[3]
            };
            int[] newRoom2 = {
               /*tailleX*/ cutRoom[0] - (cutRoom[2] + cutPosition + 1),
               /*tailleY*/ cutRoom[1],
               /*ancreX*/ cutRoom[2] + cutPosition + 1,
               /*ancreY*/ cutRoom[3]
            };
            donjon.Add(newRoom1);
            donjon.Add(newRoom2);
         }
         else {
            System.Random rand = new System.Random(Seed);
            int cutPosition = rand.Next(cutRoom[3], cutRoom[3] + cutRoom[1]); //niveau de la coupure
            int[] newRoom1 = new []{
               /*tailleX*/ cutRoom[0],
               /*tailleY*/ cutRoom[3] + cutPosition -1,
               /*ancreX*/ cutRoom[2],
               /*ancreY*/ cutRoom[3]
            };
            int[] newRoom2 = new []{
               /*tailleX*/ cutRoom[0],
               /*tailleY*/ cutRoom[1] - cutPosition + 1,
               /*ancreX*/ cutRoom[2],
               /*ancreY*/ cutRoom[3] + cutPosition + 1
            };
            donjon.Add(newRoom1);
            donjon.Add(newRoom2);
         }

         cutQtt -= 1;
         return BSP(donjon, cutQtt);
      }
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
