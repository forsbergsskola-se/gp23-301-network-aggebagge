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
            
            if (unitSo.attribute != null)
            {
                uniqueImage.gameObject.SetActive(true);
                uniqueImage.sprite = unitSo.attribute.icon;
            }
            
            if(unitSo.damage > 0)
                damage.text = unitSo.damage.ToString();
            if(unitSo.goldGain > 0)
                gold.text = unitSo.goldGain.ToString();
        }
    }
}