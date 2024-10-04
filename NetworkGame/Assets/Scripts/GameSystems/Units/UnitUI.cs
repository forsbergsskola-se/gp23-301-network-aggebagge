using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystems.Units
{
    public class UnitUI : MonoBehaviour
    {
        [HideInInspector]public UnitData data;
        
        public Image unitImage;
        public Image uniqueImage;
        public TextMeshProUGUI damage;
        public TextMeshProUGUI gold;

        public void SetupUI(UnitData unitData)
        {
            data = unitData;
            UnitSo unitSo = UnitManager.GetUnitSo(unitData.id);
            unitImage.sprite = unitSo.sprite;
            
            if (unitData.attributeType != AttributeType.None)
            {
                uniqueImage.gameObject.SetActive(true);
                uniqueImage.sprite = unitSo.attribute.icon;
            }
            
            if(unitData.damage > 0)
                damage.text = unitData.damage.ToString();
            if(unitData.goldGain > 0)
                gold.text = unitData.goldGain.ToString();
        }
    }
}