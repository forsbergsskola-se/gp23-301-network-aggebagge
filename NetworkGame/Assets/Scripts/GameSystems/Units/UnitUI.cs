using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Units
{
    public class UnitUI : MonoBehaviour
    {
        [HideInInspector]public UnitSo unit;
        
        public Image unitImage;
        public Image uniqueImage;
        public TextMeshProUGUI damage;
        public TextMeshProUGUI gold;

        public void SetupUI(UnitSo unitSo)
        {
            unit = unitSo;
            
            unitImage.sprite = unitSo.sprite;
            uniqueImage.gameObject.SetActive(unitSo.action != UnitSo.Action.None);

            if(unitSo.physicalDamage > 0)
                damage.text = unitSo.physicalDamage.ToString();
            if(unitSo.goldGain > 0)
                gold.text = unitSo.goldGain.ToString();
        }
    }
}