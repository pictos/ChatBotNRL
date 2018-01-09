using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ChatBotNRL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoboNRLView : ContentPage
    {
        public RoboNRLView()
        {
            InitializeComponent();
            BindingContext = new ViewModel.RoboNRLViewModel();
        }
    }
}