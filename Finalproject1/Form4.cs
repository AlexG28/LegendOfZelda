using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//THIS IS THE WINNING SCREEN 
namespace Finalproject1
{
    public partial class Form4 : Form
    {
        // creates all the text, rectangleFs and fonts for the logo, background and 'you lost' text
        RectangleF backgroundImage = new RectangleF(0, 0, 1300, 700);
        RectangleF gameLogoImage = new RectangleF(520, 50, 260, 140);

        PointF youLostPoint = new PointF(200, 200);
        Font youLostFont = new Font("Ariel", 25.0f);
        string youLostText = "You Won!";

        public Form4()
        {
            InitializeComponent();
        }

        private void btnTryAgain_Click(object sender, EventArgs e) // if the user presses the play again button
        {
            // go to form 2 
            Form2 frmGame = new Form2();
            frmGame.Show();
            this.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // draw the background, zelda logo and 'you lost' text
            e.Graphics.DrawImage(Properties.Resources.background2, backgroundImage);
            e.Graphics.DrawImage(Properties.Resources.Zelda_Logo_svg, gameLogoImage);
            e.Graphics.DrawString(youLostText, youLostFont, Brushes.Black, youLostPoint);

        }
    }
}
