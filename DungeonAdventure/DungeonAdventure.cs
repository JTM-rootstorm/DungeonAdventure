using System;
using System.Collections.Generic;
using System.Windows.Forms;

using Engine;

using Engine.Items;
using Engine.Items.Player;

using Engine.Creatures.Player;
using Engine.Creatures.Monsters;

using Engine.World;

namespace DungeonAdventure
{
    public partial class DungeonAdventure : Form
    {
        private Player _player;
        private Monster _currentMonster;

        public DungeonAdventure()
        {
            InitializeComponent();

            Location location = new Location(1, "Home", "This is your home");

            _player = new Player(10, 10, 20, 0, 1);
            MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
            _player.inventory.Add(new InventoryItem(World.ItemByID(World.ITEM_ID_RUSTY_SWORD), 1));
            
            lblHitPoints.Text = _player.currentHitPoints.ToString();
            lblGold.Text = _player.gold.ToString();
            lblExperience.Text = _player.experiencePoints.ToString();
            lblLevel.Text = _player.level.ToString();
        }

        private void btnNorth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToNorth);
        }

        private void btnSouth_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToSouth);
        }

        private void btnEast_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToEast);
        }

        private void btnWest_Click(object sender, EventArgs e)
        {
            MoveTo(_player.currentLocation.locationToWest);
        }

        private void MoveTo(Location newLocation)
        {
            if (newLocation.itemRequiredToEnter != null)
            {
                //Does the location have any required items
                if (!_player.HasRequiredItemToEnterThisLocation(newLocation))
                {
                    rtbMessages.Text += "You must have a " + newLocation.itemRequiredToEnter.name + " to enter this location." + Environment.NewLine;
                    return;
                }

                // Update the player's current location
                _player.currentLocation = newLocation;

                // Show/hide available movement buttons
                btnNorth.Visible = (newLocation.locationToNorth != null);
                btnEast.Visible = (newLocation.locationToEast != null);
                btnSouth.Visible = (newLocation.locationToSouth != null);
                btnWest.Visible = (newLocation.locationToWest != null);

                // Display current location name and description
                rtbLocation.Text = newLocation.name + Environment.NewLine;
                rtbLocation.Text += newLocation.description + Environment.NewLine;

                // Completely heal the player
                _player.currentHitPoints = _player.maximumHitPoints;

                // Update Hit Points in UI
                lblHitPoints.Text = _player.currentHitPoints.ToString();

                // Does the location have a quest?
                if (newLocation.questAvailableHere != null)
                {
                    // See if the player already has the quest, and if they've completed it
                    bool playerAlreadyHasQuest = _player.HasThisQuest(newLocation.questAvailableHere);
                    bool playerAlreadyCompletedQuest = _player.CompletedThisQuest(newLocation.questAvailableHere);

                    // See if the player already has the quest
                    if (playerAlreadyHasQuest)
                    {
                        // If the player has not completed the quest yet
                        if (!playerAlreadyCompletedQuest)
                        {
                            // See if the player has all the items needed to complete the quest
                            bool playerHasAllItemsToCompleteQuest = _player.HasAllQuestCompletionItems(newLocation.questAvailableHere);

                            // The player has all items required to complete the quest
                            if (playerHasAllItemsToCompleteQuest)
                            {
                                // Display message
                                rtbMessages.Text += Environment.NewLine;
                                rtbMessages.Text += "You complete the '" + newLocation.questAvailableHere.name + "' quest." + Environment.NewLine;

                                // Remove quest items from inventory
                                _player.RemoveQuestCompletionItems(newLocation.questAvailableHere);

                                // Give quest rewards
                                rtbMessages.Text += "You receive: " + Environment.NewLine;
                                rtbMessages.Text += newLocation.questAvailableHere.rewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;
                                rtbMessages.Text += newLocation.questAvailableHere.rewardGold.ToString() + " gold" + Environment.NewLine;
                                rtbMessages.Text += newLocation.questAvailableHere.rewardItem.name + Environment.NewLine;
                                rtbMessages.Text += Environment.NewLine;

                                _player.experiencePoints += newLocation.questAvailableHere.rewardExperiencePoints;
                                _player.gold += newLocation.questAvailableHere.rewardGold;

                                // Add the reward item to the player's inventory
                                _player.AddItemToInventory(newLocation.questAvailableHere.rewardItem);

                                // Mark the quest as completed
                                _player.MarkQuestCompleted(newLocation.questAvailableHere);
                            }
                        }
                    }
                    else
                    {
                        // The player does not already have the quest

                        // Display the messages
                        rtbMessages.Text += "You receive the " + newLocation.questAvailableHere.name + " quest." + Environment.NewLine;
                        rtbMessages.Text += newLocation.questAvailableHere.description + Environment.NewLine;
                        rtbMessages.Text += "To complete it, return with:" + Environment.NewLine;
                        foreach (QuestCompletionItem qci in newLocation.questAvailableHere.questCompletionItems)
                        {
                            if (qci.quantity == 1)
                            {
                                rtbMessages.Text += qci.quantity.ToString() + " " + qci.details.name + Environment.NewLine;
                            }
                            else
                            {
                                rtbMessages.Text += qci.quantity.ToString() + " " + qci.details.namePlural + Environment.NewLine;
                            }
                        }
                        rtbMessages.Text += Environment.NewLine;

                        // Add the quest to the player's quest list
                        _player.quests.Add(new PlayerQuest(newLocation.questAvailableHere));
                    }
                }

                // Does the location have a monster?
                if (newLocation.monsterLivingHere != null)
                {
                    rtbMessages.Text += "You see a " + newLocation.monsterLivingHere.name + Environment.NewLine;

                    // Make a new monster, using the values from the standard monster in the World.Monster list
                    Monster standardMonster = World.MonsterByID(newLocation.monsterLivingHere.ID);

                    _currentMonster = new Monster(standardMonster.ID, standardMonster.name, standardMonster.maximumDamage,
                        standardMonster.rewardExperiencePoints, standardMonster.rewardGold, standardMonster.currentHitPoints, standardMonster.maximumHitPoints);

                    foreach (LootItem lootItem in standardMonster.lootTable)
                    {
                        _currentMonster.lootTable.Add(lootItem);
                    }

                    cboWeapons.Visible = true;
                    cboPotions.Visible = true;
                    btnUseWeapon.Visible = true;
                    btnUsePotion.Visible = true;
                }
                else
                {
                    _currentMonster = null;

                    cboWeapons.Visible = false;
                    cboPotions.Visible = false;
                    btnUseWeapon.Visible = false;
                    btnUsePotion.Visible = false;
                }

                // Refresh player's inventory list
                UpdateInventoryListInUI();

                // Refresh player's quest list
                UpdateQuestListInUI();

                // Refresh player's weapons combobox
                UpdateWeaponListInUI();

                // Refresh player's potions combobox
                UpdatePotionListInUI();
            }
        }

        private void UpdateInventoryListInUI()
        {
            dgvInventory.RowHeadersVisible = false;

            dgvInventory.ColumnCount = 2;
            dgvInventory.Columns[0].Name = "Name";
            dgvInventory.Columns[0].Width = 197;
            dgvInventory.Columns[1].Name = "Quantity";

            dgvInventory.Rows.Clear();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.quantity > 0)
                {
                    dgvInventory.Rows.Add(new[] { inventoryItem.details.name, inventoryItem.quantity.ToString() });
                }
            }
        }

        private void UpdateQuestListInUI()
        {
            dgvQuests.RowHeadersVisible = false;

            dgvQuests.ColumnCount = 2;
            dgvQuests.Columns[0].Name = "Name";
            dgvQuests.Columns[0].Width = 197;
            dgvQuests.Columns[1].Name = "Done?";

            dgvQuests.Rows.Clear();

            foreach (PlayerQuest playerQuest in _player.quests)
            {
                dgvQuests.Rows.Add(new[] { playerQuest.details.name, playerQuest.isCompleted.ToString() });
            }
        }

        private void UpdateWeaponListInUI()
        {
            List<Weapon> weapons = new List<Weapon>();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.details is Weapon)
                {
                    if (inventoryItem.quantity > 0)
                    {
                        weapons.Add((Weapon)inventoryItem.details);
                    }
                }
            }

            if (weapons.Count == 0)
            {
                // The player doesn't have any weapons, so hide the weapon combobox and "Use" button
                cboWeapons.Visible = false;
                btnUseWeapon.Visible = false;
            }
            else
            {
                cboWeapons.DataSource = weapons;
                cboWeapons.DisplayMember = "Name";
                cboWeapons.ValueMember = "ID";

                cboWeapons.SelectedIndex = 0;
            }
        }

        private void UpdatePotionListInUI()
        {
            List<HealingPotion> healingPotions = new List<HealingPotion>();

            foreach (InventoryItem inventoryItem in _player.inventory)
            {
                if (inventoryItem.details is HealingPotion)
                {
                    if (inventoryItem.quantity > 0)
                    {
                        healingPotions.Add((HealingPotion)inventoryItem.details);
                    }
                }
            }

            if (healingPotions.Count == 0)
            {
                // The player doesn't have any potions, so hide the potion combobox and "Use" button
                cboPotions.Visible = false;
                btnUsePotion.Visible = false;
            }
            else
            {
                cboPotions.DataSource = healingPotions;
                cboPotions.DisplayMember = "Name";
                cboPotions.ValueMember = "ID";

                cboPotions.SelectedIndex = 0;
            }
        }

        private void btnUseWeapon_Click(object sender, EventArgs e)
        {

        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {

        }
    }
}
