using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    public class Map : ContentObject,ICloneable
    {
        /// <summary>
        /// The name of this section of the world.
        /// </summary>
        private string name;

        /// <summary>
        /// The name of this section of the world.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        private Point mapDimensions;

        /// <summary>
        /// The dimensions of the map, in tiles.
        /// </summary>
        public Point MapDimensions
        {
            get { return mapDimensions; }
            set { mapDimensions = value; }
        }


        /// <summary>
        /// The size of the tiles in this map, in pixels.
        /// </summary>
        private Point tileSize;

        /// <summary>
        /// The size of the tiles in this map, in pixels.
        /// </summary>
        public Point TileSize
        {
            get { return tileSize; }
            set { tileSize = value; }
        }


        /// <summary>
        /// The number of tiles in a row of the map texture.
        /// </summary>
        /// <remarks>
        /// Used to determine the source rectangle from the map layer value.
        /// </remarks>
        private int tilesPerRow;

        /// <summary>
        /// The number of tiles in a row of the map texture.
        /// </summary>
        /// <remarks>
        /// Used to determine the source rectangle from the map layer value.
        /// </remarks>
        [ContentSerializerIgnore]
        public int TilesPerRow
        {
            get { return tilesPerRow; }
        }
        /// <summary>
        /// A valid spawn position for this map. 
        /// </summary>
        private Point spawnMapPosition;

        /// <summary>
        /// A valid spawn position for this map. 
        /// </summary>
        public Point SpawnMapPosition
        {
            get { return spawnMapPosition; }
            set { spawnMapPosition = value; }
        }
        /// <summary>
        /// The content name of the texture that contains the tiles for this map.
        /// </summary>
        private string textureName;

        /// <summary>
        /// The content name of the texture that contains the tiles for this map.
        /// </summary>
        public string TextureName
        {
            get { return textureName; }
            set { textureName = value; }
        }


        /// <summary>
        /// The texture that contains the tiles for this map.
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// The texture that contains the tiles for this map.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D Texture
        {
            get { return texture; }
        }


        /// <summary>
        /// The content name of the texture that contains the background for combats 
        /// that occur while traveling on this map.
        /// </summary>
        private string combatTextureName;

        /// <summary>
        /// The content name of the texture that contains the background for combats 
        /// that occur while traveling on this map.
        /// </summary>
        public string CombatTextureName
        {
            get { return combatTextureName; }
            set { combatTextureName = value; }
        }


        /// <summary>
        /// The texture that contains the background for combats 
        /// that occur while traveling on this map.
        /// </summary>
        private Texture2D combatTexture;

        /// <summary>
        /// The texture that contains the background for combats 
        /// that occur while traveling on this map.
        /// </summary>
        [ContentSerializerIgnore]
        public Texture2D CombatTexture
        {
            get { return combatTexture; }
        }
        /// <summary>
        /// The name of the music cue for this map.
        /// </summary>
        private string musicCueName;

        /// <summary>
        /// The name of the music cue for this map.
        /// </summary>
        public string MusicCueName
        {
            get { return musicCueName; }
            set { musicCueName = value; }
        }


        /// <summary>
        /// The name of the music cue for combats that occur while traveling on this map.
        /// </summary>
        private string combatMusicCueName;

        /// <summary>
        /// The name of the music cue for combats that occur while traveling on this map.
        /// </summary>
        public string CombatMusicCueName
        {
            get { return combatMusicCueName; }
            set { combatMusicCueName = value; }
        }
        /// <summary>
        /// Spatial array for the ground tiles for this map.
        /// </summary>
        private int[] baseLayer;

        /// <summary>
        /// Spatial array for the ground tiles for this map.
        /// </summary>
        public int[] BaseLayer
        {
            get { return baseLayer; }
            set { baseLayer = value; }
        }


        /// <summary>
        /// Retrieves the base layer value for the given map position.
        /// </summary>
        public int GetBaseLayerValue(Point mapPosition)
        {
            // check the parameter
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("mapPosition");
            }

            return baseLayer[mapPosition.Y * mapDimensions.X + mapPosition.X];
        }


        /// <summary>
        /// Retrieves the source rectangle for the tile in the given position
        /// in the base layer.
        /// </summary>
        /// <remarks>This method allows out-of-bound (blocked) positions.</remarks>
        public Rectangle GetBaseLayerSourceRectangle(Point mapPosition)
        {
            // check the parameter, but out-of-bounds is nonfatal
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                return Rectangle.Empty;
            }

            int baseLayerValue = GetBaseLayerValue(mapPosition);
            if (baseLayerValue < 0)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(
                (baseLayerValue % tilesPerRow) * tileSize.X,
                (baseLayerValue / tilesPerRow) * tileSize.Y,
                tileSize.X, tileSize.Y);
        }
        /// <summary>
        /// Spatial array for the fringe tiles for this map.
        /// </summary>
        private int[] fringeLayer;

        /// <summary>
        /// Spatial array for the fringe tiles for this map.
        /// </summary>
        public int[] FringeLayer
        {
            get { return fringeLayer; }
            set { fringeLayer = value; }
        }


        /// <summary>
        /// Retrieves the fringe layer value for the given map position.
        /// </summary>
        public int GetFringeLayerValue(Point mapPosition)
        {
            // check the parameter
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("mapPosition");
            }

            return fringeLayer[mapPosition.Y * mapDimensions.X + mapPosition.X];
        }


        /// <summary>
        /// Retrieves the source rectangle for the tile in the given position
        /// in the fringe layer.
        /// </summary>
        /// <remarks>This method allows out-of-bound (blocked) positions.</remarks>
        public Rectangle GetFringeLayerSourceRectangle(Point mapPosition)
        {
            // check the parameter, but out-of-bounds is nonfatal
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                return Rectangle.Empty;
            }

            int fringeLayerValue = GetFringeLayerValue(mapPosition);
            if (fringeLayerValue < 0)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(
                (fringeLayerValue % tilesPerRow) * tileSize.X,
                (fringeLayerValue / tilesPerRow) * tileSize.Y,
                tileSize.X, tileSize.Y);
        }
        /// <summary>
        /// Spatial array for the object images on this map.
        /// </summary>
        private int[] objectLayer;

        /// <summary>
        /// Spatial array for the object images on this map.
        /// </summary>
        public int[] ObjectLayer
        {
            get { return objectLayer; }
            set { objectLayer = value; }
        }


        /// <summary>
        /// Retrieves the object layer value for the given map position.
        /// </summary>
        public int GetObjectLayerValue(Point mapPosition)
        {
            // check the parameter
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("mapPosition");
            }

            return objectLayer[mapPosition.Y * mapDimensions.X + mapPosition.X];
        }


        /// <summary>
        /// Retrieves the source rectangle for the tile in the given position
        /// in the object layer.
        /// </summary>
        /// <remarks>This method allows out-of-bound (blocked) positions.</remarks>
        public Rectangle GetObjectLayerSourceRectangle(Point mapPosition)
        {
            // check the parameter, but out-of-bounds is nonfatal
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                return Rectangle.Empty;
            }

            int objectLayerValue = GetObjectLayerValue(mapPosition);
            if (objectLayerValue < 0)
            {
                return Rectangle.Empty;
            }

            return new Rectangle(
                (objectLayerValue % tilesPerRow) * tileSize.X,
                (objectLayerValue / tilesPerRow) * tileSize.Y,
                tileSize.X, tileSize.Y);
        }
        /// <summary>
        /// Spatial array for the collision properties of this map.
        /// </summary>
        private int[] collisionLayer;

        /// <summary>
        /// Spatial array for the collision properties of this map.
        /// </summary>
        public int[] CollisionLayer
        {
            get { return collisionLayer; }
            set { collisionLayer = value; }
        }


        /// <summary>
        /// Retrieves the collision layer value for the given map position.
        /// </summary>
        public int GetCollisionLayerValue(Point mapPosition)
        {
            // check the parameter
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                throw new ArgumentOutOfRangeException("mapPosition");
            }

            return collisionLayer[mapPosition.Y * mapDimensions.X + mapPosition.X];
        }


        /// <summary>
        /// Returns true if the given map position is blocked.
        /// </summary>
        /// <remarks>This method allows out-of-bound (blocked) positions.</remarks>
        public bool IsBlocked(Point mapPosition)
        {
            // check the parameter, but out-of-bounds is nonfatal
            if ((mapPosition.X < 0) || (mapPosition.X >= mapDimensions.X) ||
                (mapPosition.Y < 0) || (mapPosition.Y >= mapDimensions.Y))
            {
                return true;
            }

            return (GetCollisionLayerValue(mapPosition) != 0);
        }

        /// <summary>
        /// Portals to other maps.
        /// </summary>
        private List<Portal> portals = new List<Portal>();

        /// <summary>
        /// Portals to other maps.
        /// </summary>
        public List<Portal> Portals
        {
            get { return portals; }
            set { portals = value; }
        }
        /// <summary>
        /// The content names and positions of the portals on this map.
        /// </summary>
        private List<MapEntry<Portal>> portalEntries =
            new List<MapEntry<Portal>>();

        /// <summary>
        /// The content names and positions of the portals on this map.
        /// </summary>
        public List<MapEntry<Portal>> PortalEntries
        {
            get { return portalEntries; }
            set { portalEntries = value; }
        }

        /// <summary>
        /// Find a portal on this map based on the given portal name.
        /// </summary>
        public MapEntry<Portal> FindPortal(string name)
        {
            // check the parameter
            if (String.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("name");
            }

            return portalEntries.Find(delegate (MapEntry<Portal> portalEntry)
            {
                return (portalEntry.ContentName == name);
            });
        }


        /// <summary>
        /// The content names and positions of the treasure chests on this map.
        /// </summary>
        private List<MapEntry<Chest>> chestEntries =
            new List<MapEntry<Chest>>();

        /// <summary>
        /// The content names and positions of the treasure chests on this map.
        /// </summary>
        public List<MapEntry<Chest>> ChestEntries
        {
            get { return chestEntries; }
            set { chestEntries = value; }
        }


        /// <summary>
        /// The content name, positions, and orientations of the 
        /// fixed combat encounters on this map.
        /// </summary>
        private List<MapEntry<FixedCombat>> fixedCombatEntries =
            new List<MapEntry<FixedCombat>>();

        /// <summary>
        /// The content name, positions, and orientations of the 
        /// fixed combat encounters on this map.
        /// </summary>
        public List<MapEntry<FixedCombat>> FixedCombatEntries
        {
            get { return fixedCombatEntries; }
            set { fixedCombatEntries = value; }
        }


        /// <summary>
        /// The random combat definition for this map.
        /// </summary>
        private RandomCombat randomCombat;

        /// <summary>
        /// The random combat definition for this map.
        /// </summary>
        public RandomCombat RandomCombat
        {
            get { return randomCombat; }
            set { randomCombat = value; }
        }


        /// <summary>
        /// The content names, positions, and orientations of quest Npcs on this map.
        /// </summary>
        private List<MapEntry<QuestNpc>> questNpcEntries =
            new List<MapEntry<QuestNpc>>();

        /// <summary>
        /// The content names, positions, and orientations of quest Npcs on this map.
        /// </summary>
        public List<MapEntry<QuestNpc>> QuestNpcEntries
        {
            get { return questNpcEntries; }
            set { questNpcEntries = value; }
        }


        /// <summary>
        /// The content names, positions, and orientations of player Npcs on this map.
        /// </summary>
        private List<MapEntry<Player>> playerNpcEntries =
            new List<MapEntry<Player>>();

        /// <summary>
        /// The content names, positions, and orientations of player Npcs on this map.
        /// </summary>
        public List<MapEntry<Player>> PlayerNpcEntries
        {
            get { return playerNpcEntries; }
            set { playerNpcEntries = value; }
        }


        /// <summary>
        /// The content names, positions, and orientations of the inns on this map.
        /// </summary>
        private List<MapEntry<Inn>> innEntries =
            new List<MapEntry<Inn>>();

        /// <summary>
        /// The content names, positions, and orientations of the inns on this map.
        /// </summary>
        public List<MapEntry<Inn>> InnEntries
        {
            get { return innEntries; }
            set { innEntries = value; }
        }


        /// <summary>
        /// The content names, positions, and orientations of the stores on this map.
        /// </summary>
        private List<MapEntry<Store>> storeEntries =
            new List<MapEntry<Store>>();

        /// <summary>
        /// The content names, positions, and orientations of the stores on this map.
        /// </summary>
        public List<MapEntry<Store>> StoreEntries
        {
            get { return storeEntries; }
            set { storeEntries = value; }
        }

        public object Clone()
        {
            Map map = new Map();

            map.AssetName = AssetName;
            map.baseLayer = BaseLayer.Clone() as int[];
            foreach (MapEntry<Chest> chestEntry in chestEntries)
            {
                MapEntry<Chest> mapEntry = new MapEntry<Chest>();
                mapEntry.Content = chestEntry.Content.Clone() as Chest;
                mapEntry.ContentName = chestEntry.ContentName;
                mapEntry.Count = chestEntry.Count;
                mapEntry.Direction = chestEntry.Direction;
                mapEntry.MapPosition = chestEntry.MapPosition;
                map.chestEntries.Add(mapEntry);
            }
            map.chestEntries.AddRange(ChestEntries);
            map.collisionLayer = CollisionLayer.Clone() as int[];
            map.combatMusicCueName = CombatMusicCueName;
            map.combatTexture = CombatTexture;
            map.combatTextureName = CombatTextureName;
            map.fixedCombatEntries.AddRange(FixedCombatEntries);
            map.fringeLayer = FringeLayer.Clone() as int[];
            map.innEntries.AddRange(InnEntries);
            map.mapDimensions = MapDimensions;
            map.musicCueName = MusicCueName;
            map.name = Name;
            map.objectLayer = ObjectLayer.Clone() as int[];
            map.playerNpcEntries.AddRange(PlayerNpcEntries);
            map.portals.AddRange(Portals);
            map.portalEntries.AddRange(PortalEntries);
            map.questNpcEntries.AddRange(QuestNpcEntries);
            map.randomCombat = new RandomCombat();
            map.randomCombat.CombatProbability = RandomCombat.CombatProbability;
            map.randomCombat.Entries.AddRange(RandomCombat.Entries);
            map.randomCombat.FleeProbability = RandomCombat.FleeProbability;
            map.randomCombat.MonsterCountRange = RandomCombat.MonsterCountRange;
            map.spawnMapPosition = SpawnMapPosition;
            map.storeEntries.AddRange(StoreEntries);
            map.texture = Texture;
            map.textureName = TextureName;
            map.tileSize = TileSize;
            map.tilesPerRow = tilesPerRow;

            return map;
        }
    }
}
