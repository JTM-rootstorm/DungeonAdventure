using Engine.Items;
using Engine.Items.Player;

using Engine.Creatures;
using Engine.Creatures.Player;
using Engine.Creatures.Monsters;

using System;

namespace Engine.Systems
{
    public class Combat
    {
        private Player _player { get; set; }
        private Monster _monster { get; set; }

        public event EventHandler<MessageEventArgs> OnMessage;

        private int playerInit, monsterInit;

        public bool inCombat { get; set; }

        public Combat()
        {

        }

        public Combat(Player player, Monster monster)
        {
            _player = player;
            _monster = monster;

            playerInit = RollInitiative(_player);
            monsterInit = RollInitiative(_monster);
        }

        private int RollInitiative(Creature creature)
        {
            return RandomNumberGenerator.NumberBetween(1, 20) + creature.FindAttModifier(creature.dexterity);
        }

        private void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            if (OnMessage != null)
            {
                OnMessage(this, new MessageEventArgs(message, addExtraNewLine));
            }
        }

        public void PlayerUsePotion(HealingPotion potion)
        {
            if(_player.currentHitPoints < _player.maximumHitPoints)
            {
                // Add healing amount to the player's current hit points
                _player.currentHitPoints = (_player.currentHitPoints + potion.amountToHeal);

                // CurrentHitPoints cannot exceed player's MaximumHitPoints
                if (_player.currentHitPoints > _player.maximumHitPoints)
                {
                    _player.currentHitPoints = _player.maximumHitPoints;
                }

                // Remove the potion from the player's inventory
                _player.RemoveItemFromInventory(potion, 1);

                // Display message
                RaiseMessage("You drink a " + potion.name);
            }
            else
            {
                RaiseMessage("You don't need to drink one of those right now!");
            }
        }

        public void PlayerUseWeapon(Weapon weapon)
        {
            int attackRoll = RandomNumberGenerator.NumberBetween(1, 20) + _player.attackMod;

            if (attackRoll >= _monster.AC)
            {
                // Determine the amount of damage to do to the monster
                int damageToMonster = RandomNumberGenerator.NumberBetween(weapon.minimumDamage, weapon.maximumDamage) + _player.damageMod;

                // Apply the damage to the monster's CurrentHitPoints
                _monster.currentHitPoints -= damageToMonster;

                // Display message
                RaiseMessage("You hit the " + _monster.name + " for " + damageToMonster + " points.");
            }
            else
            {
                RaiseMessage("You missed the " + _monster.name + "!");
            }
        }

        public void MonsterAttack()
        {
            int attackRoll = RandomNumberGenerator.NumberBetween(1, 20) + _monster.attackMod;

            if (attackRoll >= _player.AC)
            {
                // Determine the amount of damage the monster does to the player
                int damageToPlayer = RandomNumberGenerator.NumberBetween(_monster.minimumDamage, _monster.maximumDamage);

                // Display message
                RaiseMessage("The " + _monster.name + " did " + damageToPlayer + " points of damage.");

                // Subtract damage from player
                _player.currentHitPoints -= damageToPlayer;
            }
        }
    }
}
