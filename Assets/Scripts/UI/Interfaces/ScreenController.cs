namespace Wazzapps.UI
{
    public interface IScreenController
    {
        void GoToScreen<T>(IScreen currentScreen) where T : IScreen;
        void GoToScreen<T>(IScreen currentScreen, object[] args) where T : IScreen;
        void GoToScreenAndReset<T>(IScreen currentScreen) where T : IScreen;
        void GoBackScreen(IScreen currentScreen);
        void GoFirstScreen(IScreen currentScreen);
        void GoFirstScreenAndReset(IScreen currentScreen);
    }
}
