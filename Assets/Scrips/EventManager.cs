using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Int2Event : UnityEvent<int, int>
{

}
public class EventManager : MonoBehaviour
{


    #region singleton
    public static EventManager current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        } else if (current != null)
        {
            Destroy(this);
        }
    }

    #endregion


    public Int2Event updateBulletsEvents = new Int2Event();
    public UnityEvent newGunEvent = new UnityEvent();
   
   

}
