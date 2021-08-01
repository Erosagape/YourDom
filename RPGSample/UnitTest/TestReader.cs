using System.IO;
using System.Xml.Serialization;
using System.Xml;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace RPGSample.UnitTest
{    
    public class MyMap
    {
        public Vector2 Position { get; set; }
        [XmlArray]
        [XmlArrayItem(ElementName ="Point")]
        public int[] TileMask { get; set; }
        public string TileValue { get; set; }
    }
    public static class TestSession
    {
        public static string SaveGamePath { get; set; }
        public static ContentManager Content { get; set; }
        public static GameStartDescription GameStartParams { get; private set; }
        public static void StartNewGame(string contentPath)
        {
            SaveGamePath = contentPath;
            GameStartParams = new GameStartDescription();
            GameStartParams.PlayerContentNames=new System.Collections.Generic.List<string>() { "Kolatt" };
            GameStartParams.QuestLineContentName = "MainQuestLine";
        }
        public static void TestSaveGameDescription(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameStartDescription));
            StreamWriter stream = new StreamWriter(SaveGamePath + "/"+filename +".xml");
            serializer.Serialize(stream, GameStartParams);
            stream.Close();
        }               
        public static void TestSaveMapPosition()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MyMap));
            StreamWriter stream = new StreamWriter(SaveGamePath + "/TestMap.xml");
            MyMap myMap = new MyMap();
            myMap.Position = new Vector2(10, 10);
            myMap.TileMask = new int[] { 10, 20, 30, 40, 46 };
            myMap.TileValue = @"
