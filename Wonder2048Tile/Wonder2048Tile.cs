using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;
using System.IO;

namespace Wonder2048Tile {
    public partial class Wonder2048Tile : Form {
        private SoundPlayer _soundMove = null;
        private SoundPlayer _soundClick = null;
        private SoundPlayer _soundGameOver = null;
        private SoundPlayer _soundGameWin = null;
        private SoundPlayer _soundClickError = null;

        Random Rd = new Random();
        bool move = false;
        bool Presscontinue = false;
        private int _nScore;
        private string[] _arTH = {"ก","ข","ฃ","ค","ฅ", "ฆ",
                                  "ฉ","ง","จ","ฉ","ช","ซ","ฌ",
                                  "ญ","ฎ","ฏ","ฐ","ฑ","ฒ","ณ",
                                  "ด","ต","ถ","ท","ธ","น","บ",
                                  "ป","ผ","ฝ","พ","ฟ","ภ","ม",
                                  "ย","ร","ล","ว","ศ","ษ",
                                   "ส","ห","ฬ","อ","ฮ"};
        private string[] _arEngUpper = {"A","B","C","D","E","F","G",
                                        "H","I","J","K","L","M","N",
                                        "O","P","Q","R","S","T","U",
                                        "V","W","X","Y","Z"};
        private string[] _arEngLow = new string[26];

