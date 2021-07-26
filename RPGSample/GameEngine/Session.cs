using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RPGSample
{
    class Session
    {
        /// <summary>
        /// The random-number generator used with game events.
        /// </summary>
        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
            private set { random = value; }
        }
        /// <summary>
        /// The main quest line for this session.
        /// </summary>
        private QuestLine questLine;

        /// <summary>
        /// The main quest line for this session.
        /// </summary>
        public static QuestLine QuestLine
        {
            get { return (singleton == null ? null : singleton.questLine); }
        }


        /// <summary>
        /// If true, the main quest line for this session is complete.
        /// </summary>
        public static bool IsQuestLineComplete
        {
            get
            {
                if ((singleton == null) || (singleton.questLine == null) ||
                    (singleton.questLine.QuestContentNames == null))
                {
                    return false;
                }
                return singleton.currentQuestIndex >=
                    singleton.questLine.QuestContentNames.Count;
            }
        }


        /// <summary>
        /// The current quest in this session.
        /// </summary>
        private Quest quest;

        /// <summary>
        /// The current quest in this session.
        /// </summary>
        public static Quest Quest
        {
            get { return (singleton == null ? null : singleton.quest); }
        }


        /// <summary>
        /// The index of the current quest into the quest line.
        /// </summary>
        private int currentQuestIndex = 0;

        /// <summary>
        /// The index of the current quest into the quest line.
        /// </summary>
        public static int CurrentQuestIndex
        {
            get { return (singleton == null ? -1 : singleton.currentQuestIndex); }
        }


        /// <summary>
        /// Update the current quest and quest line for this session.
        /// </summary>
        public void UpdateQuest()
        {
            // check the singleton's state to see if we should care about quests
            if ((party == null) || (questLine == null))
            {
                return;
            }

            // if we don't have a quest, then take the next one from teh list
            if ((quest == null) && (questLine.Quests.Count > 0) &&
                !Session.IsQuestLineComplete)
            {
                quest = questLine.Quests[currentQuestIndex];
                quest.Stage = Quest.QuestStage.NotStarted;
                // clear the monster-kill record
                party.MonsterKills.Clear();
                // clear the modified-quest lists
                modifiedQuestChests.Clear();
                removedQuestChests.Clear();
                removedQuestFixedCombats.Clear();
            }

            // handle quest-stage transitions
            if ((quest != null) && !Session.IsQuestLineComplete)
            {
                switch (quest.Stage)
                {
                    case Quest.QuestStage.NotStarted:
                        // start the new quest
                        quest.Stage = Quest.QuestStage.InProgress;
                        if (!quest.AreRequirementsMet)
                        {
                            // show the announcement of the quest and the requirements
                            ScreenManager.AddScreen(new QuestLogScreen(quest));
                        }
                        break;

                    case Quest.QuestStage.InProgress:
                        // update monster requirements
                        foreach (QuestRequirement<Monster> monsterRequirement in
                            quest.MonsterRequirements)
                        {
                            monsterRequirement.CompletedCount = 0;
                            Monster monster = monsterRequirement.Content;
                            if (party.MonsterKills.ContainsKey(monster.AssetName))
                            {
                                monsterRequirement.CompletedCount =
                                    party.MonsterKills[monster.AssetName];
                            }
                        }
                        // update gear requirements
                        foreach (QuestRequirement<Gear> gearRequirement in
                            quest.GearRequirements)
                        {
                            gearRequirement.CompletedCount = 0;
                            foreach (ContentEntry<Gear> entry in party.Inventory)
                            {
                                if (entry.Content == gearRequirement.Content)
                                {
                                    gearRequirement.CompletedCount += entry.Count;
                                }
                            }
                        }
                        // check to see if the requirements have been met
                        if (quest.AreRequirementsMet)
                        {
                            // immediately remove the gear
                            foreach (QuestRequirement<Gear> gearRequirement in
                                quest.GearRequirements)
                            {
                                Gear gear = gearRequirement.Content;
                                party.RemoveFromInventory(gear,
                                    gearRequirement.Count);
                            }
                            // check to see if there is a destination
                            if (String.IsNullOrEmpty(
                                quest.DestinationMapContentName))
                            {
                                // complete the quest
                                quest.Stage = Quest.QuestStage.Completed;
                                // show the completion dialogue
                                if (!String.IsNullOrEmpty(quest.CompletionMessage))
                                {
                                    DialogueScreen dialogueScreen = new DialogueScreen();
                                    dialogueScreen.TitleText = "Quest Complete";
                                    dialogueScreen.BackText = String.Empty;
                                    dialogueScreen.DialogueText =
                                        quest.CompletionMessage;
                                    ScreenManager.AddScreen(dialogueScreen);
                                }
                            }
                            else
                            {
                                quest.Stage = Quest.QuestStage.RequirementsMet;
                                // remind the player about the destination
                                screenManager.AddScreen(new QuestLogScreen(quest));
                            }
                        }
                        break;

                    case Quest.QuestStage.RequirementsMet:
                        break;

                    case Quest.QuestStage.Completed:
                        // show the quest rewards screen
                        RewardsScreen rewardsScreen =
                            new RewardsScreen(RewardsScreen.RewardScreenMode.Quest,
                            Quest.ExperienceReward, Quest.GoldReward, Quest.GearRewards);
                        screenManager.AddScreen(rewardsScreen);
                        // advance to the next quest
                        currentQuestIndex++;
                        quest = null;
                        break;
                }
            }
        }
        /// <summary>
        /// The heads-up-display menu shown on the map and combat screens.
        /// </summary>
        private Hud hud;

        /// <summary>
        /// The heads-up-display menu shown on the map and combat screens.
        /// </summary>
        public static Hud Hud
        {
            get { return (singleton == null ? null : singleton.hud); }
        }
        private static Session singleton;
        /// <summary>
        /// The party that is playing the game right now.
        /// </summary>
        private Party party;

        /// <summary>
        /// The party that is playing the game right now.
        /// </summary>
        public static Party Party
        {
            get { return (singleton == null ? null : singleton.party); }
        }

        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        private ScreenManager screenManager;

        /// <summary>
        /// The ScreenManager used to manage all UI in the game.
        /// </summary>
        public static ScreenManager ScreenManager
        {
            get { return (singleton == null ? null : singleton.screenManager); }
        }
        /// <summary>
        /// Returns true if there is an active session.
        /// </summary>
        public static bool IsActive
        {
            get { return singleton != null; }
        }
        /// <summary>
        /// Check if a random combat should start.  If so, start combat immediately.
        /// </summary>
        /// <returns>True if combat was started, false otherwise.</returns>
        public static bool CheckForRandomCombat(RandomCombat randomCombat)
        {
            // check the parameter
            if ((randomCombat == null) || (randomCombat.CombatProbability <= 0))
            {
                return false;
            }

            // check to see if combat has already started
            if (CombatEngine.IsActive)
            {
                return false;
            }

            // check to see if the random combat starts
            int randomCombatCheck = random.Next(100);
            if (randomCombatCheck < randomCombat.CombatProbability)
            {
                // start combat immediately
                CombatEngine.StartNewCombat(randomCombat);
                return true;
            }

            // combat not started
            return false;
        }
        
        /// <summary>
        /// The chests removed from the map asset by player actions.
        /// </summary>
        private List<WorldEntry<Chest>> removedMapChests =
            new List<WorldEntry<Chest>>();

        /// <summary>
        /// The chests removed from the current quest asset by player actions.
        /// </summary>
        private List<WorldEntry<Chest>> removedQuestChests =
            new List<WorldEntry<Chest>>();

        /// <summary>
        /// Remove the given chest entry from the current map or quest.
        /// </summary>
        public static void RemoveChest(MapEntry<Chest> mapEntry)
        {
            // check the parameter
            if (mapEntry == null)
            {
                return;
            }

            // check the map for the item first
            if (TileEngine.Map != null)
            {
                int removedEntries = TileEngine.Map.ChestEntries.RemoveAll(
                    delegate (MapEntry<Chest> entry)
                    {
                        return ((entry.ContentName == mapEntry.ContentName) &&
                            (entry.MapPosition == mapEntry.MapPosition));
                    });
                if (removedEntries > 0)
                {
                    WorldEntry<Chest> worldEntry = new WorldEntry<Chest>();
                    worldEntry.Content = mapEntry.Content;
                    worldEntry.ContentName = mapEntry.ContentName;
                    worldEntry.Count = mapEntry.Count;
                    worldEntry.Direction = mapEntry.Direction;
                    worldEntry.MapContentName = TileEngine.Map.AssetName;
                    worldEntry.MapPosition = mapEntry.MapPosition;
                    singleton.removedMapChests.Add(worldEntry);
                    return;
                }
            }

            // look for the map entry within the quest
            if (singleton.quest != null)
            {
                int removedEntries = singleton.quest.ChestEntries.RemoveAll(
                    delegate (WorldEntry<Chest> entry)
                    {
                        return ((entry.ContentName == mapEntry.ContentName) &&
                            (entry.MapPosition == mapEntry.MapPosition) &&
                            TileEngine.Map.AssetName.EndsWith(entry.MapContentName));
                    });
                if (removedEntries > 0)
                {
                    WorldEntry<Chest> worldEntry = new WorldEntry<Chest>();
                    worldEntry.Content = mapEntry.Content;
                    worldEntry.ContentName = mapEntry.ContentName;
                    worldEntry.Count = mapEntry.Count;
                    worldEntry.Direction = mapEntry.Direction;
                    worldEntry.MapContentName = TileEngine.Map.AssetName;
                    worldEntry.MapPosition = mapEntry.MapPosition;
                    singleton.removedQuestChests.Add(worldEntry);
                    return;
                }
            }
        }


        /// <summary>
        /// The fixed-combats removed from the map asset by player actions.
        /// </summary>
        private List<WorldEntry<FixedCombat>> removedMapFixedCombats =
            new List<WorldEntry<FixedCombat>>();

        /// <summary>
        /// The fixed-combats removed from the current quest asset by player actions.
        /// </summary>
        private List<WorldEntry<FixedCombat>> removedQuestFixedCombats =
            new List<WorldEntry<FixedCombat>>();

        /// <summary>
        /// Remove the given fixed-combat entry from the current map or quest.
        /// </summary>
        public static void RemoveFixedCombat(MapEntry<FixedCombat> mapEntry)
        {
            // check the parameter
            if (mapEntry == null)
            {
                return;
            }

            // check the map for the item first
            if (TileEngine.Map != null)
            {
                int removedEntries = TileEngine.Map.FixedCombatEntries.RemoveAll(
                    delegate (MapEntry<FixedCombat> entry)
                    {
                        return ((entry.ContentName == mapEntry.ContentName) &&
                            (entry.MapPosition == mapEntry.MapPosition));
                    });
                if (removedEntries > 0)
                {
                    WorldEntry<FixedCombat> worldEntry = new WorldEntry<FixedCombat>();
                    worldEntry.Content = mapEntry.Content;
                    worldEntry.ContentName = mapEntry.ContentName;
                    worldEntry.Count = mapEntry.Count;
                    worldEntry.Direction = mapEntry.Direction;
                    worldEntry.MapContentName = TileEngine.Map.AssetName;
                    worldEntry.MapPosition = mapEntry.MapPosition;
                    singleton.removedMapFixedCombats.Add(worldEntry);
                    return;
                }
            }

            // look for the map entry within the quest
            if (singleton.quest != null)
            {
                int removedEntries = singleton.quest.FixedCombatEntries.RemoveAll(
                    delegate (WorldEntry<FixedCombat> entry)
                    {
                        return ((entry.ContentName == mapEntry.ContentName) &&
                            (entry.MapPosition == mapEntry.MapPosition) &&
                            TileEngine.Map.AssetName.EndsWith(entry.MapContentName));
                    });
                if (removedEntries > 0)
                {
                    WorldEntry<FixedCombat> worldEntry = new WorldEntry<FixedCombat>();
                    worldEntry.Content = mapEntry.Content;
                    worldEntry.ContentName = mapEntry.ContentName;
                    worldEntry.Count = mapEntry.Count;
                    worldEntry.Direction = mapEntry.Direction;
                    worldEntry.MapContentName = TileEngine.Map.AssetName;
                    worldEntry.MapPosition = mapEntry.MapPosition;
                    singleton.removedQuestFixedCombats.Add(worldEntry);
                    return;
                }
            }
        }


        /// <summary>
        /// The player NPCs removed from the map asset by player actions.
        /// </summary>
        private List<WorldEntry<Player>> removedMapPlayerNpcs =
            new List<WorldEntry<Player>>();

        /// <summary>
        /// Remove the given player NPC entry from the current map or quest.
        /// </summary>
        public static void RemovePlayerNpc(MapEntry<Player> mapEntry)
        {
            // check the parameter
            if (mapEntry == null)
            {
                return;
            }

            // check the map for the item
            if (TileEngine.Map != null)
            {
                int removedEntries = TileEngine.Map.PlayerNpcEntries.RemoveAll(
                    delegate (MapEntry<Player> entry)
                    {
                        return ((entry.ContentName == mapEntry.ContentName) &&
                            (entry.MapPosition == mapEntry.MapPosition));
                    });
                if (removedEntries > 0)
                {
                    WorldEntry<Player> worldEntry = new WorldEntry<Player>();
                    worldEntry.Content = mapEntry.Content;
                    worldEntry.ContentName = mapEntry.ContentName;
                    worldEntry.Count = mapEntry.Count;
                    worldEntry.Direction = mapEntry.Direction;
                    worldEntry.MapContentName = TileEngine.Map.AssetName;
                    worldEntry.MapPosition = mapEntry.MapPosition;
                    singleton.removedMapPlayerNpcs.Add(worldEntry);
                    return;
                }
            }

            // quests don't have a list of player NPCs
        }


        /// <summary>
        /// The chests that have been modified, but not emptied, by player action.
        /// </summary>
        private List<ModifiedChestEntry> modifiedMapChests =
            new List<ModifiedChestEntry>();


        /// <summary>
        /// The chests belonging to the current quest that have been modified,
        /// but not emptied, by player action.
        /// </summary>
        private List<ModifiedChestEntry> modifiedQuestChests =
            new List<ModifiedChestEntry>();


        /// <summary>
        /// Stores the entry for a chest on the current map or quest that has been
        /// modified but not emptied.
        /// </summary>
        public static void StoreModifiedChest(MapEntry<Chest> mapEntry)
        {
            // check the parameter
            if ((mapEntry == null) || (mapEntry.Content == null))
            {
                throw new ArgumentNullException("mapEntry");
            }

            Predicate<ModifiedChestEntry> checkModifiedChests =
                delegate (ModifiedChestEntry entry)
                {
                    return
                        (TileEngine.Map.AssetName.EndsWith(
                            entry.WorldEntry.MapContentName) &&
                        (entry.WorldEntry.ContentName == mapEntry.ContentName) &&
                        (entry.WorldEntry.MapPosition == mapEntry.MapPosition));
                };

            // check the map for the item first
            if ((TileEngine.Map != null) && TileEngine.Map.ChestEntries.Exists(
                delegate (MapEntry<Chest> entry)
                {
                    return ((entry.ContentName == mapEntry.ContentName) &&
                        (entry.MapPosition == mapEntry.MapPosition));
                }))
            {
                singleton.modifiedMapChests.RemoveAll(checkModifiedChests);
                ModifiedChestEntry modifiedChestEntry = new ModifiedChestEntry();
                modifiedChestEntry.WorldEntry.Content = mapEntry.Content;
                modifiedChestEntry.WorldEntry.ContentName = mapEntry.ContentName;
                modifiedChestEntry.WorldEntry.Count = mapEntry.Count;
                modifiedChestEntry.WorldEntry.Direction = mapEntry.Direction;
                modifiedChestEntry.WorldEntry.MapContentName =
                    TileEngine.Map.AssetName;
                modifiedChestEntry.WorldEntry.MapPosition = mapEntry.MapPosition;
                Chest chest = mapEntry.Content;
                modifiedChestEntry.ChestEntries.AddRange(chest.Entries);
                modifiedChestEntry.Gold = chest.Gold;
                singleton.modifiedMapChests.Add(modifiedChestEntry);
                return;
            }


            // look for the map entry within the quest
            if ((singleton.quest != null) && singleton.quest.ChestEntries.Exists(
                delegate (WorldEntry<Chest> entry)
                {
                    return ((entry.ContentName == mapEntry.ContentName) &&
                        (entry.MapPosition == mapEntry.MapPosition) &&
                        TileEngine.Map.AssetName.EndsWith(entry.MapContentName));
                }))
            {
                singleton.modifiedQuestChests.RemoveAll(checkModifiedChests);
                ModifiedChestEntry modifiedChestEntry = new ModifiedChestEntry();
                modifiedChestEntry.WorldEntry.Content = mapEntry.Content;
                modifiedChestEntry.WorldEntry.ContentName = mapEntry.ContentName;
                modifiedChestEntry.WorldEntry.Count = mapEntry.Count;
                modifiedChestEntry.WorldEntry.Direction = mapEntry.Direction;
                modifiedChestEntry.WorldEntry.MapContentName = TileEngine.Map.AssetName;
                modifiedChestEntry.WorldEntry.MapPosition = mapEntry.MapPosition;
                Chest chest = mapEntry.Content;
                modifiedChestEntry.ChestEntries.AddRange(chest.Entries);
                modifiedChestEntry.Gold = chest.Gold;
                singleton.modifiedQuestChests.Add(modifiedChestEntry);
                return;
            }
        }


        /// <summary>
        /// Remove the specified content from the map, typically from an earlier session.
        /// </summary>
        private void ModifyMap(Map map)
        {
            // check the parameter
            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            // remove all chests that were emptied already
            if ((removedMapChests != null) && (removedMapChests.Count > 0))
            {
                // check each removed-content entry
                map.ChestEntries.RemoveAll(delegate (MapEntry<Chest> mapEntry)
                {
                    return (removedMapChests.Exists(
                        delegate (WorldEntry<Chest> removedEntry)
                        {
                            return
                                (map.AssetName.EndsWith(removedEntry.MapContentName) &&
                                (removedEntry.ContentName == mapEntry.ContentName) &&
                                (removedEntry.MapPosition == mapEntry.MapPosition));
                        }));
                });
            }

            // remove all fixed-combats that were defeated already
            if ((removedMapFixedCombats != null) && (removedMapFixedCombats.Count > 0))
            {
                // check each removed-content entry
                map.FixedCombatEntries.RemoveAll(delegate (MapEntry<FixedCombat> mapEntry)
                {
                    return (removedMapFixedCombats.Exists(
                        delegate (WorldEntry<FixedCombat> removedEntry)
                        {
                            return
                                (map.AssetName.EndsWith(removedEntry.MapContentName) &&
                                (removedEntry.ContentName == mapEntry.ContentName) &&
                                (removedEntry.MapPosition == mapEntry.MapPosition));
                        }));
                });
            }

            // remove the player NPCs that have already joined the party
            if ((removedMapPlayerNpcs != null) && (removedMapPlayerNpcs.Count > 0))
            {
                // check each removed-content entry
                map.PlayerNpcEntries.RemoveAll(delegate (MapEntry<Player> mapEntry)
                {
                    return (removedMapPlayerNpcs.Exists(
                        delegate (WorldEntry<Player> removedEntry)
                        {
                            return
                                (map.AssetName.EndsWith(removedEntry.MapContentName) &&
                                (removedEntry.ContentName == mapEntry.ContentName) &&
                                (removedEntry.MapPosition == mapEntry.MapPosition));
                        }));
                });
            }

            // replace the chest entries of modified chests - they are already clones
            if ((modifiedMapChests != null) && (modifiedMapChests.Count > 0))
            {
                foreach (MapEntry<Chest> entry in map.ChestEntries)
                {
                    ModifiedChestEntry modifiedEntry = modifiedMapChests.Find(
                        delegate (ModifiedChestEntry modifiedTestEntry)
                        {
                            return
                                (map.AssetName.EndsWith(
                                    modifiedTestEntry.WorldEntry.MapContentName) &&
                                (modifiedTestEntry.WorldEntry.ContentName ==
                                    entry.ContentName) &&
                                (modifiedTestEntry.WorldEntry.MapPosition ==
                                    entry.MapPosition));
                        });
                    // if the chest has been modified, apply the changes
                    if (modifiedEntry != null)
                    {
                        ModifyChest(entry.Content, modifiedEntry);
                    }
                }
            }
        }


        /// <summary>
        /// Remove the specified content from the map, typically from an earlier session.
        /// </summary>
        private void ModifyQuest(Quest quest)
        {
            // check the parameter
            if (quest == null)
            {
                throw new ArgumentNullException("quest");
            }

            // remove all chests that were emptied arleady
            if ((removedQuestChests != null) && (removedQuestChests.Count > 0))
            {
                // check each removed-content entry
                quest.ChestEntries.RemoveAll(
                    delegate (WorldEntry<Chest> worldEntry)
                    {
                        return (removedQuestChests.Exists(
                            delegate (WorldEntry<Chest> removedEntry)
                            {
                                return
                                    (removedEntry.MapContentName.EndsWith(
                                        worldEntry.MapContentName) &&
                                    (removedEntry.ContentName ==
                                        worldEntry.ContentName) &&
                                    (removedEntry.MapPosition ==
                                        worldEntry.MapPosition));
                            }));
                    });
            }

            // remove all of the fixed-combats that have already been defeated
            if ((removedQuestFixedCombats != null) &&
                (removedQuestFixedCombats.Count > 0))
            {
                // check each removed-content entry
                quest.FixedCombatEntries.RemoveAll(
                    delegate (WorldEntry<FixedCombat> worldEntry)
                    {
                        return (removedQuestFixedCombats.Exists(
                            delegate (WorldEntry<FixedCombat> removedEntry)
                            {
                                return
                                    (removedEntry.MapContentName.EndsWith(
                                        worldEntry.MapContentName) &&
                                    (removedEntry.ContentName ==
                                        worldEntry.ContentName) &&
                                    (removedEntry.MapPosition ==
                                        worldEntry.MapPosition));
                            }));
                    });
            }

            // replace the chest entries of modified chests - they are already clones
            if ((modifiedQuestChests != null) && (modifiedQuestChests.Count > 0))
            {
                foreach (WorldEntry<Chest> entry in quest.ChestEntries)
                {
                    ModifiedChestEntry modifiedEntry = modifiedQuestChests.Find(
                        delegate (ModifiedChestEntry modifiedTestEntry)
                        {
                            return
                                ((modifiedTestEntry.WorldEntry.MapContentName ==
                                    entry.MapContentName) &&
                                (modifiedTestEntry.WorldEntry.ContentName ==
                                    entry.ContentName) &&
                                (modifiedTestEntry.WorldEntry.MapPosition ==
                                    entry.MapPosition));
                        });
                    // if the chest has been modified, apply the changes
                    if (modifiedEntry != null)
                    {
                        ModifyChest(entry.Content, modifiedEntry);
                    }
                }
            }
        }


        /// <summary>
        /// Modify a Chest object based on the data in a ModifiedChestEntry object.
        /// </summary>
        private void ModifyChest(Chest chest, ModifiedChestEntry modifiedChestEntry)
        {
            // check the parameters
            if ((chest == null) || (modifiedChestEntry == null))
            {
                return;
            }

            // set the new gold amount
            chest.Gold = modifiedChestEntry.Gold;

            // remove all contents not found in the modified version
            chest.Entries.RemoveAll(delegate (ContentEntry<Gear> contentEntry)
            {
                return !modifiedChestEntry.ChestEntries.Exists(
                    delegate (ContentEntry<Gear> modifiedTestEntry)
                    {
                        return (contentEntry.ContentName ==
                            modifiedTestEntry.ContentName);
                    });
            });

            // set the new counts on the remaining content items
            foreach (ContentEntry<Gear> contentEntry in chest.Entries)
            {
                ContentEntry<Gear> modifiedGearEntry =
                    modifiedChestEntry.ChestEntries.Find(
                        delegate (ContentEntry<Gear> modifiedTestEntry)
                        {
                            return (contentEntry.ContentName ==
                                modifiedTestEntry.ContentName);
                        });
                if (modifiedGearEntry != null)
                {
                    contentEntry.Count = modifiedGearEntry.Count;
                }
            }
        }
        /// <summary>
        /// The GameplayScreen object that created this session.
        /// </summary>
        private GameplayScreen gameplayScreen;
        /// <summary>
        /// Save game descriptions for the current set of save games.
        /// </summary>
        private static List<SaveGameDescription> saveGameDescriptions = null;

        /// <summary>
        /// Save game descriptions for the current set of save games.
        /// </summary>
        public static List<SaveGameDescription> SaveGameDescriptions
        {
            get { return saveGameDescriptions; }
        }


        /// <summary>
        /// The maximum number of save-game descriptions that the list may hold.
        /// </summary>
        public const int MaximumSaveGameDescriptions = 5;
        public Session(ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            this.screenManager = screenManager;
            this.gameplayScreen = gameplayScreen;
        }

        /// <summary>
        /// End the current session.
        /// </summary>
        public static void EndSession()
        {
            // exit the gameplay screen
            // -- store the gameplay session, for re-entrance
            if (singleton != null)
            {
                GameplayScreen gameplayScreen = singleton.gameplayScreen;
                singleton.gameplayScreen = null;

                // pop the music
                AudioManager.PopMusic();

                // clear the singleton
                singleton = null;

                if (gameplayScreen != null)
                {
                    gameplayScreen.ExitScreen();
                }
            }
        }

        /// <summary>
        /// XML serializer for SaveGameDescription objects.
        /// </summary>
        private static XmlSerializer saveGameDescriptionSerializer =
            new XmlSerializer(typeof(SaveGameDescription));


        /// <summary>
        /// Refresh the list of save-game descriptions.
        /// </summary>
        public static void RefreshSaveGameDescriptions()
        {
            // clear the list
            saveGameDescriptions = null;

            // retrieve the storage device, asynchronously
            //GetStorageDevice(RefreshSaveGameDescriptionsResult);
        }

        /// <summary>
        /// Start a new session based on the data provided.
        /// </summary>
        public static void StartNewSession(GameStartDescription gameStartDescription,
            ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            
            // check the parameters
            if (gameStartDescription == null)
            {
                throw new ArgumentNullException("gameStartDescripton");
            }
            if (screenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (gameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            // end any existing session
            EndSession();

            // create a new singleton
            singleton = new Session(screenManager, gameplayScreen);

            // set up the initial map
            //ChangeMap(gameStartDescription.MapContentName, null);
            /*
            // set up the initial party
            ContentManager content = singleton.screenManager.Game.Content;
            singleton.party = new Party(gameStartDescription, content);

            // load the quest line
            singleton.questLine = content.Load<QuestLine>(
                Path.Combine(@"Quests\QuestLines",
                gameStartDescription.QuestLineContentName)).Clone() as QuestLine;
            */
        }
        /// <summary>
        /// Start a new session, using the data in the given save game.
        /// </summary>
        /// <param name="saveGameDescription">The description of the save game.</param>
        /// <param name="screenManager">The ScreenManager for the new session.</param>
        public static void LoadSession(SaveGameDescription saveGameDescription,
            ScreenManager screenManager, GameplayScreen gameplayScreen)
        {
            /*
            // check the parameters
            if (saveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }
            if (screenManager == null)
            {
                throw new ArgumentNullException("screenManager");
            }
            if (gameplayScreen == null)
            {
                throw new ArgumentNullException("gameplayScreen");
            }

            // end any existing session
            EndSession();

            // create the new session
            singleton = new Session(screenManager, gameplayScreen);

            // get the storage device and load the session
            GetStorageDevice(
                delegate (StorageDevice storageDevice)
                {
                    LoadSessionResult(storageDevice, saveGameDescription);
                });
            */
        }

        internal static void Draw(GameTime gameTime)
        {
            /*
            SpriteBatch spriteBatch = singleton.screenManager.SpriteBatch;

            if (CombatEngine.IsActive)
            {
                // draw the combat background
                if (TileEngine.Map.CombatTexture != null)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(TileEngine.Map.CombatTexture, Vector2.Zero,
                        Color.White);
                    spriteBatch.End();
                }

                // draw the combat itself
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
                CombatEngine.Draw(gameTime);
                spriteBatch.End();
            }
            else
            {
                singleton.DrawNonCombat(gameTime);
            }

            singleton.hud.Draw();
            */
        }


        /// <summary>
        /// Update the session for this frame.
        /// </summary>
        /// <remarks>This should only be called if there are no menus in use.</remarks>
        public static void Update(GameTime gameTime)
        {
            /*
            // check the singleton
            if (singleton == null)
            {
                return;
            }

            if (CombatEngine.IsActive*)
            {
                CombatEngine.Update(gameTime);
            }
            else
            {
                singleton.UpdateQuest();
                TileEngine.Update(gameTime);
            }
            */
        }


        /// <summary>
        /// Delete the save game specified by the description.
        /// </summary>
        /// <param name="saveGameDescription">The description of the save game.</param>
        public static void DeleteSaveGame(SaveGameDescription saveGameDescription)
        {
            /*
            // check the parameters
            if (saveGameDescription == null)
            {
                throw new ArgumentNullException("saveGameDescription");
            }

            // get the storage device and load the session
            GetStorageDevice(
                delegate (StorageDevice storageDevice)
                {
                    DeleteSaveGameResult(storageDevice, saveGameDescription);
                });
            */
        }
        /// <summary>
        /// Save the current state of the session.
        /// </summary>
        /// <param name="overwriteDescription">
        /// The description of the save game to over-write, if any.
        /// </param>
        public static void SaveSession(SaveGameDescription overwriteDescription)
        {
            // retrieve the storage device, asynchronously
            /*
            GetStorageDevice(delegate (StorageDevice storageDevice)
            {
                SaveSessionResult(storageDevice, overwriteDescription);
            });
            */
        }

        public static bool EncounterTile(Point mapPosition)
        {
            throw new NotImplementedException();
        }
    }
}
