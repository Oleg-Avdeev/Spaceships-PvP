using UnityEngine;
using UnityEngine.UI;

namespace Wazzapps.UI
{
    public class GameScreen : AbstractScreen, IGameScreenReceiver
    {
        private ISpawnData[] _spawnData;
        public Transform SpawnButtons;
        public Text EnergyText;

        public override void Show(object[] args)
        {
            base.Show(args);
            OnDataChanged(null);

            Resolver.Instance.RoomController.SetOnDataChangedReceiver(this);
        }

        public override void Hide()
        {
            base.Hide();
        }

        public void ButtonBack()
        {
            OnBackPressed();
        }

        protected override void OnBackPressed()
        {
            Application.Quit();
        }

        public void LateUpdate()
        {   
            EnergyText.text = Resolver.Instance.RoomController.GetCurrentEnergy().ToString();
        }

        public void OnDataChanged(ISpawnData[] data)
        {
            _spawnData = data;
            if(_spawnData == null)
            {
                foreach(Transform t in SpawnButtons)
                {
                    t.gameObject.SetActive(false);
                }
            } else {
                int i = 0;
                foreach(Transform t in SpawnButtons)
                {
                    t.gameObject.SetActive(_spawnData.Length > i);
                    t.GetComponent<SpawnButtonController>().SetTimer(data[i].GetTimeToAllowSpawn(), data[i].GetUnitInfo().GetSpawnTime());
                    i++;
                }
            }
        }

        public void OnSpawnUnitClick(int id, int shipLane)
        {
            Resolver.Instance.RoomController.TrySpawnUnit(id, shipLane);
        }

        public void OnShowInfoClick()
        {
            Resolver.Instance.RoomController.ToggleShowTip();
        }
    }
}
