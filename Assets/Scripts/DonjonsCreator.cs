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
      System.Random rand1 = new System.Random(Seed);
      if (dungeon.Count == 0) {
         int[] firstRoom = new []{WidthX, HeightY, 0, 0, WidthX*HeightY};
         dungeon.Add(firstRoom);
      }
      if (cutQtt <= 0) return dungeon;
      cutQtt -= 1;
      return BSP(Cut2DRoomIn2DDungeon(dungeon, RoomToCutSelection(dungeon)), cutQtt);
   }

   private int[] RoomToCutSelection(List<int[]> dungeon) {
      int[] tmp = dungeon[0];
      for (int i = 0; i < dungeon.Count; i++) {
         if (tmp[4] < dungeon[i][4]) tmp = dungeon[i];
      }
      return tmp;
   }
   
   /**
    * Coupe la room, en fonction de l'axe choisi, en deux et renvoie le donjon contenant les room nouvellement créés
    */
   private List<int[]> Cut2DRoomIn2DDungeon(List<int[]> dungeon,int[] cutedRoom) {
      System.Random rand2 = new System.Random(Seed);
      int[] newRoom1 = new int[cutedRoom.Length];
      int[] newRoom2 = new int[cutedRoom.Length];
      int cutPosition;
      if (cutedRoom[0] > cutedRoom[1] && cutedRoom[0] >= 3) {
         cutPosition = rand2.Next(cutedRoom[2], cutedRoom[2]+cutedRoom[0]);
         if(cutedRoom[2]==0) cutPosition = rand2.Next(1, cutedRoom[2] + cutedRoom[0]);
         
         newRoom1[0] = cutPosition ;/*tailleX*/ newRoom1[1] = cutedRoom[1];/*tailleY*/
         newRoom1[2] = cutedRoom[2];/*ancreX*/ newRoom1[3] = cutedRoom[3];/*ancreY*/
         newRoom1[4] = newRoom1[0] * newRoom1[1];/*airSalle*/

         newRoom2[0] = cutedRoom[0] - cutPosition;/*tailleX*/ newRoom2[1] = cutedRoom[1];/*tailleY*/
         newRoom2[2] = cutPosition;/*ancreX*/ newRoom2[3] = cutedRoom[3]; /*ancreY*/
         newRoom2[4] = newRoom2[0] * newRoom2[1];/*airSalle*/
         dungeon.Remove(cutedRoom);
         dungeon.Add(newRoom1);
         dungeon.Add(newRoom2);
      }
            
      if (cutedRoom[0] <= cutedRoom[1] && cutedRoom[1] >= 3){
         cutPosition = rand2.Next(cutedRoom[3], cutedRoom[3] + cutedRoom[1]);
         if(cutedRoom[3]==0) cutPosition = rand2.Next(1, cutedRoom[3] + cutedRoom[1]);

         newRoom1[0] = cutedRoom[0];/*tailleX*/ newRoom1[1] = cutPosition;/*tailleY*/
         newRoom1[2] = cutedRoom[2];/*ancreX*/ newRoom1[3] = cutedRoom[3];/*ancreY*/
         newRoom1[4] = newRoom1[0] * newRoom1[1];/*airSalle*/

         newRoom2[0] = cutedRoom[0];/*tailleX*/ newRoom2[1] = cutedRoom[1] - cutPosition;/*tailleY*/
         newRoom2[2] = cutedRoom[2];/*ancreX*/ newRoom2[3] = cutPosition;/*ancreY*/
         newRoom2[4] = newRoom2[0] * newRoom2[1];/*airSalle*/
         dungeon.Remove(cutedRoom);
         dungeon.Add(newRoom1);
         dungeon.Add(newRoom2);
      }
      return dungeon;
   }

   private void ResizeRooms(List<int[]> dungeon) {
      foreach (int[] room in dungeon) {
         room[0] -= 2; room[1] -= 2; room[2]++; room[3]++;
         if(room[0] <= 0 || room[1] <= 0)dungeon.Remove(room);
      }
   }
   
   public void DisplayDungeonInDebug(List<int[]> dungeon) {
      for (int i = 0; i < dungeon.Count; i++) {
         Debug.Log("Salle " + i + ": TailleX=" + dungeon[i][0] + " TailleY=" + dungeon[i][1] + " AncreX=" + dungeon[i][2] + 
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
//delaunay : https://www.gorillasun.de/blog/bowyer-watson-algorithm-for-delaunay-triangulation/
struct Vertex {
   public int x;
   public int y;
   public bool Equals(Vertex other){return x == other.x && y == other.y;}
}

struct Edge {
   public Vertex a;
   public Vertex b;
   public bool Equals(Edge other){return a.Equals(other.a) && b.Equals(other.b);}
}

struct Triangle {
   public Vertex a;
   public Vertex b;
   public Vertex c;
   
   
}