        public Wonder2048Tile() {
            InitializeComponent();
            this.BackColor = Color.FromArgb(219, 246, 252);
            _soundMove = new SoundPlayer(Properties.Resources.dropwater);
            _soundClick = new SoundPlayer(Properties.Resources.Click);
            _soundGameOver = new SoundPlayer(Properties.Resources.gameover);
            _soundGameWin = new SoundPlayer(Properties.Resources.gamewin);
            _soundClickError = new SoundPlayer(Properties.Resources.clickError);
        }
        //
        // Form Load
        //
        private void Wonder2048Tile_Load(object sender, EventArgs e) {

            // assign initial value to _arEngLow
            for (int i = 0; i < _arEngLow.Length; i++) {
                _arEngLow[i] = _arEngUpper[i].ToLower();
            }

            if (lblScore.Text == "") {
                lblScore.Text = "0";
            }

            if (lblHighScore.Text == "") {
                lblHighScore.Text = "0";
            }

            if (lblHighScore.Text == "0") {
                labelUsername.Text = "-";
            }

            // Decrypt
            string str = File.ReadAllText(Application.StartupPath + "/Highscore.txt");
            if (str != "") {
                string strDecrypt_Username = "";
                char str1 = ' ';
                int x = 0;
                int key = 13;
                for (int i = 0; i < 8; i++) {
                    str1 = Convert.ToChar(str[i]);

                    for (int k = 0; k < 26; k++) {
                        // Upper case
                        if (char.IsUpper(str1)) {
                            if (str1.ToString() == _arEngUpper[k]) {
                                x = (26 + (k - key)) % 26;
                                strDecrypt_Username += _arEngUpper[x];
                            }
                        }
                        // Lower case
                        if (char.IsLower(str1)) {
                            if (str1.ToString() == _arEngLow[k]) {
                                x = (26 + (k - key)) % 26;
                                strDecrypt_Username += _arEngLow[x];
                            }
                        }
                    }
                }
                // Display username can make best score
                labelUsername.Text = strDecrypt_Username;

                // Display best score
                string bestscoreStr = str.Substring(8);
                lblHighScore.Text = bestscoreStr;
            }

            // random num 2 - 4 when start game
            random_StartGame();
            random_StartGame();
            random_StartGame();
        }
        //
        // Color for tile
        //
        private void coloreForTile() {
            // Tile
            Label[,] tile = { {lbl1,lbl2,lbl3,lbl4},
                              {lbl5,lbl6,lbl7,lbl8},
                              {lbl9,lbl10,lbl11,lbl12},
                              {lbl13,lbl14,lbl15,lbl16}};
            // Shadow
            Label[,] tileBG = {{lblBG1,lblBG2,lblBG3,lblBG4},
                              {lblBG5,lblBG6,lblBG7,lblBG8},
                              {lblBG9,lblBG10,lblBG11,lblBG12},
                              {lblBG13,lblBG14,lblBG15,lblBG16}};
            // Colors
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tile[i, j].Text == "") {
                        tile[i, j].BackColor = System.Drawing.Color.WhiteSmoke;
                        tileBG[i, j].BackColor = Color.FromArgb(226, 226, 226);
                    }
                    if (tile[i, j].Text == "2") {
                        tile[i, j].BackColor = Color.FromArgb(255, 203, 125);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(255, 187, 84);
                    }
                    if (tile[i, j].Text == "4") {
                        tile[i, j].BackColor = Color.FromArgb(255, 185, 155);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(255, 156, 113);


                    }
                    if (tile[i, j].Text == "8") {
                        tile[i, j].BackColor = Color.FromArgb(156, 231, 248);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(128, 209, 255);
                    }
                    if (tile[i, j].Text == "16") {
                        tile[i, j].BackColor = Color.FromArgb(115, 189, 231);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(45, 167, 236);
                    }
                    if (tile[i, j].Text == "32") {
                        tile[i, j].BackColor = Color.FromArgb(197, 152, 233);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(171, 139, 239);
                    }
                    if (tile[i, j].Text == "64") {
                        tile[i, j].BackColor = Color.FromArgb(176, 121, 219);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(163, 99, 214);
                    }
                    if (tile[i, j].Text == "128") {
                        tile[i, j].BackColor = Color.FromArgb(255, 191, 218);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(255, 164, 235);
                    }
                    if (tile[i, j].Text == "256") {
                        tile[i, j].BackColor = Color.FromArgb(255, 169, 205);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(255, 146, 192);
                    }
                    if (tile[i, j].Text == "512") {
                        tile[i, j].BackColor = Color.FromArgb(255, 119, 176);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(233, 107, 160);
                    }
                    if (tile[i, j].Text == "1024") {
                        tile[i, j].BackColor = Color.FromArgb(255, 85, 156);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(235, 76, 143);
                    }
                    if (tile[i, j].Text == "2048") {
                        tile[i, j].BackColor = Color.FromArgb(255, 50, 136);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(237, 52, 130);
                    }
                    if (tile[i, j].Text == "4096") {
                        tile[i, j].BackColor = Color.FromArgb(255, 68, 68);
                        tile[i, j].ForeColor = System.Drawing.Color.White;
                        tileBG[i, j].BackColor = Color.FromArgb(231, 63, 63);
                    }
                }
            }
        }
        //
        // Start Game : Random 
        //
        private void random_StartGame() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            int x, y;
            int nNumRd;

            x = Rd.Next(0, 4);
            y = Rd.Next(0, 4);
            nNumRd = Rd.Next(0, 4);

            if (nNumRd % 2 == 0) tile[x, y].Text = "2";
            else tile[x, y].Text = "4";
            coloreForTile();
        }
        //
        // New Game : Random 
        //
        private void random_NewGame() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            for (int i = 0; i < 4; i++) {
                for (int k = 0; k < 4; k++) {
                    if (tile[i, k].Text != "") {
                        tile[i, k].Text = "";
                    }
                }
            }

            random_StartGame();
            random_StartGame();
            random_StartGame();

            coloreForTile();
        }
        //
        // Move tile : Random spawn 
        //
        public void random_MoveTile() { // Random when tile move
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            int x, y;
            if (move == true) {
                // Random Position
                do {
                    x = Rd.Next(0, 4);
                    y = Rd.Next(0, 4);
                } while (tile[x, y].Text != "");
                tile[x, y].Text = "2";
            }

            coloreForTile();
        }
        //
        // win2048 () : win
        //
        private void win2048() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};

            bool win = false;
            if (Presscontinue == false) {
                for (int i = 0; i < 4; i++) {
                    for (int k = 0; k < 4; k++) {
                        if (tile[i, k].Text == "2048") {
                            win = true;
                        }
                    }

                }
            }
            if (win) {
                _soundGameWin.Play();
                panelWin.Visible = true;
            }
        }
        //
        // Encrypt data
        //
        private void EncryptData(string username, string score) {
            string strEncrypt = "";
            char str = ' ';
            int y = 0;
            int key = 13;

            // Encrypt Letter
            for (int i = 0; i < 8; i++) {
                str = Convert.ToChar(username[i]);
                for (int k = 0; k < 26; k++) {
                    // Upper case
                    if (char.IsUpper(str)) {
                        if (str.ToString() == _arEngUpper[k]) {
                            y = (k + key) % 26;
                            strEncrypt += _arEngUpper[y];
                        }
                    }
                    // Lower case
                    if (char.IsLower(str)) {
                        if (str.ToString() == _arEngLow[k]) {
                            y = (k + key) % 26;
                            strEncrypt += _arEngLow[y];
                        }
                    }

                }

            }
            strEncrypt += score;
            // Clear old data in .text file and Write a new user and score 
            File.WriteAllText((Application.StartupPath + "/Highscore.txt"), String.Empty);
            File.WriteAllText((Application.StartupPath + "/Highscore.txt"), strEncrypt);
        }
        //
        // Game over
        //
        private void gameOver() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            int ok = 0; // Check tile back space
            for (int i = 0; i < 4; i++) {
                for (int j = 0; j < 4; j++) {
                    if (tile[i, j].Text == "") {
                        ok = 1;
                    }
                }
            }

            if (ok == 0) {
                bool gameover = true;

                // Check [between 0 and 2]
                for (int i = 0; i < 3; i++) {
                    for (int j = 0; j < 3; j++) {
                        if (tile[i, j].Text == tile[i + 1, j].Text ||
                            tile[i, j].Text == tile[i, j + 1].Text) {
                            gameover = false;
                        }
                    }
                }

                if ((tile[3, 0].Text == tile[3, 1].Text) ||
                    (tile[3, 1].Text == tile[3, 2].Text) ||
                    (tile[3, 2].Text == tile[3, 3].Text) ||
                    (tile[3, 0].Text == tile[2, 0].Text) ||
                    (tile[3, 1].Text == tile[2, 1].Text) ||
                    (tile[3, 2].Text == tile[2, 2].Text) ||
                    (tile[3, 3].Text == tile[2, 3].Text)) {
                    gameover = false;

                }

                if ((tile[0, 3].Text == tile[1, 3].Text) ||
                    (tile[1, 3].Text == tile[2, 3].Text)) {
                    gameover = false;
                }

                if (gameover == true) {
                    _soundGameOver.Play();
                    labelReset.Text = "New"; // Change text from "RESET" to "NEW" : when Game Over status show
                    panelGameOver.Visible = true;
                    int newScore = 0; // prevent error. When don't have any score, So newScore = 0 
                    int oldScore = 0; // prevent error. When don't have any score, So oldScore = 0
                    // import number from .txt file
                    string str = File.ReadAllText(Application.StartupPath + "/Highscore.txt");
                    string strOldScore = "";

                    newScore = Convert.ToInt32(lblScore.Text);
                    for (int i = 8; i < str.Length; i++) {
                        strOldScore += str[i];
                    }

                    if (str != "") {
                        string str_OldScore = str.Substring(8);
                        oldScore = Convert.ToInt32(strOldScore);
                    }

                    // Write new username and score : new score  more than old score
                    if (newScore > oldScore) {
                        labelUsername.Text = textUsername.Text; // USER Display
                        lblHighScore.Text = newScore.ToString(); // Score Display
                        EncryptData(labelUsername.Text, newScore.ToString());
                    }
                }
            }
        }
        //
        // new or Reset
        //
        private void newORreset() {
            random_NewGame();
            random_NewGame();
            random_NewGame();
            lblScore.Text = "0";
            _nScore = 0;
        }
        //
        // Down
        //
        private void Down() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};

            for (int i = 2; i >= 0; i--) {
                int slide = i;
                for (int j = 0; j < 4; j++) { // [ - , j ]
                    for (slide = i; slide < 3; slide++) {
                        if (tile[slide + 1, j].Text == "" && tile[slide, j].Text != "") {
                            tile[slide + 1, j].Text = tile[slide, j].Text;
                            tile[slide, j].Text = "";
                            coloreForTile();
                            move = true;

                        }

                    }
                }
            }
        }
        //
        // Up
        //
        private void Up() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 4; j++) {
                    for (int slide = 3; slide > 0; slide--) {
                        if (tile[slide - 1, j].Text == "" && tile[slide, j].Text != "") {
                            tile[slide - 1, j].Text = tile[slide, j].Text;
                            tile[slide, j].Text = "";
                            coloreForTile();
                            move = true;
                        }
                    }
                }
            }
        }
        //
        // Left
        //
        private void Left() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            for (int i = 0; i < 4; i++) {
                int slide = i;
                for (int j = 1; j < 4; j++) {
                    for (slide = j; slide > 0; slide--) {
                        if (tile[i, slide - 1].Text == "" && tile[i, slide].Text != "") {
                            tile[i, slide - 1].Text = tile[i, slide].Text;
                            tile[i, slide].Text = "";
                            coloreForTile();
                            move = true;
                        }


                    }
                }
            }
        }
        //
        // Right
        //
        private void Right() {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            for (int i = 0; i < 4; i++) {
                for (int j = 2; j >= 0; j--) {
                    for (int slide = j; slide < 3; slide++) {
                        if (tile[i, slide + 1].Text == "" && tile[i, slide].Text != "") {
                            tile[i, slide + 1].Text = tile[i, slide].Text;
                            tile[i, slide].Text = "";
                            coloreForTile();
                            move = true;
                        }
                    }
                }
            }
        }
        //
        // Key check
        //
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            if (panelLogin.Visible == false) {
                if (panelWin.Visible == false) {
                    if (panelGameOver.Visible == false) {
                        switch (keyData) {
                            // KEY DOWN 
                            case Keys.Down:
                                move = false;
                                Down();
                                gameOver();
                                for (int i = 2; i >= 0; i--) {
                                    for (int j = 0; j < 4; j++) {
                                        if (tile[i + 1, j].Text == tile[i, j].Text) {
                                            if (tile[i, j].Text != "") {
                                                _soundMove.Play();
                                                int s = Convert.ToInt32(tile[i + 1, j].Text);
                                                int m = Convert.ToInt32(tile[i, j].Text);
                                                s = s + m;
                                                tile[i + 1, j].Text = s.ToString();
                                                tile[i, j].Text = "";
                                                coloreForTile();
                                                _nScore = Convert.ToInt32(lblScore.Text) + Convert.ToInt32(tile[i + 1, j].Text);
                                                lblScore.Text = _nScore.ToString();
                                                move = true;
                                            }
                                        }
                                    }
                                }
                                Down();
                                if (move == true) random_MoveTile();
                                break;
                            // KEY UP
                            case Keys.Up:
                                move = false;
                                Up();
                                gameOver();
                                for (int i = 1; i < 4; i++) {
                                    for (int j = 0; j < 4; j++) {
                                        if (tile[i - 1, j].Text == tile[i, j].Text) {
                                            if (tile[i, j].Text != "") {
                                                _soundMove.Play();
                                                int s = Convert.ToInt32(tile[i - 1, j].Text);
                                                int m = Convert.ToInt32(tile[i, j].Text);
                                                s = s + m;
                                                tile[i - 1, j].Text = s.ToString();
                                                tile[i, j].Text = "";
                                                coloreForTile();
                                                _nScore = Convert.ToInt32(lblScore.Text) + Convert.ToInt32(tile[i - 1, j].Text);
                                                lblScore.Text = _nScore.ToString();
                                                move = true;
                                            }
                                        }
                                    }
                                }
                                Up();
                                if (move == true) random_MoveTile();
                                break;
                            // KEY LEFT
                            case Keys.Left:
                                move = false;
                                Left();
                                gameOver();
                                for (int i = 0; i < 4; i++) {
                                    for (int j = 1; j < 4; j++) {
                                        if (tile[i, j - 1].Text == tile[i, j].Text) {
                                            if (tile[i, j].Text != "") {
                                                _soundMove.Play();
                                                int s = Convert.ToInt32(tile[i, j - 1].Text);
                                                int m = Convert.ToInt32(tile[i, j].Text);
                                                s = s + m;
                                                tile[i, j - 1].Text = s.ToString();
                                                tile[i, j].Text = "";
                                                coloreForTile();
                                                _nScore = Convert.ToInt32(lblScore.Text) + Convert.ToInt32(tile[i, j - 1].Text);
                                                lblScore.Text = _nScore.ToString();
                                                move = true;
                                            }
                                        }
                                    }
                                }
                                Left();
                                if (move == true) random_MoveTile();
                                break;
                            // KEY RIGHT
                            case Keys.Right:
                                move = false;
                                Right();
                                gameOver();
                                for (int i = 0; i < 4; i++) {
                                    for (int j = 2; j >= 0; j--) {
                                        if (tile[i, j + 1].Text == tile[i, j].Text) {
                                            if (tile[i, j].Text != "") {
                                                _soundMove.Play();
                                                int s = Convert.ToInt32(tile[i, j + 1].Text);
                                                int m = Convert.ToInt32(tile[i, j].Text);
                                                s = s + m;
                                                tile[i, j + 1].Text = s.ToString();
                                                tile[i, j].Text = "";
                                                coloreForTile();
                                                _nScore = Convert.ToInt32(lblScore.Text) + Convert.ToInt32(tile[i, j + 1].Text);
                                                lblScore.Text = _nScore.ToString();
                                                move = true;
                                            }
                                        }
                                    }
                                }
                                Right();
                                if (move == true) random_MoveTile();
                                break;
                        }
                    }
                    win2048();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        //
        //  labelReset : New Game
        //
        private void labelReset_Click(object sender, EventArgs e) {
            labelReset.Text = "RESET";
            panelWin.Visible = false;
            panelGameOver.Visible = false;
            newORreset();
        }
        private void labelReset_MouseHover(object sender, EventArgs e) {
            labelReset.BackColor = Color.FromArgb(206, 206, 255);
        }
        private void labelReset_MouseLeave(object sender, EventArgs e) {
            labelReset.BackColor = Color.FromArgb(216, 216, 255);//93, 56
        }
        private void labelReset_MouseClick(object sender, MouseEventArgs e) {
            labelReset.BackColor = Color.FromArgb(195, 195, 240);
            _soundClick.Play();
        }
        private void labelReset_MouseDown(object sender, MouseEventArgs e) {
            labelReset.BackColor = Color.FromArgb(216, 216, 255);
        }
        //
        // labelLogin : New Player
        //
        private void labelLogin_Click(object sender, EventArgs e) {
            Label[,] tile = {{lbl1,lbl2,lbl3,lbl4},
                             {lbl5,lbl6,lbl7,lbl8},
                             {lbl9,lbl10,lbl11,lbl12},
                             {lbl13,lbl14,lbl15,lbl16}};
            Label[,] tileBG = {{lblBG1,lblBG2,lblBG3,lblBG4},
                              {lblBG5,lblBG6,lblBG7,lblBG8},
                              {lblBG9,lblBG10,lblBG11,lblBG12},
                              {lblBG13,lblBG14,lblBG15,lblBG16}};
            textUsername.Text = "";
            panelLogin.Visible = true;
            pictureBox3.Visible = false;
            panelWin.Visible = false;
            panelGameOver.Visible = false;
            // Clear tile to default
            for (int i = 0; i < 4; i++) {
                for (int k = 0; k < 4; k++) {
                    tile[i, k].Text = "";
                    tile[i, k].BackColor = System.Drawing.Color.WhiteSmoke;
                    tileBG[i, k].BackColor = Color.FromArgb(226, 226, 226);
                }
            }
            newORreset();
        }
        private void labelLogin_MouseHover(object sender, EventArgs e) {
            labelLogin.BackColor = Color.FromArgb(255, 181, 255);
        }
        private void labelLogin_MouseLeave(object sender, EventArgs e) {
            labelLogin.BackColor = Color.FromArgb(255, 192, 255);

        }
        private void labelLogin_MouseClick(object sender, MouseEventArgs e) {
            labelLogin.BackColor = Color.FromArgb(240, 170, 240);
            _soundClick.Play();

        }
        private void labelLogin_MouseDown(object sender, MouseEventArgs e) {
            labelLogin.BackColor = Color.FromArgb(255, 192, 255);
        }
        //
        // labelContinue : win > continue
        //
        private void labelContinue_Click(object sender, EventArgs e) {
            panelWin.Visible = false;
            Presscontinue = true;
        }
        private void labelContinue_MouseClick(object sender, MouseEventArgs e) {
            labelContinue.BackColor = Color.FromArgb(195, 195, 240);
            _soundClick.Play();
        }
        private void labelContinue_MouseDown(object sender, MouseEventArgs e) {
            labelContinue.BackColor = Color.FromArgb(216, 216, 255);
        }
        private void labelContinue_MouseHover(object sender, EventArgs e) {
            labelContinue.BackColor = Color.FromArgb(206, 206, 255);
        }
        private void labelContinue_MouseLeave(object sender, EventArgs e) {
            labelContinue.BackColor = Color.FromArgb(216, 216, 255);//93, 56
        }
        //
        // labelExit : win > end game
        //
        private void labelExit_Click_2(object sender, EventArgs e) {
            Application.ExitThread();
        }
        private void labelExit_MouseClick(object sender, MouseEventArgs e) {
            labelExit.BackColor = Color.FromArgb(240, 170, 240);
        }
        private void labelExit_MouseDown(object sender, MouseEventArgs e) {
            labelExit.BackColor = Color.FromArgb(255, 192, 255);
        }
        private void labelExit_MouseHover(object sender, EventArgs e) {
            labelExit.BackColor = Color.FromArgb(255, 181, 255);
        }
        private void labelExit_MouseLeave(object sender, EventArgs e) {
            labelExit.BackColor = Color.FromArgb(255, 192, 255);
        }
        //
        // labelPlay : New player : PLAY
        //
        private void labelPlay_Click(object sender, EventArgs e) {
            // Check lang
            string str = textUsername.Text;
            string str1 = "";
            bool lang = false;
            if (textUsername.Text != "") {
                labelError.Visible = false;
                labelNotError.Visible = true;
                for (int k = 0; k < str.Length; k++) {
                    for (int i = 0; i < _arTH.Length; i++) {
                        str1 = Convert.ToString(str[k]);
                        if (str1 == _arTH[i]) {
                            labelError.Visible = true;
                            labelNotError.Visible = false;
                            lang = true;
                        }
                    }
                }
            }// Checklang

            if (textUsername.Text == "" || lang) {
                _soundClickError.Play();
                labelError.Visible = true;
                return;
            }

            int x = str.Length;
            if (x < 8 && str != "") {
                _soundClickError.Play();
                labelLeastChar.Visible = true;
                labelError.Visible = false;
                return;
            }

            _soundClick.Play();
            labelReset.Text = "RESET";
            labelLeastChar.Visible = false;
            labelNotError.Visible = false;
            labelError.Visible = false;
            panelLogin.Visible = false;
            pictureBox3.Visible = true; // whale picture
        }
        private void labelPlay_MouseDown(object sender, MouseEventArgs e) {
            labelPlay.BackColor = Color.FromArgb(216, 216, 255);
        }
        private void labelPlay_MouseHover(object sender, EventArgs e) {
            labelPlay.BackColor = Color.FromArgb(206, 206, 255);
        }
        private void labelPlay_MouseLeave(object sender, EventArgs e) {
            labelPlay.BackColor = Color.FromArgb(216, 216, 255);
        }
        private void labelPlay_MouseClick(object sender, MouseEventArgs e) {
            labelPlay.BackColor = Color.FromArgb(195, 195, 240);
        }
        //
        // textUsername : login
        //
        private void textUsername_KeyPress(object sender, KeyPressEventArgs e) {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar)) {
                return;
            }
            e.Handled = true;
        }
        private void textUsername_TextChanged(object sender, EventArgs e) {
            string str = textUsername.Text;
            string str1 = "";

            // Check Length : 8
            int x = str.Length;
            if (x < 8 && str != "") {
                labelLeastChar.Visible = true;
                labelError.Visible = false;
                labelNotError.Visible = false;
            }

            if (x == 0 || x == 8)labelLeastChar.Visible = false;

            // Check empty string
            if (str != "" && x == 8) {
                labelError.Visible = false;
                labelNotError.Visible = true;
            }

            if (str == "") labelNotError.Visible = false;

            // Check Thai language
            for (int k = 0; k < str.Length; k++) {
                for (int i = 0; i < _arTH.Length; i++) {
                    str1 = Convert.ToString(str[k]);
                    if (str1 == _arTH[i]) {
                        labelError.Visible = true;
                        labelNotError.Visible = false;
                        labelLeastChar.Visible = false;
                    }
                }
            }
            if (str == "") labelError.Visible = false;
        }
    }
}