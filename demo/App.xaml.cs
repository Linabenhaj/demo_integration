namespace demo;  // 👈 MOET demo zijn, niet demo_app_rabbit of iets anders

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
}