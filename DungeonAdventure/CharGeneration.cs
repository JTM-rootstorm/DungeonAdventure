using System;
using System.Windows.Forms;

using Engine;
using Engine.Creatures.Player;

namespace DungeonAdventure
{
    public partial class CharGeneration : Form
    {
        private int halfElfBonusAbs = 2, str = 0, dex = 0, con = 0, wis = 0, intel = 0, cha = 0;
        private int[,] rollArray;

        public CharGeneration()
        {
            InitializeComponent();

            lblAbsStrRmod.Text = "0";
            lblAbsDexRmod.Text = "0";
            lblAbsConRmod.Text = "0";
            lblAbsIntRmod.Text = "0";
            lblAbsWisRmod.Text = "0";
            lblAbsChaRmod.Text = "0";

            lblAbsStr.Text = "0";
            lblAbsDex.Text = "0";
            lblAbsCon.Text = "0";
            lblAbsInt.Text = "0";
            lblAbsWis.Text = "0";
            lblAbsCha.Text = "0";

            UpdateStats();
        }

        private void OnCheckedChange(object sender, EventArgs e)
        {
            CheckBox box = (CheckBox)sender;         

            if (box.Checked)
            {
                halfElfBonusAbs -= 1;
                if(box == chkStrRmod)
                {
                    lblAbsStrRmod.Text = "1";
                }
                else if(box == chkDexRmod)
                {
                    lblAbsDexRmod.Text = "1";
                }
                else if (box == chkConRmod)
                {
                    lblAbsConRmod.Text = "1";
                }
                else if (box == chkIntRmod)
                {
                    lblAbsIntRmod.Text = "1";
                }
                else if (box == chkWisRmod)
                {
                    lblAbsWisRmod.Text = "1";
                }
            }
            else 
            {
                halfElfBonusAbs += 1;
                if (box == chkStrRmod)
                {
                    lblAbsStrRmod.Text = "0";
                }
                else if (box == chkDexRmod)
                {
                    lblAbsDexRmod.Text = "0";
                }
                else if (box == chkConRmod)
                { 
                    lblAbsConRmod.Text = "0";
                }
                else if (box == chkIntRmod)
                {
                    lblAbsIntRmod.Text = "0";
                }
                else if (box == chkWisRmod)
                {
                    lblAbsWisRmod.Text = "0";
                }
            }

            if (halfElfBonusAbs < 0)
            {
                box.Checked = false;
                halfElfBonusAbs = 0;
            }

            UpdateStats();
        }

