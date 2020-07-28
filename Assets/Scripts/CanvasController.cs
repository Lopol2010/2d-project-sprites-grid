using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{

    [SerializeField]
    private List<Sprite> TimeOfDaySprites = new List<Sprite>();
    [SerializeField]
    private Image TimeOfDayImage;

    private int _timeOfDay;
    public int TimeOfDay
    {
        get
        {
            return _timeOfDay;
        }
        set
        {
            _timeOfDay = value;
            TimeOfDayImage.sprite = TimeOfDaySprites[value];
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

}
