using System.Collections.Generic;
using UnityEngine;

namespace Wazzapps.UI
{
    public class ScreenController : MonoBehaviour, IScreenController
    {
        public AbstractScreen FirstScreen;
        public AbstractScreen[] Screens;

        private AbstractScreen _firstAbstractScreen;

        private readonly List<IScreen> _createdScreenList = new List<IScreen>();
        private readonly Stack<IScreen> _screenStack = new Stack<IScreen>();

        void Awake()
        {
            AbstractScreen[] screens = FindObjectsOfType<AbstractScreen>();
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].gameObject.SetActive(false);
            }

            if (FirstScreen.gameObject.scene == null)
            {
                GameObject firstScreen = Instantiate(FirstScreen.gameObject);
                _firstAbstractScreen = firstScreen.GetComponent<AbstractScreen>();
            }
            else
            {
                _firstAbstractScreen = FirstScreen.GetComponent<AbstractScreen>();
            }
            _createdScreenList.Add(_firstAbstractScreen);

            _firstAbstractScreen.Init(this);
            GoToScreen(null, _firstAbstractScreen, new object[] { true });
        }

        public void GoToScreen<T>(IScreen currentScreen) where T : IScreen
        {
            IScreen nextScreen = FindScreenOfType<T>();
            GoToScreen(currentScreen, nextScreen);
        }

        public void GoToScreen<T>(IScreen currentScreen, object[] args) where T : IScreen
        {
            IScreen nextScreen = FindScreenOfType<T>();
            GoToScreen(currentScreen, nextScreen, args);
        }

        private void GoToScreen(IScreen currentScreen, IScreen nextScreen, object[] args = null)
        {
            if (nextScreen != null)
            {
                if (currentScreen != null)
                {
                    _screenStack.Push(currentScreen);
                    currentScreen.Hide();
                }
                nextScreen.Show(args);
            }
        }

        public void GoToScreenAndReset<T>(IScreen currentScreen) where T : IScreen
        {
            GoToScreen<T>(currentScreen);
            _screenStack.Clear();
        }

        public void GoBackScreen(IScreen currentScreen)
        {
            if (_screenStack.Count > 0)
            {
                currentScreen.Hide();

                IScreen previousScreen = _screenStack.Pop();
                previousScreen.Return();
            }
        }

        public void GoFirstScreen(IScreen currentScreen)
        {
            GoToScreen(currentScreen, _firstAbstractScreen);
        }

        public void GoFirstScreenAndReset(IScreen currentScreen)
        {
            GoToScreen(currentScreen, _firstAbstractScreen);
            _screenStack.Clear();
        }

        private IScreen FindScreenOfType<T>() where T : IScreen
        {
            IScreen result = null;

            foreach (IScreen screen in _createdScreenList)
            {
                if (screen is T)
                {
                    result = screen;
                    break;
                }
            }

            if (result == null)
            {
                for (int i = 0; i < Screens.Length; i++)
                {
                    if (Screens[i] is T)
                    {
                        if (Screens[i].gameObject.scene == null)
                        {
                            GameObject go = Instantiate(Screens[i].gameObject);
                            result = go.GetComponent<T>();
                        }
                        else
                        {
                            result = Screens[i].GetComponent<T>();
                        }
                        break;
                    }
                }

                if (result != null)
                {
                    result.Init(this);
                    _createdScreenList.Add(result);
                }
                else
                {
                    Debug.Log("Unable to find screen " + typeof(T));
                }
            }

            return result;
        }

    }
}
