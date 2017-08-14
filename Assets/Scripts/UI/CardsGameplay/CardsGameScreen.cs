using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Wazzapps.UI
{
    public class CardsGameScreen : AbstractScreen, IGameScreenReceiver
    {
        private ISpawnData[] _spawnData;
        public Transform SpawnButtons;
        public Text EnergyText;

        private SpawnCard[] _spawnCards;
        private double _endTime;
        private bool _inited;

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
                _spawnCards = null;
                foreach(Transform t in SpawnButtons)
                {
                    t.gameObject.SetActive(false);
                }
            } else {
                int i = 0;

                if (_spawnCards == null) _spawnCards = new SpawnCard[SpawnButtons.childCount];
                else return;
                foreach(Transform t in SpawnButtons)
                {
                    t.gameObject.SetActive(_spawnData.Length > i);
                    _spawnCards[i] = t.GetComponent<SpawnCard>();
                    i++;
                }

                FillCards();
            }
        }

        private void FillCards()
        {
            for (int i = 0; i < _spawnCards.Length; i++)
            {
                int randomIndex = Random.Range(0,_spawnData.Length);
                _spawnCards[i].SetValues(_spawnData[randomIndex].GetUnitInfo());
            }

            _endTime = Time.time + 3;
            StartCoroutine(RefillCards());
        }

        IEnumerator RefillCards()
	    {
            if (_spawnCards == null) yield break;
            
	    	while(Time.time < _endTime)
	    		yield return new WaitForSeconds(0.04f);
	    	
            for (int i = 0; i < _spawnCards.Length; i++)
                if (_spawnCards[i].IsEmpty)
                {
                    int randomIndex = Random.Range(0,_spawnData.Length);
                    _spawnCards[i].SetValues(_spawnData[randomIndex].GetUnitInfo());
                    break;
                }

            _endTime = Time.time + 3;
            StartCoroutine(RefillCards());
	    } 

        public bool OnSpawnUnitClick(int id, int shipLane)
        {
            return Resolver.Instance.RoomController.TrySpawnUnit(id, shipLane);
        }
        public void OnShowInfoClick()
        {
            Resolver.Instance.RoomController.ToggleShowTip();
        }
    }
}
