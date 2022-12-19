using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RaceYa.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpForARaceTabbedPage : TabbedPage
    {
        public static int? SelectedTab;

        public SignUpForARaceTabbedPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SwitchTabs();
        }

        public void SwitchTabs()
        {
            if (SelectedTab == 0)
            {
                CurrentPage = Children[0];
                SelectedTab = null;
            }
            else if (SelectedTab == 1)
            {
                CurrentPage = Children[1];
                SelectedTab = null;
            }
            else if (SelectedTab == 2)
            {
                CurrentPage = Children[2];
                SelectedTab = null;
            }
        }
    }
}