using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Anim
{
    public partial class MainPage : ContentPage
    {
        bool i = false;
        public MainPage()
        {
            InitializeComponent();
            
        }
        void Button1Clicked(object sender, EventArgs e)
        {
            if (!i)
            {
                lbl1.Text = "here we go again";
                i = true;
            } else
            {
                lbl1.Text = "help me plz";
                i = false;
            }
            Console.WriteLine("123");
        }
    }
}