        private void UpdateStats()
        {
            lblAbsStr.Text = "" + (str + Convert.ToInt32(lblAbsStrRmod.Text));
            lblAbsDex.Text = "" + (dex + Convert.ToInt32(lblAbsDexRmod.Text));
            lblAbsCon.Text = "" + (con + Convert.ToInt32(lblAbsConRmod.Text));
            lblAbsInt.Text = "" + (intel + Convert.ToInt32(lblAbsIntRmod.Text));
            lblAbsWis.Text = "" + (wis + Convert.ToInt32(lblAbsWisRmod.Text));
            lblAbsCha.Text = "" + (cha + Convert.ToInt32(lblAbsChaRmod.Text));

            lblAbsStrMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsStr.Text) - 10.0d) / 2.0d));
            lblAbsDexMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsDex.Text) - 10.0d) / 2.0d));
            lblAbsConMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsCon.Text) - 10.0d) / 2.0d));
            lblAbsIntMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsInt.Text) - 10.0d) / 2.0d));
            lblAbsWisMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsWis.Text) - 10.0d) / 2.0d));
            lblAbsChaMod.Text = "" + ((int)Math.Floor((Convert.ToInt32(lblAbsCha.Text) - 10.0d) / 2.0d));
        }

        private void raceSet_Click(object sender, EventArgs e)
        {
            string selection = cboRace.Items[cboRace.SelectedIndex].ToString();

            if (selection == "Half-Elf")
            {
                chkStrRmod.Enabled = true;
                chkDexRmod.Enabled = true;
                chkConRmod.Enabled = true;
                chkIntRmod.Enabled = true;
                chkWisRmod.Enabled = true;

                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "2";
            }
            else
            {
                chkStrRmod.Enabled = false;
                chkDexRmod.Enabled = false;
                chkConRmod.Enabled = false;
                chkIntRmod.Enabled = false;
                chkWisRmod.Enabled = false;

                chkStrRmod.Checked = false;
                chkDexRmod.Checked = false;
                chkConRmod.Checked = false;
                chkIntRmod.Checked = false;
                chkWisRmod.Checked = false;

                halfElfBonusAbs = 2;
            }

            if(selection == "Dwarf - Hill")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "2";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "1";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Dwarf - Mountain")
            {
                lblAbsStrRmod.Text = "2";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "2";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Elf - High")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "2";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "1";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Elf - Wood")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "2";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "1";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Halfling - Lightfoot")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "2";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "1";
            }
            else if(selection == "Halfling - Stout")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "2";
                lblAbsConRmod.Text = "1";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Human")
            {
                lblAbsStrRmod.Text = "1";
                lblAbsDexRmod.Text = "1";
                lblAbsConRmod.Text = "1";
                lblAbsIntRmod.Text = "1";
                lblAbsWisRmod.Text = "1";
                lblAbsChaRmod.Text = "1";
            }
            else if(selection == "Dragonborn")
            {
                lblAbsStrRmod.Text = "2";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "1";
            }
            else if(selection == "Gnome - Forest")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "1";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "2";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Gnome - Rock")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "1";
                lblAbsIntRmod.Text = "2";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Half-Orc")
            {
                lblAbsStrRmod.Text = "2";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "1";
                lblAbsIntRmod.Text = "0";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "0";
            }
            else if(selection == "Tiefling")
            {
                lblAbsStrRmod.Text = "0";
                lblAbsDexRmod.Text = "0";
                lblAbsConRmod.Text = "0";
                lblAbsIntRmod.Text = "1";
                lblAbsWisRmod.Text = "0";
                lblAbsChaRmod.Text = "2";
            }

            UpdateStats();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if(cboRace.Text != "" && str != 0 && dex != 0 && con != 0 && intel != 0 && wis != 0 && cha != 0)
            {
                Player _player = new Player(20, 20, str, dex, con, intel, wis, cha, "");
                new DungeonAdventure(_player).Show();
                Hide();
                Enabled = false;
            }
        }

        private void CharGeneration_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void chkStrRmod_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChange(sender, e);
        }

        private void chkDexRmod_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChange(sender, e);
        }

        private void chkConRmod_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChange(sender, e);
        }

        private void chkIntRmod_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChange(sender, e);
        }

        private void chkWisRmod_CheckedChanged(object sender, EventArgs e)
        {
            OnCheckedChange(sender, e);
        }

        private void btnRollAbs_Click(object sender, EventArgs e)
        {
            if(cboRollType.Text == "3d6")
            {
                str = RandomNumberGenerator.NumberBetween(3, 18);
                dex = RandomNumberGenerator.NumberBetween(3, 18);
                con = RandomNumberGenerator.NumberBetween(3, 18);
                intel = RandomNumberGenerator.NumberBetween(3, 18);
                wis = RandomNumberGenerator.NumberBetween(3, 18);
                cha = RandomNumberGenerator.NumberBetween(3, 18);
            }
            else if(cboRollType.Text == "2d6+6")
            {
                str = RandomNumberGenerator.NumberBetween(2, 12) + 6;
                dex = RandomNumberGenerator.NumberBetween(2, 12) + 6;
                con = RandomNumberGenerator.NumberBetween(2, 12) + 6;
                intel = RandomNumberGenerator.NumberBetween(2, 12) + 6;
                wis = RandomNumberGenerator.NumberBetween(2, 12) + 6;
                cha = RandomNumberGenerator.NumberBetween(2, 12) + 6;
            }
            else if(cboRollType.Text == "4d6 drop lowest 1")
            {
                rollArray = new int[6, 5];
                int lowRoll;

                for(int i = 0; i < 6; i++)
                {
                    lowRoll = 6;
                    for(int j = 0; j < 4; j++)
                    {
                        rollArray[i, j] = RandomNumberGenerator.NumberBetween(1, 6);

                        if(rollArray[i,j] < lowRoll)
                        {
                            lowRoll = rollArray[i, j];
                        }
                    }
                    rollArray[i, 4] = lowRoll;
                }

                str = rollArray[0, 0] + rollArray[0, 1] + rollArray[0, 2] + rollArray[0, 3] - rollArray[0, 4];
                dex = rollArray[1, 0] + rollArray[1, 1] + rollArray[1, 2] + rollArray[1, 3] - rollArray[1, 4];
                con = rollArray[2, 0] + rollArray[2, 1] + rollArray[2, 2] + rollArray[2, 3] - rollArray[2, 4];
                intel = rollArray[3, 0] + rollArray[3, 1] + rollArray[3, 2] + rollArray[3, 3] - rollArray[3, 4];
                wis = rollArray[4, 0] + rollArray[4, 1] + rollArray[4, 2] + rollArray[4, 3] - rollArray[4, 4];
                cha = rollArray[5, 0] + rollArray[5, 1] + rollArray[5, 2] + rollArray[5, 3] - rollArray[5, 4];
            }
            else if(cboRollType.Text == "5d6 drop lowest 2")
            {
                rollArray = new int[6, 7];
                int lowRoll, lowRollSecond;

                for (int i = 0; i < 6; i++)
                {
                    lowRoll = 6;
                    lowRollSecond = 6;
                    for (int j = 0; j < 5; j++)
                    {
                        rollArray[i, j] = RandomNumberGenerator.NumberBetween(1, 6);

                        if (rollArray[i, j] < lowRoll && rollArray[i,j] > lowRollSecond)
                        {
                            lowRoll = rollArray[i, j];
                        }
                        else if(rollArray[i, j] < lowRoll && rollArray[i, j] < lowRollSecond)
                        {
                            lowRollSecond = rollArray[i, j];
                        }
                    }
                    rollArray[i, 5] = lowRoll;
                    rollArray[i, 6] = lowRollSecond;
                }

                str = rollArray[0, 0] + rollArray[0, 1] + rollArray[0, 2] + rollArray[0, 3] + rollArray[0, 4] - rollArray[0, 5] - rollArray[0, 6];
                dex = rollArray[1, 0] + rollArray[1, 1] + rollArray[1, 2] + rollArray[1, 3] + rollArray[1, 4] - rollArray[1, 5] - rollArray[1, 6];
                con = rollArray[2, 0] + rollArray[2, 1] + rollArray[2, 2] + rollArray[2, 3] + rollArray[2, 4] - rollArray[2, 5] - rollArray[2, 6];
                intel = rollArray[3, 0] + rollArray[3, 1] + rollArray[3, 2] + rollArray[3, 3] + rollArray[3, 4] - rollArray[3, 5] - rollArray[3, 6];
                wis = rollArray[4, 0] + rollArray[4, 1] + rollArray[4, 2] + rollArray[4, 3] + rollArray[4, 4] - rollArray[4, 5] - rollArray[4, 6];
                cha = rollArray[5, 0] + rollArray[5, 1] + rollArray[5, 2] + rollArray[5, 3] + rollArray[5, 4] - rollArray[5, 5] - rollArray[5, 6];
            }

            UpdateStats();
        }
    }
}
