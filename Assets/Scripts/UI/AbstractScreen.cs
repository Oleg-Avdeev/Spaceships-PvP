using UnityEngine;

namespace Wazzapps.UI
{
    public abstract class AbstractScreen : MonoBehaviour, IScreen
    {
		protected IScreenController _controller;

        public virtual void Init(IScreenController controller) {
			_controller = controller;
		}

        public virtual void Show(object[] args = null) {
			gameObject.SetActive(true);
		}

        public virtual void Return() {
			gameObject.SetActive(true);
		}

        public virtual void Hide() {
			gameObject.SetActive(false);
		}

		protected abstract void OnBackPressed();

		private void Update() {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				OnBackPressed();
			}
		}

    }
}
