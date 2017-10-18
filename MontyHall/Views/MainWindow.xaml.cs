using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MontyHall.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand;
        private GameStats stats;
        private Button[] doorSet;
        private bool switched;
        private int correct;
        private int chosen;
        private int revealed;
        private string noun;
        private string adjective;
        private string verb;
        private string[] nounList;
        private string[] adjectiveList;
        private string[] verbList;

        public MainWindow()
        {
            InitializeComponent();
            InitializeFields();
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            revealed = 0;
            correct = new Random().Next(1, 4);
            for (int i = 0; i < 3; i++)
            {
                doorSet[i].IsEnabled = true;
                doorSet[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                string doorText = "Door " + (i + 1);
                doorSet[i].Content = doorText;
                doorSet[i].Visibility = Visibility.Visible;
            }
            textQuestion.Text = "Behind one of these doors is a car! Behind the other two? Certain death.\n" +
                                "Make your choice.";
            textQuestion.Visibility = Visibility.Visible;
            buttonStart.Visibility = Visibility.Collapsed;
            buttonStats.Visibility = Visibility.Collapsed;
        }

        private void buttonStats_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.Visibility = Visibility.Collapsed;
            buttonStats.Visibility = Visibility.Collapsed;
            textStats.Visibility = Visibility.Visible;
            textStats.Foreground = Brushes.White;
            textStats.Text = "Total Games: " + stats.GamesTotal + "\n" +
                             "Total Wins: " + stats.GamesWon + "\n" +
                             "Total Losses: " + stats.GamesLost + "\n" +
                             "Percent Wins: " + stats.PercentWins.ToString("f2") + "%\n\n" +

                             "Total Games (Switched): " + stats.SwitchedGamesTotal + "\n" +
                             "Total Wins (Switched): " + stats.SwitchedGamesWon + "\n" +
                             "Total Losses (Switched): " + stats.SwitchedGamesLost + "\n" +
                             "Percent Wins (Switched): " + stats.PercentSwitchedWins.ToString("f2") + "%\n\n" +

                             "Total Games (Kept): " + stats.KeptGamesTotal + "\n" +
                             "Total Wins (Kept): " + stats.KeptGamesWon + "\n" +
                             "Total Losses (Kept): " + stats.KeptGamesLost + "\n" +
                             "Percent Wins (Kept): " + stats.PercentKeptWins.ToString("f2") + "%";

            buttonStatsExit.Visibility = Visibility.Visible;

        }

        private void RevealDoor(int door)
        {
            chosen = door;
            if (chosen == correct)
            {
                revealed = rand.Next(1, 3) + door;
                if (revealed > 3)
                {
                    revealed %= 3;
                }
            }
            else
            {
                revealed = 6 - (correct + chosen);
            }
            adjective = adjectiveList[rand.Next(0, adjectiveList.Length)];
            noun = nounList[rand.Next(0, nounList.Length)];
            int doorIndex = revealed - 1;
            doorSet[doorIndex].IsEnabled = false;
            doorSet[doorIndex].HorizontalContentAlignment = HorizontalAlignment.Left;
            doorSet[doorIndex].Content = adjective.PadLeft(15) + "\r\n" + noun.PadLeft(15);
            textQuestion.Text = "Wait! Door " + revealed + " has been revealed to contain " + adjective + 
                                " " + noun + "!!\nNow which door will you choose?!";
        }

        private void buttonDoor1_Click(object sender, RoutedEventArgs e)
        {
            DoorClick(1);
        }

        private void buttonDoor2_Click(object sender, RoutedEventArgs e)
        {
            DoorClick(2);
        }

        private void buttonDoor3_Click(object sender, RoutedEventArgs e)
        {
            DoorClick(3);
        }

        private void DoorClick(int door)
        {
            if (revealed == 0)
            {
                RevealDoor(door);
            }
            else if (revealed < 4)
            {
                if (chosen != door)
                {
                    chosen = door;
                    switched = true;
                }
                else
                {
                    switched = false;
                }

                if (chosen == correct)
                {
                    if (switched)
                    {
                        stats.SwitchedGamesWon++;
                    }
                    else
                    {
                        stats.KeptGamesWon++;
                    }
                    FinalReveal(true, chosen);
                }
                else
                {
                    if (switched)
                    {
                        stats.SwitchedGamesLost++;
                    }
                    else
                    {
                        stats.KeptGamesLost++;
                    }
                    FinalReveal(false, chosen);
                }
            }
        }

        private void FinalReveal(bool winner, int door)
        {
            revealed = 4;
            if (winner)
            {
                int doorIndex = door - 1;
                doorSet[doorIndex].Content = "BRAND NEW CAR!!";
                textQuestion.Text = "Congratulations!! You won A BRAND NEW CAR!!!";
            }
            else
            {
                verb = verbList[rand.Next(0, verbList.Length)];
                textQuestion.Text = "Oh no!! You were " + verb + " by " + adjective + " " + noun + "!!!";
                int doorIndex = door - 1;
                doorSet[doorIndex].HorizontalContentAlignment = HorizontalAlignment.Left;
                doorSet[doorIndex].Content = adjective.PadLeft(15) + "\r\n" + noun.PadLeft(15);
            }
            buttonMainMenu.Visibility = Visibility.Visible;
        }

        private void buttonStatsExit_Click(object sender, RoutedEventArgs e)
        {
            textStats.Visibility = Visibility.Collapsed;
            textStats.Text = "";
            buttonStatsExit.Visibility = Visibility.Collapsed;
            buttonStart.Visibility = Visibility.Visible;
            buttonStats.Visibility = Visibility.Visible;
        }

        private void buttonMainMenu_Click(object sender, RoutedEventArgs e)
        {
            buttonMainMenu.Visibility = Visibility.Collapsed;
            for (int i = 0; i < 3; i++)
            {
                doorSet[i].Visibility = Visibility.Collapsed;
            }
            textQuestion.Visibility = Visibility.Collapsed;
            buttonStart.Visibility = Visibility.Visible;
            buttonStats.Visibility = Visibility.Visible;
        }

        private void InitializeFields()
        {
            rand = new Random();
            stats = new GameStats();
            doorSet = new Button[] { buttonDoor1, buttonDoor2, buttonDoor3 };
            adjectiveList = new string[] {  "DEADLY", "LETHAL", "TOXIC", "MURDEROUS",
                                            "DEADLY DEADLY", "HOMICIDAL" , "VENOMOUS", "POISONOUS",
                                            "KILLER", "EXPLOSIVE", "MUTANT", "DEMONIC",
                                            "PSYCHOTIC", "FIERY", "RADIOACTIVE", "ENRAGED" };

            nounList = new string[] {   "GHOSTS", "ANTHRAX", "BEES", "SHARKS", "BABIES", "GOATS",
                                        "SNAKES", "SPIDERS", "ANDROIDS", "KNIVES", "GUILLOTINES",
                                        "NEEDLES", "CARCINOGENS", "ZOMBIES", "VAMPIRES", "NINJAS",
                                        "LAWNMOWERS", "ACIDS", "PIRATES", "LASERS", "PUPPETS" };

            verbList = new string[] {   "OBLITERATED", "MURDERED", "DESTROYED", "EVISCERATED",
                                        "DISINTEGRATED", "DEVOURED", "ANNIHILATED", "KILLED",
                                        "EXECUTED", "ASPHYXIATED", "STRANGLED", "DISEMBOWELED",
                                        "DISMEMBERED", "DECAPITATED", "TORTURED TO DEATH",
                                        "DRAWN AND QUARTERED", "DIGESTED", "MELTED", "FLATTENED",
                                        "ENDED", "IMPALED", "CHOPPED UP", "TRAMPLED", "LIQUIFIED",
                                        "BURNED TO A CRISP" };
        }
    }
}
