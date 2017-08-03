namespace Wazzapps.UI
{
    public interface IScreen
    {
        void Init(IScreenController controller);
        void Show(object[] args);
        void Return();
        void Hide();
    }
}