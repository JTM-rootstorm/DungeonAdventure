using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Engine.Creatures.Player;

namespace DungeonAdventure
{
    public partial class DungeonAdventure : Form
    {
        private Player _player;

        public DungeonAdventure()
        {
            InitializeComponent();

            _player = new Player();

            _player.currentHitPoints = 10;
            _player.maximumHitPoints = 10;
            _player.gold = 20;
            _player.experiencePoints = 0;
            _player.level = 1;

            lblHitPoints.Text = _player.currentHitPoints.ToString();
            lblGold.Text = _player.gold.ToString();
            lblExperience.Text = _player.experiencePoints.ToString();
            lblLevel.Text = _player.level.ToString();
        }
    }
}
