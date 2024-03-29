﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Damka.Classes;

using System.IO;
using System.Runtime.Serialization;//!!!!!!
using System.Runtime.Serialization.Formatters.Binary;

namespace Damka
{
    public partial class Damka : Form
    {
        GameClass game = new GameClass();

        public Damka()
        {
            InitializeComponent();
            gameLoad();
        }

        private void gameLoad()
        {
            drawBoard();
            //placePlayers();
        }

        // Draws all the buttons and adds them to game List<Button> _board
        private void drawBoard()
        {
            Panel gamePanel = new Panel();
            gamePanel.Width = Constants.PANEL_SIZE;
            gamePanel.Height = Constants.PANEL_SIZE;
            gamePanel.BackColor = Color.Yellow;
            this.Controls.Add(gamePanel);
            for (int row = 0; row < Constants.NUM_OF_ROWS; row++)
            {
                for (int col = 0; col < Constants.NUM_OF_COLS; col++)
                {
                    Button btn = new Button();
                    btn.ForeColor = Color.White;
                    btn.Text = (row * Constants.NUM_OF_COLS + col).ToString();
                    btn.Name = (row * Constants.NUM_OF_COLS + col).ToString();
                    btn.Size = new Size(Constants.BUTTON_SIZE, Constants.BUTTON_SIZE);
                    btn.Location = new Point(col * Constants.BUTTON_SIZE, row * Constants.BUTTON_SIZE);
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.FlatAppearance.BorderSize = 0;
                    if ((row + col) % 2 == 0)
                    {
                        btn.AccessibleDescription = "LIGHT_BROWN";
                        btn.BackColor = Constants.LIGHT_BROWN;
                    }
                    else
                    {
                        btn.AccessibleDescription = "DARK_BROWN";
                        btn.BackColor = Constants.DARK_BROWN;
                    }
                    btn.Click += new EventHandler(boardClick);
                    gamePanel.Controls.Add(btn);
                    game.addButtonToBoard(btn);
                    game.initializePlayers(btn, col, row);
                    game.setGamePhase();
                    game.ShowAvailablePieces();
                }
            }
        }

        // Checks the current GamePhase and initiate a proper response
        private void boardClick(object sender, EventArgs e)
        {
            int pressedIndex = int.Parse(((Button)sender).Name);
            // game.getBoard()[30].Text = game.getCurrentGamePhase() + "";
            game.nextGamePhase(pressedIndex);
        }

        //--- SAVE --
        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            saveFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //!!!!
                    formatter.Serialize(stream, game);
                }
            }
        }

        //--- LOAD --
        private void loadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //!!!!
                /*                pts = (FigureList)binaryFormatter.Deserialize(stream);
                                pictureBox1.Invalidate();*/
                game = (GameClass)binaryFormatter.Deserialize(stream);
            }
        }
    }
}
