using System;
using System.Windows.Forms;

using Engine;

namespace DungeonAdventure
{
    public partial class CharGeneration : Form
    {
        private int halfElfBonusAbs = 2, str, dex, con, wis, intel, cha;     

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

            if(selection == "Human")
            {
                lblAbsStrRmod.Text = "1";
                lblAbsDexRmod.Text = "1";
                lblAbsConRmod.Text = "1";
                lblAbsIntRmod.Text = "1";
                lblAbsWisRmod.Text = "1";
                lblAbsChaRmod.Text = "1";
            }

            UpdateStats();
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
            str = RandomNumberGenerator.NumberBetween(3, 18);
            dex = RandomNumberGenerator.NumberBetween(3, 18);
            con = RandomNumberGenerator.NumberBetween(3, 18);
            intel = RandomNumberGenerator.NumberBetween(3, 18);
            wis = RandomNumberGenerator.NumberBetween(3, 18);
            cha = RandomNumberGenerator.NumberBetween(3, 18);

            UpdateStats();
        }
    }
}
