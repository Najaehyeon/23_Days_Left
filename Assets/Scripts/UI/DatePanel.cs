using TMPro;
using UnityEngine;

public class DatePanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dateText;

    public void SetDate(int day)
    {
        dateText.text = day.ToString();
    }
}
