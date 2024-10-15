using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = Unity.Mathematics.Random;

public class SecondSouffle : MonoBehaviour
{
   public int Width;
   public int Height;
   public List<Room> MyDungeon;
   public Tilemap MyTilemap;
   public Tile MyTile;
   public int Seed;
   public int Cut;
   
   private System.Random _random;
   private void Start() {
      _random = new System.Random(Seed);
      InitiateDonjon();
      MyDungeon = BSPsecond(MyDungeon);
      DisplayDungeonData();
      DisplayDungeon();
   }

   public void InitiateDonjon() {
      MyDungeon = new List<Room>();
      Room room = new Room(Width, Height,0,0);
      MyDungeon.Add(room);
   }

   public List<Room> BSPsecond(List<Room> dungeon) {
      //Choix de la salle
      
      //Choix du coté à split
      
      //select split position
      
      //split
      dungeon = Split(dungeon,dungeon[_random.Next(0,dungeon.Count-1)],Coinflip(),Cut);
      return dungeon;
   }
   private int Coinflip(){return _random.Next(0,1);}
/**
 * sideToSplit = 'x' || 'y'
 */
   public List<Room> Split(List<Room> dungeon, Room roomToSplit, int sideToSplit, int cut) {
      if(cut <= 0)return dungeon;
      switch (sideToSplit) {
         case 0:
            int pos = _random.Next(roomToSplit.Anchor.x, roomToSplit.Size.x + roomToSplit.Anchor.x);
            Debug.Log("Pos"+pos);
            Room roomPartOne = new Room(roomToSplit.Size.x - pos, roomToSplit.Size.y, 
                                       roomToSplit.Anchor.x,roomToSplit.Anchor.y);
            //Debug.Log("RoomPartOne: Size->"+roomPartOne.Size+" Anchor->"+roomPartOne.Anchor);
            
            Room roomPartTwo = new Room(roomToSplit.Size.x - roomPartOne.Size.x - 1, roomToSplit.Size.y, 
                                       roomToSplit.Anchor.x + pos +2,roomToSplit.Anchor.y);
            //Debug.Log("RoomPartTwo: Size->" + roomPartTwo.Size + " Anchor->" + roomPartTwo.Anchor);
            dungeon.Remove(roomToSplit);
            dungeon.Add(roomPartOne);
            dungeon.Add(roomPartTwo);
            return Split(dungeon,dungeon[_random.Next(0,dungeon.Count-1)],Coinflip(),--cut);
         case 1:
            pos = _random.Next(roomToSplit.Anchor.y, roomToSplit.Size.y + roomToSplit.Anchor.y);
            roomPartOne = new Room(roomToSplit.Size.x, roomToSplit.Size.y - pos, 
                                 roomToSplit.Anchor.x,roomToSplit.Anchor.y);
            roomPartTwo = new Room(roomToSplit.Size.x , roomToSplit.Size.y- roomPartOne.Size.y - 1, 
                                 roomToSplit.Anchor.x ,roomToSplit.Anchor.y+ pos +2);
            dungeon.Remove(roomToSplit);
            dungeon.Add(roomPartOne);
            dungeon.Add(roomPartTwo);
            return Split(dungeon,dungeon[_random.Next(0,dungeon.Count-1)],Coinflip(),--cut);
         default:
            Debug.LogError("Invalid side to Split");
            return dungeon;
      }
   }

   public void DisplayDungeonData() {
      for (int i = 0; i < MyDungeon.Count; i++) {
         Debug.Log("Salle " + i + ": Taille=" + MyDungeon[i].Size + " Ancre=" + MyDungeon[i].Anchor + ".");
      }
   }

   public void DisplayDungeon() {
      for (int i = 0; i < MyDungeon.Count; i++) {
         for (int j = MyDungeon[i].Anchor.x; j < MyDungeon[i].Size.x + MyDungeon[i].Anchor.x; j++) {
            for (int k = MyDungeon[i].Anchor.y; k < MyDungeon[i].Size.y + MyDungeon[i].Anchor.y; k++) {
               MyTilemap.SetTile(new Vector3Int(j, k, 0), MyTile);
            }
         }
      }
   }
}

public struct Room
{
   public Vector2Int Size { get; }
   public Vector2Int Anchor { get; }

   public Room(int x, int y, int i, int j)
   {
      Size = new Vector2Int(x, y);
      Anchor = new Vector2Int(i, j);
   }
}