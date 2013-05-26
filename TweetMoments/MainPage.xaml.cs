using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.Net.NetworkInformation;

namespace TweetMoments
{
    public class TwitterItem
    {
        public string username { get; set; }
        public string tuit { get; set; }
        public string imageSource { get; set; }
        public string created_at { get; set; }
    }

    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            MessageBox.Show("Para que la aplicación funcione correctamente asegurate de tener acceso a internet");
            if (ChecarInternet())
            {
                aviso.Text = "";
            }
            else
            {
                aviso.Text = "No Conectado a Internet";
            }
        }

        private bool ChecarInternet()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                MessageBox.Show("No tienes conexión de internet.");
                return false;
            }
            else
            {
                //MessageBox.Show("Tienes conexión de internet.");
                return true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtuser.Text == string.Empty || txtuser.Text == "Inserta una cuenta...")
            {
                MessageBox.Show("Inserta una cuenta de usuario para poder ver los tweets");
            }
            else
            {
                WebClient Twitter = new WebClient();
                Twitter.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Twitter_DownloadStringCompleted);
                Twitter.DownloadStringAsync(new Uri("http://api.twitter.com/1/statuses/user_timeline.xml?screen_name=" + txtuser.Text + "&count=40"));
            }
        }

        private void Twitter_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //throw new NotImplementedException();
            if (e.Error != null)
            {
                MessageBox.Show("Usuario inexistente.");
                return;
            }

            XElement xmlTweets = XElement.Parse(e.Result);
            if (xmlTweets.IsEmpty)
            {
                MessageBox.Show("El usuario no ha escrito nuevos tweets.");
            }
            listbox.ItemsSource = from Tweet in xmlTweets.Descendants("status")
                                  select new TwitterItem
                                  {
                                      imageSource = Tweet.Element("user").Element("profile_image_url").Value,
                                      username = Tweet.Element("user").Element("name").Value,
                                      tuit = Tweet.Element("text").Value,
                                      created_at = Tweet.Element("created_at").Value
                                  };
        }

        private void ContentPanel_Loaded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Para usar esta aplicación inserta el nombre exacto de la cuenta del usuario para poder ver sus ultimos tweets.");
        }

        private void txtuser_MouseEnter(object sender, MouseEventArgs e)
        {
            txtuser.Text = "";
        }
    }
}