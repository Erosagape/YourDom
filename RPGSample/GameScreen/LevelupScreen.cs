using System;
using System.Collections.Generic;

namespace RPGSample
{
    public class LevelUpScreen : GameScreen
    {
        private List<Player> leveledUpPlayers;

        public LevelUpScreen()
        {
        }

        public LevelUpScreen(List<Player> leveledUpPlayers)
        {
            this.leveledUpPlayers = leveledUpPlayers;
        }
    }
}
