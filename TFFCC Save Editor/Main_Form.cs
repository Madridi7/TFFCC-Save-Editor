﻿using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TFFCC_Save_Editor
{
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }

        OpenFileDialog open = new OpenFileDialog();
        OpenFileDialog open_main = new OpenFileDialog();

        private void Open_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                open.Filter = " extsavedata.bk Files |extsavedata.bk|All Files (*.*)|*.*";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(open.FileName));

                    //Main songs
                    int Main_count = 0;
                    for (int i = 0x5FD54; i < 0x66F48; i += 0x2C)
                    {
                        var index = Songs_dataGridView.Rows.Add();
                        Songs_dataGridView.Rows[index].Cells["Level_name"].Value = "Unknown";

                        //Read Score value for song
                        br.BaseStream.Position = i;
                        Songs_dataGridView.Rows[index].Cells["Score"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read chain value for song
                        br.BaseStream.Position = i + 0x04;
                        Songs_dataGridView.Rows[index].Cells["Chain"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read rank value for song
                        br.BaseStream.Position = i + 0x08;
                        var rank = br.ReadBytes(0x05);
                        if (rank[0] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "SSS";
                        }
                        else if (rank[0] == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "SS";
                        }
                        else if (rank[0] == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "S";
                        }
                        else if (rank[0] == 0x03)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "A";
                        }
                        else if (rank[0] == 0x04)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "B";
                        }
                        else if (rank[0] == 0x05)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "C";
                        }
                        else if (rank[0] == 0x06)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "D";
                        }
                        else if (rank[0] == 0x07)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "E";
                        }
                        else if (rank[0] == 0x08 && rank[4] != 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "F";
                        }
                        else if (rank[0] == 0x08 && rank[4] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "Unplayed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "Unknown";
                        }

                        //Read status value for song
                        br.BaseStream.Position = i + 0x09;
                        var status = br.ReadBytes(0x04);
                        if (status[0] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "All-Critical";
                        }
                        else if (status[0] == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Perfect Chain";
                        }
                        else if (status[0] == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Clear";
                        }
                        else if (status[0] == 0x03 && status[3] != 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Failed";
                        }
                        else if (status[0] == 0x03 && status[3] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Unplayed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Unknown";
                        }

                        //Read playstyle value for song
                        br.BaseStream.Position = i + 0x0A;
                        var playstyle = br.ReadByte();
                        if (playstyle == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Stylus";
                        }
                        else if (playstyle == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Button";
                        }
                        else if (playstyle == 0x03)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Hybrid";
                        }
                        else if (playstyle == 0x04)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "One-Handed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Unplayed";
                        }

                        //Read times played value for song
                        br.BaseStream.Position = i + 0x0C;
                        Songs_dataGridView.Rows[index].Cells["Times_played"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read times cleared value for song
                        br.BaseStream.Position = i + 0x10;
                        Songs_dataGridView.Rows[index].Cells["Times_cleared"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read date value for song
                        br.BaseStream.Position = i + 0x26;
                        var year = BitConverter.ToInt16(br.ReadBytes(0x02), 0);
                        br.BaseStream.Position = i + 0x28;
                        var month = br.ReadByte();
                        br.BaseStream.Position = i + 0x29;
                        var day = br.ReadByte();
                        Songs_dataGridView.Rows[index].Cells["Date"].Value = $"{day}.{month}.{year}";

                        ++Main_count;
                    }
                    label1.Text = $"Songs Found: {Main_count}";

                    //DLC songs
                    int DLC_count = 0;
                    for (int i = 0x670D4; i < 0x6A464; i += 0x2C)
                    {
                        var index = Songs_dataGridView.Rows.Add();
                        Songs_dataGridView.Rows[index].Cells["Level_name"].Value = "Unknown";

                        //Read Score value for song
                        br.BaseStream.Position = i;
                        Songs_dataGridView.Rows[index].Cells["Score"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read chain value for song
                        br.BaseStream.Position = i + 0x04;
                        Songs_dataGridView.Rows[index].Cells["Chain"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read rank value for song
                        br.BaseStream.Position = i + 0x08;
                        var rank = br.ReadBytes(0x05);
                        if (rank[0] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "SSS";
                        }
                        else if (rank[0] == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "SS";
                        }
                        else if (rank[0] == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "S";
                        }
                        else if (rank[0] == 0x03)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "A";
                        }
                        else if (rank[0] == 0x04)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "B";
                        }
                        else if (rank[0] == 0x05)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "C";
                        }
                        else if (rank[0] == 0x06)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "D";
                        }
                        else if (rank[0] == 0x07)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "E";
                        }
                        else if (rank[0] == 0x08 && rank[4] != 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "F";
                        }
                        else if (rank[0] == 0x08 && rank[4] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "Unplayed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Rank"].Value = "Unknown";
                        }

                        //Read status value for song
                        br.BaseStream.Position = i + 0x09;
                        var status = br.ReadBytes(0x04);
                        if (status[0] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "All-Critical";
                        }
                        else if (status[0] == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Perfect Chain";
                        }
                        else if (status[0] == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Clear";
                        }
                        else if (status[0] == 0x03 && status[3] != 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Failed";
                        }
                        else if (status[0] == 0x03 && status[3] == 0x00)
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Unplayed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Status"].Value = "Unknown";
                        }

                        //Read playstyle value for song
                        br.BaseStream.Position = i + 0x0A;
                        var playstyle = br.ReadByte();
                        if (playstyle == 0x01)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Stylus";
                        }
                        else if (playstyle == 0x02)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Button";
                        }
                        else if (playstyle == 0x03)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Hybrid";
                        }
                        else if (playstyle == 0x04)
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "One-Handed";
                        }
                        else
                        {
                            Songs_dataGridView.Rows[index].Cells["Play_style"].Value = "Unplayed";
                        }

                        //Read times played value for song
                        br.BaseStream.Position = i + 0x0C;
                        Songs_dataGridView.Rows[index].Cells["Times_played"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read times cleared value for song
                        br.BaseStream.Position = i + 0x10;
                        Songs_dataGridView.Rows[index].Cells["Times_cleared"].Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                        //Read date value for song
                        br.BaseStream.Position = i + 0x26;
                        var year = BitConverter.ToInt16(br.ReadBytes(0x02), 0);
                        br.BaseStream.Position = i + 0x28;
                        var month = br.ReadByte();
                        br.BaseStream.Position = i + 0x29;
                        var day = br.ReadByte();
                        Songs_dataGridView.Rows[index].Cells["Date"].Value = $"{day}.{month}.{year}";

                        ++DLC_count;
                    }
                    label2.Text = $"DLC Songs Found: {DLC_count}";
                    label3.Text = $"Total Songs Found: {Main_count + DLC_count}";
                    br.Close();
                }
            }
            catch
            {
                MessageBox.Show("Invalid extsavedata.bk", "Failed to open the file");
            }
        }

        private void Open_main_ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the combobox items from the monsters database
            comboBox1.DataSource = Databases.monsters;

            //Set the combobox items from the characters database
            comboBox4.DataSource = Databases.characters;

            //create list from items remove cards.
            List<string> ItemsCardsRemoved = new List<string>(Databases.items);

            //Find N and R cards from the items database
            var itemN = ItemsCardsRemoved.FindAll(i => i.Substring(4).StartsWith(" [N]"));
            var itemR = ItemsCardsRemoved.FindAll(i => i.Substring(4).StartsWith(" [R]"));

            //Remove N and R cards from the items database
            foreach (string i in itemN)
            {
                ItemsCardsRemoved.Remove(i);
            }
            foreach (string i in itemR)
            {
                ItemsCardsRemoved.Remove(i);
            }

            //Set the combobox items from the items database with the N and R cards removed
            comboBox2.DataSource = ItemsCardsRemoved;

            //Set the combobox items from the items database without anything removed
            comboBox3.DataSource = Databases.items;

            //This would be to check what is the current item selected on the items combobox and then get the index for its position on the original database
            Console.WriteLine(Databases.items.FindIndex(i => i.Equals("#001 [N] CollectaCard")));

            try
            {
                open_main.Filter = " savedata.bk Files |savedata.bk|All Files (*.*)|*.*";
                if (open_main.ShowDialog() == DialogResult.OK)
                {
                    BinaryReader br = new BinaryReader(File.OpenRead(open_main.FileName));

                    //Items
                    int Item_count = 0;
                    for (int i = 0xCA0; i < 0xCFC; i++)
                    {
                        var index = Items_dataGridView.Rows.Add();
                        Items_dataGridView.Rows[index].Cells["Item"].Value = Databases.items[index];

                        //Read item quantity
                        br.BaseStream.Position = i;
                        Items_dataGridView.Rows[index].Cells["Quantity"].Value = br.ReadByte() - 0x80;

                        ++Item_count;
                    }
                    label8.Text = $"Items Found: {Item_count}";

                    //CollectaCards
                    int Card_count = 0;
                    for (int i = 0x3497; i < 0x367D; i++)
                    {
                        var index = Cards_dataGridView.Rows.Add();
                        Cards_dataGridView.Rows[index].Cells["Card_name"].Value = Databases.collectacards[index];

                        if (Cards_dataGridView.Rows[index].Cells["Card_name"].Value.ToString().Contains("[N]"))
                        {
                            Cards_dataGridView.Rows[index].Cells["Rarity"].Value = "Normal";
                        }
                        else if (Cards_dataGridView.Rows[index].Cells["Card_name"].Value.ToString().Contains("[R]"))
                        {
                            Cards_dataGridView.Rows[index].Cells["Rarity"].Value = "Rare";
                        }
                        else if (Cards_dataGridView.Rows[index].Cells["Card_name"].Value.ToString().Contains("[P]"))
                        {
                            Cards_dataGridView.Rows[index].Cells["Rarity"].Value = "Premium";
                        }

                        //Read card quantity
                        br.BaseStream.Position = i;
                        Cards_dataGridView.Rows[index].Cells["Card_quantity"].Value = br.ReadByte() - 0x80;

                        ++Card_count;
                    }
                    label9.Text = $"Cards Found: {Card_count}";

                    //Read player name
                    br.BaseStream.Position = 0x12;
                    Player_name_textBox.Text = Encoding.Unicode.GetString(br.ReadBytes(0x0C));

                    //Read Rhythmia
                    br.BaseStream.Position = 0x2C;
                    Rhythmia_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read total playtime
                    br.BaseStream.Position = 0x38;
                    TimeSpan time = TimeSpan.FromSeconds(BitConverter.ToInt32(br.ReadBytes(0x04), 0));
                    Total_playtime_hours_numericUpDown.Value = Convert.ToInt16(Math.Floor(time.TotalHours));
                    Total_playtime_minutes_numericUpDown.Value = time.Minutes;
                    Total_playtime_seconds_numericUpDown.Value = time.Seconds;

                    //Read songs played
                    br.BaseStream.Position = 0x3738;
                    Songs_played_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read enemies defeated
                    br.BaseStream.Position = 0x373C;
                    Enemies_defeated_numericUpDown1.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read distance travelled
                    br.BaseStream.Position = 0x3740;
                    Distance_traveled_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read chained triggers
                    br.BaseStream.Position = 0x3748;
                    Chained_triggers_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read critical triggers
                    br.BaseStream.Position = 0x374C;
                    Critical_triggers_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read proficards recieved
                    br.BaseStream.Position = 0x3750;
                    ProfiCards_received_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read streetpasses
                    br.BaseStream.Position = 0x3754;
                    StreetPasses_numericUpDown.Value = BitConverter.ToInt16(br.ReadBytes(0x02), 0);

                    //Read total songs
                    br.BaseStream.Position = 0x3780;
                    Total_songs_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read basic scores cleared
                    br.BaseStream.Position = 0x3784;
                    Basic_scores_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read expert scores cleared
                    br.BaseStream.Position = 0x3788;
                    Expert_scores_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read ultimate scores cleared
                    br.BaseStream.Position = 0x378C;
                    Ultimate_scores_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read perfect chains achieved
                    br.BaseStream.Position = 0x3790;
                    Perfect_chains_achieved_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read daily specials cleared
                    br.BaseStream.Position = 0x3794;
                    Daily_specials_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read crowns recieved
                    br.BaseStream.Position = 0x3798;
                    Crowns_received_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read sss ranks recieved
                    br.BaseStream.Position = 0x379C;
                    SSS_ranks_received_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read total quests cleared
                    br.BaseStream.Position = 0x37B4;
                    Total_quests_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read short quests cleared
                    br.BaseStream.Position = 0x37B8;
                    Short_quests_cleared_numericUpDown.Value = BitConverter.ToInt16(br.ReadBytes(0x02), 0);

                    //Read medium quests cleared
                    br.BaseStream.Position = 0x37BA;
                    Medium_quests_cleared_numericUpDown.Value = BitConverter.ToInt16(br.ReadBytes(0x02), 0);

                    //Read long quests cleared
                    br.BaseStream.Position = 0x37BC;
                    Long_quests_cleared_numericUpDown.Value = BitConverter.ToInt16(br.ReadBytes(0x02), 0);

                    //Read inherited quests cleared
                    br.BaseStream.Position = 0x37BE;
                    Inherited_quests_cleared_numericUpDown.Value = BitConverter.ToInt16(br.ReadBytes(0x02), 0);

                    //Read bosses conquered
                    br.BaseStream.Position = 0x37C0;
                    Bosses_conquered_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read stages cleared
                    br.BaseStream.Position = 0x37C4;
                    Stages_cleared_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read keys used
                    br.BaseStream.Position = 0x37C8;
                    Keys_used_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read ai battle victories
                    br.BaseStream.Position = 0x37F6;
                    AI_battle_victories_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read scores played basic
                    br.BaseStream.Position = 0x272A;
                    Scores_played_basic_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read scores played expert
                    br.BaseStream.Position = 0x3750;
                    Scores_played_expert_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);

                    //Read ex bursts used
                    br.BaseStream.Position = 0x3810;
                    EX_bursts_used_numericUpDown.Value = BitConverter.ToInt32(br.ReadBytes(0x04), 0);
                }
            }
            catch
            {
                MessageBox.Show("Invalid savedata.bk", "Failed to open the file");
            }
        }
    }
}
