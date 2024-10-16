using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonCreator : MonoBehaviour {
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
      MyDungeon = BSPsecond(Cut);
      DisplayDungeonDebug();
      DisplayDungeonTilemap();
   }
   public void InitiateDonjon() {
      MyDungeon = new List<Room>();
      Room room = new Room(Width, Height,0,0);
      MyDungeon.Add(room);
   }
   public List<Room> BSPsecond(int cut) {
      if (cut <= 0) return MyDungeon;
      Room selectedRoomToSlice = SelectRoom();
      Split(selectedRoomToSlice,SelectSliceAxe(selectedRoomToSlice));
      DungeonCleaner();
      cut -= 1;
      return BSPsecond(cut);
   }
   public void Split( Room roomToSplit, char sideToSplit) {
      Room roomPartOne = new Room(roomToSplit);
      Room roomPartTwo = new Room(roomToSplit);
      switch (sideToSplit) {
         case 'X':
            int pos = _random.Next(roomToSplit.Anchor.x, roomToSplit.Anchor.x + roomToSplit.Size.x );
            Debug.Log("WidthPos"+pos);
            roomPartOne.Size.x = pos-roomPartOne.Anchor.x ;
            roomPartTwo.Size.x -= roomPartOne.Size.x;
            roomPartTwo.Anchor.x = pos;
            MyDungeon.Remove(roomToSplit);
            MyDungeon.Add(roomPartOne);
            MyDungeon.Add(roomPartTwo);
            return;
         case 'Y':
            pos = _random.Next(roomToSplit.Anchor.y, roomToSplit.Size.y + roomToSplit.Anchor.y);
            Debug.Log("HeightPos"+pos);
            roomPartOne.Size.y = pos - roomPartOne.Anchor.y;
            roomPartTwo.Size.y -= roomPartOne.Size.y;
            roomPartTwo.Anchor.y = pos;
            MyDungeon.Remove(roomToSplit);
            MyDungeon.Add(roomPartOne);
            MyDungeon.Add(roomPartTwo);
            return ;
         default:
            Debug.LogError("Invalid side to Split");
            return ;
      }
   }
   public void DisplayDungeonDebug() {
      for (int i = 0; i < MyDungeon.Count; i++) {
         Debug.Log("Salle " + i + ": Taille=" + MyDungeon[i].Size + " Ancre=" + MyDungeon[i].Anchor + ".");
      }
   }
   public void DisplayDungeonTilemap() {
      for (int i = 0; i < MyDungeon.Count; i++) {
         for (int j = MyDungeon[i].Anchor.x; j < MyDungeon[i].Size.x + MyDungeon[i].Anchor.x; j++) {
            for (int k = MyDungeon[i].Anchor.y; k < MyDungeon[i].Size.y + MyDungeon[i].Anchor.y; k++) {
               MyTilemap.SetTile(new Vector3Int(j, k, 0), MyTile);
               MyTilemap.SetTileFlags(new Vector3Int(j, k, 0), TileFlags.None);//toujours penser à unlock la flags
               MyTilemap.SetColor(new Vector3Int(j, k, 0), MyDungeon[i].Color);
            }
         }
      }
   }
   private Room SelectRoom() {
      Room tmp = MyDungeon[0];
      foreach (Room room in MyDungeon) { if (room.Size.y + room.Size.x > tmp.Size.y + tmp.Size.x) tmp = room; }
      return tmp;
   }
   private Char SelectSliceAxe(Room room) {
      if (room.Size.x > room.Size.y) return 'X';
      return 'Y';
   }
   private void DungeonCleaner() {
      foreach (Room room in MyDungeon) {
         if (room.Size.x == 0 || room.Size.y == 0) MyDungeon.Remove(room);
      }
   }
}