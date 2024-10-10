using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class DonjonsCreator : MonoBehaviour {
   public int Seed = 666;
   public int WidthX;
   public int HeightY;
   public int CutQtt = 1;
   public Tile WhiteTile;
   public Tile BlackTile;
   public Tilemap Tilemap;
   
   private List<int[]> _dungeon = new List<int[]>();
   private void Start() {
      _dungeon = BSP(_dungeon, CutQtt);
      DisplayDungeonInDebug(_dungeon);
      DisplayDungeonInTilemap(_dungeon);
   }

   /**
    * Les salles du donjon sont des tableaux de Int sous la forme:
    * TailleX | TailleY | AncreX | AncreY | AirSalle
    */
   public List<int[]> BSP(List<int[]> dungeon, int cutQtt) {
      if (dungeon.Count == 0) {
         int[] firstRoom = new []{WidthX, HeightY, 0, 0, WidthX*HeightY};
         dungeon.Add(firstRoom);
      }
      if (cutQtt <= 0) return dungeon;
      int tmp = 0;
      if (dungeon.Count > 1) {
         for (int i = 0; i < dungeon.Count; i++) {
            if (tmp != i && dungeon[tmp][4] < dungeon[i][4]) tmp = i;
         }
      }
      int[] cutRoom = dungeon[tmp];
      dungeon.Remove(dungeon[tmp]);
         
      if (cutRoom[0] > cutRoom[1] && cutRoom[0] >= 3) {
         cutQtt -= 1;
         return BSP(Cut2DRoomIn2DDungeon(dungeon, cutRoom, 'X'), cutQtt);
      }
      if (cutRoom[0] <= cutRoom[1] && cutRoom[1] >= 3) {
         cutQtt -= 1;
         return BSP(Cut2DRoomIn2DDungeon(dungeon, cutRoom, 'Y'), cutQtt);
      }
      cutQtt -= 1;
      return BSP(dungeon, cutQtt);
   }

   /**
    * Coupe la room, en fonction de l'axe choisi, en deux et renvoie le donjon contenant les room nouvellement créés
    */
   private List<int[]> Cut2DRoomIn2DDungeon(List<int[]> dungeon,int[] cutedRoom, char axe) {
      System.Random rand = new System.Random(Seed);
      int[] newRoom1 = new int[cutedRoom.Length];
      int[] newRoom2 = new int[cutedRoom.Length];
      int cutPosition;
      switch (axe) {
         case 'X':
            cutPosition = rand.Next(cutedRoom[2], cutedRoom[2]+cutedRoom[0]);
            newRoom1[0] = cutedRoom[2] + cutPosition ;                     /*tailleX*/
            newRoom1[1] = cutedRoom[1];                                    /*tailleY*/
            newRoom1[2] = cutedRoom[2];                                    /*ancreX*/ 
            newRoom1[3] = cutedRoom[3];                                    /*ancreY*/
            newRoom1[4] = cutedRoom[0] * cutedRoom[1];                     /*airSalle*/

            newRoom2[0] = cutedRoom[0] - (cutedRoom[2] + cutPosition ) -1;    /*tailleX*/
            newRoom2[1] = cutedRoom[1];                                    /*tailleY*/
            newRoom2[2] = cutedRoom[2] + cutPosition + 1 ;                 /*ancreX*/
            newRoom2[3] = cutedRoom[3];                                    /*ancreY*/
            newRoom2[4] = cutedRoom[0] * cutedRoom[1];                     /*airSalle*/
            break;
         case 'Y':
             cutPosition = rand.Next(cutedRoom[3], cutedRoom[3] + cutedRoom[1]);
            newRoom1[0] = cutedRoom[0];                                /*tailleX*/
            newRoom1[1] = cutedRoom[3] + cutPosition;                  /*tailleY*/
            newRoom1[2] = cutedRoom[2];                                /*ancreX*/
            newRoom1[3] = cutedRoom[3];                                /*ancreY*/
            newRoom1[4] = cutedRoom[0] * cutedRoom[1];                 /*airSalle*/

            newRoom2[0] = cutedRoom[0];                                       /*tailleX*/
            newRoom2[1] = cutedRoom[1] - (cutedRoom[3] + cutPosition ) -1 ;      /*tailleY*/
            newRoom2[2] = cutedRoom[2];                                       /*ancreX*/
            newRoom2[3] = cutedRoom[3] + cutPosition + 1;                     /*ancreY*/
            newRoom2[4] = cutedRoom[0] * cutedRoom[1];                        /*airSalle*/
            break;
      }
      dungeon.Add(newRoom1);
      dungeon.Add(newRoom2);
      return dungeon;
   }

   public void DisplayDungeonInDebug(List<int[]> dungeon)
   {
      for (int i = 0; i < dungeon.Count; i++)
      {
         Debug.Log("Salle " + i + ": TailleX =" + dungeon[i][0] + " TailleY =" + dungeon[i][1] + " AncreX=" + dungeon[i][2] + 
                   " AncreY="+ dungeon[i][3] + ".");
      }
   }

   public void DisplayDungeonInTilemap(List<int[]> dungeon) {
      for (int i = 0; i < dungeon.Count; i++) {
         for (int j = dungeon[i][2]; j < dungeon[i][2]+dungeon[i][0]; j++) {
            for (int k = dungeon[i][3]; k < dungeon[i][3]+dungeon[i][1]; k++) {
               Tilemap.SetTile(new Vector3Int(j, k, 0), WhiteTile);
            }
         }
      }
   }
}
