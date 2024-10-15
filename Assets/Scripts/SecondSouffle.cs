using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
      MyDungeon = BSPsecond(MyDungeon,Cut);
      DisplayDungeonData();
      DisplayDungeon();
   }

   public void InitiateDonjon() {
      MyDungeon = new List<Room>();
      Room room = new Room(Width, Height,0,0);
      MyDungeon.Add(room);
   }

   public List<Room> BSPsecond(List<Room> dungeon,int cut) {
      if (cut <= 0) return dungeon;
      //Choix de la salle
      
      //Choix du coté à split
      
      //select split position
      
      //split
      dungeon = Split(dungeon,dungeon[_random.Next(0,dungeon.Count-1)],Coinflip());
      cut -= 1;
      return BSPsecond(dungeon,cut);
   }
   private int Coinflip(){return _random.Next(0,1);}
/**
 * sideToSplit = 'x' || 'y'
 */
   public List<Room> Split(List<Room> dungeon, Room roomToSplit, int sideToSplit) {
      Room roomPartOne = new Room(roomToSplit);
      Room roomPartTwo = new Room(roomToSplit);
      switch (sideToSplit) {
         case 0:
            int pos = _random.Next(roomToSplit.Anchor.x, roomToSplit.Size.x + roomToSplit.Anchor.x);
            Debug.Log("Pos"+pos);
            roomPartOne.Size.x = pos-roomPartOne.Anchor.x ;
            //Debug.Log("RoomPartOne: Size->"+roomPartOne.Size+" Anchor->"+roomPartOne.Anchor);
            roomPartTwo.Size.x -= roomPartOne.Size.x - 1;
            roomPartTwo.Anchor.x = pos+1;
            //Debug.Log("RoomPartTwo: Size->" + roomPartTwo.Size + " Anchor->" + roomPartTwo.Anchor);
            dungeon.Remove(roomToSplit);
            dungeon.Add(roomPartOne);
            dungeon.Add(roomPartTwo);
            return dungeon;
         case 1:
            pos = _random.Next(roomToSplit.Anchor.y, roomToSplit.Size.y + roomToSplit.Anchor.y);
            roomPartOne.Size.y = pos - roomPartOne.Anchor.y;
            roomPartTwo.Size.y -= roomPartOne.Size.y - 1;
            roomPartTwo.Anchor.y = pos + 1;
            dungeon.Remove(roomToSplit);
            dungeon.Add(roomPartOne);
            dungeon.Add(roomPartTwo);
            return dungeon;
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
   public Vector2Int Size;
   public Vector2Int Anchor;

   public Room(int x, int y, int i, int j)
   {
      Size = new Vector2Int(x, y);
      Anchor = new Vector2Int(i, j);
   }

   public Room(Room origin)
   {
      Size = origin.Size;
      Anchor = origin.Anchor;
   }

   public Room Clone(){
      Room clone = new Room(Size.x, Size.y, Anchor.x, Anchor.y);
      return clone;
   }
}