/*
 * Travis Cartmell
 * 
 * This program is less a pong game, and more an online game of racketball. Ball bounces off everything, but the left wall, which is how
 * you lose. The game can be restarted at any point by pressing the R key, space starts/pauses the game, and the ESC key exits the program.
 * These can also be viewed from within the "Info" menu.
 * 
 * Extra Credit Possibilities:
 * 
 * Sound is implemented to happen at every bounce with two different sound files for some variety.
 * I also downloaded a custom font specifically only for the score.
 * Ball also speeds up by 20% for every point.
 * Two different sets of hotkeys are available, the standard for start/pause, quit, and restart. And a set for people that don't know what
 * a computer is (alt + first letter of every command).
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnimateBall
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer = new DispatcherTimer();
        private double dx = 2;
        private double dy = 2;
        private double vertDirection = -1;
        private double horizDirection = 1;

        private double gameBallTop = 0;
        private double gameBallLeft = 0;

        private double gamePaddleTop = 0;
        private double gamePaddleLeft = 0;
        private double gamePaddleDy = 10;

        private int score = 0;

        MediaPlayer player = new MediaPlayer();
        MediaPlayer player2 = new MediaPlayer();



        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            gameTimer.Tick += GameTimer_Tick;

            gameBallTop = Canvas.GetTop(gameBall);
            gameBallLeft = Canvas.GetLeft(gameBall);

            gamePaddleTop = Canvas.GetTop(gamePaddle);
            gamePaddleLeft = Canvas.GetLeft(gamePaddle);

            player.Open(new Uri(@"../../sounds/blip.wav", UriKind.Relative));
            player2.Open(new Uri(@"../../sounds/bloop.wav", UriKind.Relative));

            player.Volume = .2;
            player2.Volume = .2;


        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            //double xComponent = gameBall;
            //double yComponent = 0;
            if (gameBallTop <= 0  || gameBallTop >= (myGameCanvas.Height - gameBall.Height))
            {
                player2.Stop();
                player2.Position = TimeSpan.FromSeconds(0);
                player2.Play();

                vertDirection *= -1;
            }
            if (gameBallLeft >= myGameCanvas.Width - gameBall.Width)
            {
                player.Stop();
                player.Position = TimeSpan.FromSeconds(0);
                player.Play();

                horizDirection *= -1;
            }

            if (gameBallLeft <= 0)
            {
                if (score < 10)
                    lbl_score.Content = "DEAD\n0" + score;
                else
                    lbl_score.Content = "DEAD\n" + score;

                gameTimer.Stop();
            }

            if (gamePaddleLeft + gamePaddle.Width >= gameBallLeft
                && gameBallTop + gameBall.Height/2 >= gamePaddleTop && gameBallTop + gameBall.Height / 2 <= gamePaddleTop + gamePaddle.Height)
            {
                player.Stop();
                player.Position = TimeSpan.FromSeconds(0);
                player.Play();

                horizDirection = 1;
                score++;
                dx *= 1.2;
                dy *= 1.2;

                if (score < 10)
                    lbl_score.Content = "0" + score;
                else
                    lbl_score.Content = score;
            }
            

            gameBallLeft += dx * horizDirection;
            gameBallTop += dy * vertDirection;

            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);

        }

        private void Restart()
        {
            dx = 2;
            dy = 2;
            vertDirection = -1;
            horizDirection = 1;

            gameBallTop = 0;
            gameBallLeft = 0;

            gamePaddleTop = 0;
            gamePaddleLeft = 0;
            gamePaddleDy = 5;

            gameBallTop = 230;
            gameBallLeft = 100;

            gamePaddleTop = 200;
            gamePaddleLeft = 5;

            Canvas.SetTop(gamePaddle, gamePaddleTop);

            Canvas.SetTop(gameBall, gameBallTop);
            Canvas.SetLeft(gameBall, gameBallLeft);

            score = 0;
            lbl_score.Content = "00";

            gameTimer.Stop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Up)
            {
                if (gamePaddleTop - gamePaddleDy >= 0)
                    gamePaddleTop -= gamePaddleDy;
                Canvas.SetTop(gamePaddle, gamePaddleTop);
            }
            else if (e.Key == Key.Down)
            {
                if (gamePaddleTop + gamePaddleDy + gamePaddle.Height <= myGameCanvas.Height)
                    gamePaddleTop += gamePaddleDy;
                Canvas.SetTop(gamePaddle, gamePaddleTop);
            }

            else if (e.Key == Key.Space)
            {
                if (gameTimer.IsEnabled)
                    gameTimer.Stop();
                else
                    gameTimer.Start();
            }

            else if (e.Key == Key.R)
            {
                Restart();
            }

            else if (e.Key == Key.Escape)
            {
                Environment.Exit(0);
            }

        }

        private void Menu_quit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Menu_pause_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();
        }

        private void Menu_start_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Start();
        }

        private void Menu_info_Click(object sender, RoutedEventArgs e)
        {
            gameTimer.Stop();

            MessageBox.Show("Start/Stop: Space key\nRestart: R key\nExit: ESC key", "Useless Box");

        }

        private void Menu_restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }
    }
}