0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0
0   0   0   0   0   1   1   1   1   1   1   1   1   0   0   0   0   0   0   0
0   0   0   0   0   1   1   0   0   0   0   0   1   0   0   0   0   0   0   0
0   0   0   0   0   1   1   0   0   0   0   0   1   0   0   0   0   0   0   0
0   0   0   0   0   1   1   1   1   1   1   1   1   0   0   0   0   0   0   0
0   0   0   0   0   1   1   1   1   1   1   1   1   0   0   0   0   0   0   0
0   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1   1   0
1   1   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   0   1   1
1   1   1   1   0   0   0   0   0   0   0   0   0   0   0   0   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   1   1   0   0   0   0   1   1   1   1   1   1   1   1
1   1   1   1   1   1   1   1   0   0   0   0   1   1   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   1   0   0   1   1   1   1   1   1
1   1   1   1   1   1   0   0   0   0   0   0   0   0   1   1   1   1   1   1
1   1   1   1   1   1   1   1   0   0   0   0   1   1   1   1   1   1   1   1
1   1   1   1   1   1   1   1   0   0   0   0   1   1   1   1   1   1   1   1
1   1   1   1   1   1   1   1   0   0   0   0   1   1   1   1   1   1   1   1
";
            serializer.Serialize(stream, myMap);
            stream.Close();
        }
        public static Map TestGetMapData(string filename)
        {
            Map map = new Map();
            var doc = new TestReader<XmlDocument>(SaveGamePath).ReadXml(filename);
            XmlHelper.RootPath = "Map/";
            map.Name = XmlHelper.ReadText(doc,"Name");
            map.MapDimensions = XmlHelper.ReadPoint(doc, "MapDimensions");
            map.TileSize = XmlHelper.ReadPoint(doc, "TileSize");
            map.SpawnMapPosition = XmlHelper.ReadPoint(doc, "SpawnMapPosition");
            map.TextureName = XmlHelper.ReadText(doc, "TextureName");
            map.CombatMusicCueName = XmlHelper.ReadText(doc, "CombatMusicCueName");
            map.CombatTextureName = XmlHelper.ReadText(doc, "CombatTextureName");
            map.MusicCueName = XmlHelper.ReadText(doc, "MusicCueName");
            map.BaseLayer = XmlHelper.ReadIntMatrix(doc, "BaseLayer");
            map.FringeLayer = XmlHelper.ReadIntMatrix(doc, "FringeLayer");
            map.ObjectLayer = XmlHelper.ReadIntMatrix(doc, "ObjectLayer");
            map.CollisionLayer = XmlHelper.ReadIntMatrix(doc, "CollisionLayer");
            return map;
        }
        public static Map GetMapData(string filename)
        {
            Map map = new Map();
            var doc = XmlHelper.ReadXml("Content/" + filename);
            XmlHelper.RootPath = "/XnaContent/Asset/";
            map.Name = XmlHelper.ReadText(doc, "Name");
            map.MapDimensions = XmlHelper.ReadPoint(doc, "MapDimensions");
            map.TileSize = XmlHelper.ReadPoint(doc, "TileSize");
            map.SpawnMapPosition = XmlHelper.ReadPoint(doc, "SpawnMapPosition");
            map.TextureName = XmlHelper.ReadText(doc, "TextureName");
            map.Texture= Content.Load<Texture2D>(
                    @"Texture\Maps\NonCombat\"+
                    map.TextureName);
            map.TilesPerRow = map.Texture.Width / map.TileSize.X;
            map.CombatMusicCueName = XmlHelper.ReadText(doc, "CombatMusicCueName");
            map.CombatTextureName = XmlHelper.ReadText(doc, "CombatTextureName");
            map.CombatTexture=Content.Load<Texture2D>(
                    @"Texture\Maps\Combat\"+
                    map.CombatTextureName);
            map.MusicCueName = XmlHelper.ReadText(doc, "MusicCueName");
            map.BaseLayer = XmlHelper.ReadIntMatrix(doc, "BaseLayer");
            map.FringeLayer = XmlHelper.ReadIntMatrix(doc, "FringeLayer");
            map.ObjectLayer = XmlHelper.ReadIntMatrix(doc, "ObjectLayer");
            map.CollisionLayer = XmlHelper.ReadIntMatrix(doc, "CollisionLayer");
            map.Portals = XmlHelper.ReadArrayPortal(doc, "Portals");
            map.PortalEntries = XmlHelper.ReadArrayPortalEntry(doc, "PortalEntries");
            foreach (MapEntry<Portal> portalEntry in map.PortalEntries)
            {
                portalEntry.Content = map.Portals.Find(delegate (Portal portal)
                {
                    return (portal.Name == portalEntry.ContentName);
                });
            }
            map.ChestEntries = XmlHelper.ReadArrayChestEntry(doc, "ChestEntries");
            foreach (MapEntry<Chest> chestEntry in map.ChestEntries)
            {
                chestEntry.Content = GetChest(chestEntry.ContentName);
                chestEntry.Content.Texture =Content.Load<Texture2D>(
                    Path.Combine(@"Texture\Chests", chestEntry.Content.TextureName));
            }
            map.FixedCombatEntries = new List<MapEntry<FixedCombat>>();
            map.QuestNpcEntries = new List<MapEntry<QuestNpc>>();
            map.PlayerNpcEntries = new List<MapEntry<Player>>();
            map.InnEntries = new List<MapEntry<Inn>>();
            map.StoreEntries = new List<MapEntry<Store>>();
            map.RandomCombat = new RandomCombat();
            return map;
        }
        public static Chest GetChest(string filename)
        {
            Chest data = new Chest();
            var doc = XmlHelper.ReadXml("Content/Maps/Chests/" + filename);
            XmlHelper.RootPath = "/XnaContent/Asset/";
            data.Name = XmlHelper.ReadText(doc, "Name");
            data.Gold = Convert.ToInt32(XmlHelper.ReadText(doc, "Gold"));
            data.TextureName = XmlHelper.ReadText(doc, "TextureName");
            var nodes = doc.SelectNodes(XmlHelper.RootPath + "Entries/Item");
            if (nodes.Count > 0)
            {
                var lst = new List<ContentEntry<Gear>>();
                foreach(XmlNode node in nodes)
                {
                    ContentEntry<Gear> item = new ContentEntry<Gear>();
                    item.ContentName = node.ChildNodes[0].InnerText;
                    item.Content = GetItem(item.ContentName);
                    item.Count = Convert.ToInt32(node.ChildNodes[1].InnerText);
                    lst.Add(item);
                }
                data.Entries = lst;
            }
            return data;

        }
        public static GameStartDescription GetStartDescription(string filename)
        {
            GameStartDescription data = new GameStartDescription();
            var doc = XmlHelper.ReadXml("Content/"+filename);
            XmlHelper.RootPath = "/XnaContent/Asset/";
            data.MapContentName = XmlHelper.ReadText(doc,"MapContentName");
            data.QuestLineContentName = XmlHelper.ReadText(doc, "QuestLineContentName");
            data.PlayerContentNames = XmlHelper.ReadArrayText(doc, "PlayerContentNames");            
            return data;
        }
        public static Item GetItem(string filename)
        {
            Item data = new Item();
            var doc = XmlHelper.ReadXml("Content/Gear/" + filename);
            XmlHelper.RootPath = "/XnaContent/Asset/";
            //base case of Gear
            data.Name = XmlHelper.ReadText(doc,"Name");
            data.Description = XmlHelper.ReadText(doc, "Description");
            data.GoldValue = System.Convert.ToInt32(XmlHelper.ReadText(doc, "GoldValue"));
            data.IsDroppable = XmlHelper.ReadText(doc, "IsDroppable") == "true";
            data.MinimumCharacterLevel= System.Convert.ToInt32(XmlHelper.ReadText(doc, "MinimumCharacterLevel"));
            data.SupportedClasses = XmlHelper.ReadArrayText(doc, "SupportedClasses");
            data.IconTextureName = XmlHelper.ReadText(doc, "IconTextureName");
            data.IconTexture=Content.Load<Texture2D>(
                    Path.Combine(@"Texture\Gear", data.IconTextureName));
            //Item Detail
            if(XmlHelper.ReadText(doc, "Usage").IndexOf("Combat") >= 0)
            {
                data.Usage = Item.ItemUsage.Combat;
            }
            if (XmlHelper.ReadText(doc, "Usage").IndexOf("NonCombat") >= 0)
            {
                data.Usage &= Item.ItemUsage.NonCombat;
            }
            data.IsOffensive = XmlHelper.ReadText(doc, "IsOffensive") == "true";
            data.TargetDuration= System.Convert.ToInt32(XmlHelper.ReadText(doc, "TargetDuration"));
            data.TargetEffectRange = XmlHelper.GetStatisticsRange(doc,"TargetEffectRange");
            data.AdjacentTargets= System.Convert.ToInt32(XmlHelper.ReadText(doc, "AdjacentTargets"));
            data.UsingCueName = XmlHelper.ReadText(doc, "UsingCueName");
            data.TravelingCueName = XmlHelper.ReadText(doc, "TravelingCueName");
            data.ImpactCueName = XmlHelper.ReadText(doc, "ImpactCueName");
            data.BlockCueName = XmlHelper.ReadText(doc, "BlockCueName");
            data.CreationSprite = XmlHelper.GetAnimatingSprite(doc, "CreationSprite");
            data.CreationSprite.Texture = Content.Load<Texture2D>(
                        System.IO.Path.Combine(@"Textures",
                        data.CreationSprite.TextureName)
                        );
            data.SpellSprite = XmlHelper.GetAnimatingSprite(doc, "SpellSprite");
            data.SpellSprite.Texture = Content.Load<Texture2D>(
                        System.IO.Path.Combine(@"Textures",
                        data.SpellSprite.TextureName)
                        );
            data.Overlay = XmlHelper.GetAnimatingSprite(doc, "Overlay");
            data.Overlay.Texture = Content.Load<Texture2D>(
                        System.IO.Path.Combine(@"Textures",
                        data.Overlay.TextureName)
                        );
            data.CreationSprite.SourceOffset = new Vector2(
                    data.CreationSprite.FrameDimensions.X / 2,
                    data.CreationSprite.FrameDimensions.Y);
            data.SpellSprite.SourceOffset = new Vector2(
                    data.SpellSprite.FrameDimensions.X / 2,
                    data.SpellSprite.FrameDimensions.Y);
            data.Overlay.SourceOffset = new Vector2(
                    data.Overlay.FrameDimensions.X / 2,
                    data.Overlay.FrameDimensions.Y);
            return data;
        }
    }
    public static class XmlHelper
    {
        public static string RootPath {get;set;}
        public static XmlDocument ReadXml(string fname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fname + ".xml");
            return doc;
        }
        public static AnimatingSprite GetAnimatingSprite(XmlDocument doc,string path)
        {
            AnimatingSprite data = new AnimatingSprite();
            var elem = doc.SelectSingleNode(RootPath + path + "/TextureName");
            if (elem!=null)
            {
                data.TextureName = elem.InnerText;
            }
            elem = doc.SelectSingleNode(RootPath + path + "/FrameDimensions");
            if (elem != null)
            {
                data.FrameDimensions = GetPoint(elem);
            }
            elem = doc.SelectSingleNode(RootPath + path + "/FramePerRow");
            if (elem != null)
            {
                data.FramesPerRow = Convert.ToInt32(elem.InnerText);
            }
            elem = doc.SelectSingleNode(RootPath + path + "/SourceOffSet");
            if (elem != null)
            {
                data.SourceOffset= GetPoint(elem).ToVector2();
            }
            var nodes = doc.SelectNodes(RootPath + path + "/Animations/Item");
            if (nodes.Count > 0)
            {
                List<Animation> lst = new List<Animation>();
                foreach(XmlNode node in nodes)
                {
                    Animation anime = new Animation();
                    anime.Name = node.ChildNodes[0].InnerText;
                    anime.StartingFrame = Convert.ToInt32(node.ChildNodes[1].InnerText);
                    anime.EndingFrame= Convert.ToInt32(node.ChildNodes[2].InnerText);
                    anime.Interval= Convert.ToInt32(node.ChildNodes[3].InnerText);
                    anime.IsLoop = node.ChildNodes[4].InnerText == "true";
                    lst.Add(anime);
                }
                data.Animations = lst;
            }
            return data;
        }
        public static StatisticsRange GetStatisticsRange(XmlDocument doc,string path)
        {
            StatisticsRange data = new StatisticsRange();
            var nodes = doc.SelectNodes(RootPath + path+"/HealthPointsRange");
            if (nodes.Count > 0)
            {
                data.HealthPointsRange = GetInt32Range(nodes[0]);
            }
            nodes = doc.SelectNodes(RootPath + path + "/MagicPointsRange");
            if (nodes.Count > 0)
            {
                data.MagicPointsRange = GetInt32Range(nodes[0]);
            }
            nodes = doc.SelectNodes(RootPath + path + "/PhysicalOffenseRange");
            if (nodes.Count > 0)
            {
                data.PhysicalOffenseRange = GetInt32Range(nodes[0]);
            }
            nodes = doc.SelectNodes(RootPath + path + "/PhysicalDefenseRange");
            if (nodes.Count > 0)
            {
                data.PhysicalDefenseRange = GetInt32Range(nodes[0]);
            }
            nodes = doc.SelectNodes(RootPath + path + "/MagicalOffenseRange");
            if (nodes.Count > 0)
            {
                data.MagicalOffenseRange = GetInt32Range(nodes[0]);
            }
            nodes = doc.SelectNodes(RootPath + path + "/MagicalDefenseRange");
            if (nodes.Count > 0)
            {
                data.MagicalDefenseRange = GetInt32Range(nodes[0]);
            }
            return data;
        }
        public static Int32Range GetInt32Range(XmlNode node)
        {
            return new Int32Range(
                    System.Convert.ToInt32(node.ChildNodes[0].InnerText),
                    System.Convert.ToInt32(node.ChildNodes[1].InnerText)
                    );
        }
        public static List<MapEntry<Chest>> ReadArrayChestEntry(XmlDocument doc,string path)
        {
            List<MapEntry<Chest>> lst = new List<MapEntry<Chest>>();
            var nodes = doc.SelectNodes(RootPath + path + "/Item");
            if (nodes.Count > 0)
            {
                foreach(XmlNode node in nodes)
                {
                    MapEntry<Chest> data = new MapEntry<Chest>();
                    data.ContentName = node.ChildNodes[0].InnerText;
                    data.MapPosition = GetPoint(node.ChildNodes[1]);
                }
            }
            return lst;
        }
        public static List<Portal> ReadArrayPortal(XmlDocument doc,string path)
        {
            List<Portal> lst = new List<Portal>();
            var nodes = doc.SelectNodes(RootPath + path + "/Item");
            if (nodes.Count>0)
            {
                foreach (XmlNode node in nodes)
                {
                    Portal data = new Portal();
                    data.Name = node.ChildNodes[0].InnerText;
                    data.LandingMapPosition = GetPoint(node.ChildNodes[1]);
                    data.DestinationMapContentName = node.ChildNodes[2].InnerText;
                    data.DestinationMapPortalName = node.ChildNodes[3].InnerText;
                    lst.Add(data);
                }
            }            
            return lst;
        }
        public static List<MapEntry<Portal>> ReadArrayPortalEntry(XmlDocument doc, string path)
        {
            List<MapEntry<Portal>> lst = new List<MapEntry<Portal>>();
            var nodes = doc.SelectNodes(RootPath + path + "/Item");
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    MapEntry<Portal> data = new MapEntry<Portal>();
                    data.ContentName = node.ChildNodes[0].InnerText;
                    data.MapPosition = GetPoint(node.ChildNodes[1]);
                    lst.Add(data);
                }
            }
            return lst;
        }
        public static List<string> ReadArrayText(XmlDocument doc,string path)
        {
            List<string> lst = new List<string>();
            var nodes = doc.SelectNodes(RootPath + path);
            if (nodes.Count > 0)
            {
                foreach (XmlNode node in nodes)
                {
                    lst.Add(node.InnerText);
                }
            }            
            return lst;
        }
        public static int[] ReadIntMatrix(XmlDocument doc,string path)
        {
            System.Collections.Generic.List<int> lst = new System.Collections.Generic.List<int>();
            XmlNode elem = doc.SelectSingleNode(RootPath +path);
            if (elem != null)
            {
                var data = elem.InnerText;
                data = data.Replace("\n", "X");
                data = data.Replace(" ", "X");
                var tmp = "";
                var c = 0;
                for (int i = data.Length - 1; i >= 0; i--)
                {
                    if ((c > 0 && data.Substring(i, 1) == "X") || (i == 0 && tmp != ""))
                    {
                        lst.Add(System.Convert.ToInt32(tmp.Trim()));
                        tmp = "";
                        c = 0;
                    }
                    if (data.Substring(i, 1) != "X")
                    {
                        tmp = data.Substring(i, 1) + tmp;
                        c += 1;
                    }
                }
                lst.Reverse();
            }            
            return lst.ToArray();
        }
        public static string ReadText(XmlDocument doc,string path)
        {
            XmlNode elem = doc.SelectSingleNode(RootPath + path);
            if (elem != null)
            {
                return elem.InnerText;
            }
            return string.Empty;
        }
        public static Point ReadPoint(XmlDocument doc,string path)
        {
            XmlNode elem = doc.SelectSingleNode(RootPath + path);
            if (elem != null)
            {
                return GetPoint(elem);
            }
            return new Point();
        }
        public static Point GetPoint(XmlNode elem)
        {            
            return new Point(
                System.Convert.ToInt32(elem.InnerText.Split(' ')[0]),
                System.Convert.ToInt32(elem.InnerText.Split(' ')[1])
                );
        }
    }
    public class TestReader<T>
    {
        XmlSerializer serializer;
        string contentPath;
        System.Type type;
        StreamReader reader;
        public TestReader(string path)
        {
            type = typeof(T);
            contentPath = path;
            serializer = new XmlSerializer(type);
        }
        public T Read(string fname)
        {
            reader = new StreamReader(contentPath + "/" + fname+".xml");
            return (T)serializer.Deserialize(reader);
        }
        public XmlDocument ReadXml(string fname)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(contentPath + "/" + fname + ".xml");
            return doc;
        }
    }
}
