﻿using System;
using System.IO;
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

            MoveTo(_player.currentLocation);
            RefreshUI(true);
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

        private void rtbLocation_TextChanged(object sender, EventArgs e)
        {
            rtbLocation.SelectionStart = rtbMessages.Text.Length;
            rtbLocation.ScrollToCaret();
        }

        private void rtbMessages_TextChanged(object sender, EventArgs e)
        {
            rtbMessages.SelectionStart = rtbMessages.Text.Length;
            rtbMessages.ScrollToCaret();
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
                //_player.currentHitPoints = _player.maximumHitPoints;

                /* Not Healing the player unless they either rest or use a healing potion */

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

                    _currentMonster = new Monster(standardMonster.ID, standardMonster.name, standardMonster.AC, standardMonster.attackMod, 
                        standardMonster.minimumDamage, standardMonster.maximumDamage, standardMonster.rewardExperiencePoints, standardMonster.rewardGold, 
                        standardMonster.maximumHitPoints);

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
            int monsterInit = RandomNumberGenerator.NumberBetween(1, 20) + _currentMonster.dexterity;
            int playerInit = RandomNumberGenerator.NumberBetween(1, 20) + _player.dexterity;

            // Debug stuff
            rtbMessages.Text += Environment.NewLine;
            rtbMessages.Text += "Player rolled a initiative of " + playerInit.ToString() + Environment.NewLine;
            rtbMessages.Text += "Monster rolled a initiative of " + monsterInit.ToString() + Environment.NewLine;

            if (playerInit > monsterInit)
            {
                TakeTurn(true);
            }
            else if (playerInit < monsterInit)
            {
                TakeTurn(false);
            }

            // Check if the monster is dead
            if (_currentMonster.currentHitPoints <= 0)
            {
                // Monster is dead
                rtbMessages.Text += Environment.NewLine;
                rtbMessages.Text += "You defeated the " + _currentMonster.name + Environment.NewLine;

                // Give player experience points for killing the monster
                _player.experiencePoints += _currentMonster.rewardExperiencePoints;
                rtbMessages.Text += "You receive " + _currentMonster.rewardExperiencePoints.ToString() + " experience points" + Environment.NewLine;

                // Give player gold for killing the monster 
                _player.gold += _currentMonster.rewardGold;
                rtbMessages.Text += "You receive " + _currentMonster.rewardGold.ToString() + " gold" + Environment.NewLine;

                // Get random loot items from the monster
                List<InventoryItem> lootedItems = new List<InventoryItem>();

                // Add items to the lootedItems list, comparing a random number to the drop percentage
                foreach (LootItem lootItem in _currentMonster.lootTable)
                {
                    if (RandomNumberGenerator.NumberBetween(1, 100) <= lootItem.dropPercentage)
                    {
                        lootedItems.Add(new InventoryItem(lootItem.details, 1));
                    }
                }

                // If no items were randomly selected, then add the default loot item(s).
                if (lootedItems.Count == 0)
                {
                    foreach (LootItem lootItem in _currentMonster.lootTable)
                    {
                        if (lootItem.isDefaultItem)
                        {
                            lootedItems.Add(new InventoryItem(lootItem.details, 1));
                        }
                    }
                }

                // Add the looted items to the player's inventory
                foreach (InventoryItem inventoryItem in lootedItems)
                {
                    _player.AddItemToInventory(inventoryItem.details);

                    if (inventoryItem.quantity == 1)
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.quantity.ToString() + " " + inventoryItem.details.name + Environment.NewLine;
                    }
                    else
                    {
                        rtbMessages.Text += "You loot " + inventoryItem.quantity.ToString() + " " + inventoryItem.details.namePlural + Environment.NewLine;
                    }
                }

                // Refresh player information and inventory controls
                RefreshUI(true);

                // Add a blank line to the messages box, just for appearance.
                rtbMessages.Text += Environment.NewLine;

                // Move player to current location (to heal player and create a new monster to fight)
                //MoveTo(_player.currentLocation);
                
            /* Keeping the above commented so it 
                   1. refreshes when the player leaves and then returns
                   2. doesn't heal the player */
            }
        }

        private void btnUsePotion_Click(object sender, EventArgs e)
        {
            // Get the currently selected potion from the combobox
            HealingPotion potion = (HealingPotion)cboPotions.SelectedItem;

            // Add healing amount to the player's current hit points
            _player.currentHitPoints = (_player.currentHitPoints + potion.amountToHeal);

            // CurrentHitPoints cannot exceed player's MaximumHitPoints
            if (_player.currentHitPoints > _player.maximumHitPoints)
            {
                _player.currentHitPoints = _player.maximumHitPoints;
            }

            // Remove the potion from the player's inventory
            foreach (InventoryItem ii in _player.inventory)
            {
                if (ii.details.ID == potion.ID)
                {
                    ii.quantity--;
                    break;
                }
            }

            // Display message
            rtbMessages.Text += "You drink a " + potion.name + Environment.NewLine;

            TakeTurn(false);

            // Refresh player data in UI
            RefreshUI();
        }

        public void TakeTurn(bool playerTurn)
        {
            int attackRoll = 0, damageDelt = 0;

            if (playerTurn)
            {
                // Get the currently selected weapon from the cboWeapons ComboBox
                Weapon currentWeapon = (Weapon)cboWeapons.SelectedItem;

                if (currentWeapon.finesse)
                {
                    if(_player.FindAttModifier(_player.strength) >= _player.FindAttModifier(_player.dexterity))
                    {
                        _player.attackMod = _player.profBonus + _player.FindAttModifier(_player.strength);
                        _player.damageMod = _player.FindAttModifier(_player.strength);
                    }
                    else
                    {
                        _player.attackMod = _player.profBonus + _player.FindAttModifier(_player.dexterity);
                        _player.damageMod = _player.FindAttModifier(_player.dexterity);
                    }
                }

                attackRoll = RandomNumberGenerator.NumberBetween(0, 20) + _player.attackMod;

                if (attackRoll >= _currentMonster.AC)
                {
                    // Determine the amount of damage to do to the monster
                    damageDelt = RandomNumberGenerator.NumberBetween(currentWeapon.minimumDamage, currentWeapon.maximumDamage) + _player.damageMod;

                    // Apply the damage to the monster's CurrentHitPoints
                    _currentMonster.currentHitPoints -= damageDelt;

                    // Display message
                    rtbMessages.Text += "You hit the " + _currentMonster.name + " for " + damageDelt.ToString() + " points." + Environment.NewLine;
                }
            }
            else if (!playerTurn)
            {
                // Monster gets their turn to attack

                // Determine if the monster hits
                attackRoll = RandomNumberGenerator.NumberBetween(1, 20) + _currentMonster.attackMod;

                if (attackRoll >= _player.AC)
                {
                    // Determine the amount of damage the monster does to the player
                    damageDelt = RandomNumberGenerator.NumberBetween(_currentMonster.minimumDamage, _currentMonster.maximumDamage);

                    // Display message
                    rtbMessages.Text += "The " + _currentMonster.name + " did " + damageDelt.ToString() + " points of damage." + Environment.NewLine;

                    // Subtract damage from player
                    _player.currentHitPoints -= damageDelt;

                    if (_player.currentHitPoints <= 0)
                    {
                        // Display message
                        rtbMessages.Text += "The " + _currentMonster.name + " killed you." + Environment.NewLine;

                        // Move player to "Home"
                        MoveTo(World.LocationByID(World.LOCATION_ID_HOME));
                    }
                }
                else
                {
                    rtbMessages.Text += "The " + _currentMonster.name + " missed!" + Environment.NewLine;
                }
            }      
        }

        public void RefreshUI(bool fullRef = false)
        {
            lblHitPoints.Text = _player.currentHitPoints.ToString();
            UpdateInventoryListInUI();
            UpdatePotionListInUI();

            if (fullRef)
            {
                lblGold.Text = _player.gold.ToString();
                lblExperience.Text = _player.experiencePoints.ToString();
                lblLevel.Text = _player.level.ToString();

                UpdateWeaponListInUI();
            }
        }

        private void DungeonAdventure_FormClosing(object sender, FormClosingEventArgs e)
        {
            File.WriteAllText(PLAYER_DATA_FILE_NAME, _player.ToXMLString());
        }
    }
}
