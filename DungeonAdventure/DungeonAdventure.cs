using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine;
using Engine.Creatures.Player;

namespace DungeonAdventure
{
    public partial class DungeonAdventure : Form
    {
        private Player _player;

        public DungeonAdventure()
        {
            InitializeComponent();

            Location location = new Location(1, "Home", "This is your home");

            _player = new Player(10, 10, 20, 0, 1);

            lblHitPoints.Text = _player.currentHitPoints.ToString();
            lblGold.Text = _player.gold.ToString();
            lblExperience.Text = _player.experiencePoints.ToString();
            lblLevel.Text = _player.level.ToString();
        }
    }
}
