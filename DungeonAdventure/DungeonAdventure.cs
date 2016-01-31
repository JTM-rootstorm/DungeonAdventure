using System;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Linq;

using Engine;

using Engine.Systems;

using Engine.Items;
using Engine.Items.Player;

using Engine.Creatures.Player;
using Engine.Creatures.Monsters;

namespace DungeonAdventure
{
    public partial class DungeonAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;
        private Messenger messenger;
        private Combat combat;
        private const string PLAYER_DATA_FILE_NAME = "PlayerData.xml";

        public DungeonAdventure()
        {
            InitializeComponent();

            if (File.Exists(PLAYER_DATA_FILE_NAME))
            {
                _player = Player.CreatePlayerFromXmlString(File.ReadAllText(PLAYER_DATA_FILE_NAME));
            }
            else
            {
                _player = Player.CreateDefaultPlayer();
            }

            lblHitPoints.DataBindings.Add("Text", _player, "currentHitPoints");
            lblGold.DataBindings.Add("Text", _player, "gold");
            lblExperience.DataBindings.Add("Text", _player, "experiencePoints");
            lblLevel.DataBindings.Add("Text", _player, "level");

            dgvInventory.RowHeadersVisible = false;
            dgvInventory.AutoGenerateColumns = false;

            dgvInventory.DataSource = _player.inventory;

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "description"
            });

            dgvInventory.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Quantity",
                DataPropertyName = "quantity"
            });

            dgvQuests.RowHeadersVisible = false;
            dgvQuests.AutoGenerateColumns = false;

            dgvQuests.DataSource = _player.quests;

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Name",
                Width = 197,
                DataPropertyName = "name"
            });

            dgvQuests.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Done?",
                DataPropertyName = "isCompleted"
            });

            cboWeapons.DataSource = _player.weapons;
            cboWeapons.DisplayMember = "Name";
            cboWeapons.ValueMember = "Id";

            if(_player.currentWeapon != null)
            {
                cboWeapons.SelectedItem = _player.currentWeapon;
            }

            cboWeapons.SelectedIndexChanged += cboWeapons_SelectedIndexChanged;

            cboPotions.DataSource = _player.potions;
            cboPotions.DisplayMember = "Name";
            cboPotions.ValueMember = "Id";

            _player.PropertyChanged += PlayerOnPropertyChanged;
            messenger.OnMessage += DisplayMessage;

            _player.MoveTo(_player.currentLocation);
        }

        private void DisplayMessage(object sender, MessageEventArgs messageEventArgs)
        {
            rtbMessages.Text += messageEventArgs.Message + Environment.NewLine;

            if (messageEventArgs.AddExtraNewLine)
            {
                rtbMessages.Text += Environment.NewLine;
            }

            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
        }

        private void PlayerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if(propertyChangedEventArgs.PropertyName == "weapons")
            {
                cboWeapons.DataSource = _player.weapons;

                if (!_player.weapons.Any())
                {
                    cboWeapons.Visible = false;
                    btnUseWeapon.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "potions")
            {
                cboPotions.DataSource = _player.potions;

                if (!_player.potions.Any())
                {
                    cboPotions.Visible = false;
                    btnUsePotion.Visible = false;
                }
            }

            if (propertyChangedEventArgs.PropertyName == "currentLocation")
            {
                // Show/hide available movement buttons
                btnNorth.Visible = (_player.currentLocation.locationToNorth != null);
                btnEast.Visible = (_player.currentLocation.locationToEast != null);
                btnSouth.Visible = (_player.currentLocation.locationToSouth != null);
                btnWest.Visible = (_player.currentLocation.locationToWest != null);

                // Display current location name and description
                rtbLocation.Text = _player.currentLocation.name + Environment.NewLine;
                rtbLocation.Text += _player.currentLocation.description + Environment.NewLine;

                if (_player.currentLocation.monsterLivingHere == null)
                {
                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                }
                else
                {
                    cboWeapons.Visible = _player.weapons.Any();
                    cboPotions.Visible = _player.potions.Any();
                    btnUseWeapon.Visible = _player.weapons.Any();
                    btnUsePotion.Visible = _player.potions.Any();
                }
            }
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            _player.MoveNorth();
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            _player.MoveSouth();
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            _player.MoveEast();
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            _player.MoveWest();
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {
            Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

            if (combat.inCombat)
            {
                _player.UseWeapon(currentWeapon);
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            combat.PlayerUsePotion(potion);
        }

        private void DungeonAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXMLString());
        }

        private void cboWeapons_SelectedIndexChanged(object sender, EventArgs e)
        {
            _player.currentWeapon = (Weapon)cboWeapons.SelectedItem;
        }
    }
}
