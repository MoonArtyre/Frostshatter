using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private MusicManager.MusicType type;
    [SerializeField] private Vector2 triggerArea;
     private Transform triggerTracker;
    bool entered;

    [SerializeField] bool debug;
    void Start()
    {
        triggerTracker = ReferenceManager.Instance.playerRef;
    }

    void Update()
    {
        if(debug)
            Debug.Log(Vector3.Distance(transform.position, triggerTracker.position) + " < " + triggerArea.y);
        if(Vector3.Distance(transform.position, triggerTracker.position) < triggerArea.y)
        {
            entered = true;

            var distancetoMaxStrength = Vector3.Distance(transform.position, triggerTracker.position) - triggerArea.x;
            var radius = triggerArea.y - triggerArea.x;
            var strength = 1 - distancetoMaxStrength / radius;
            strength *= 10;
            MusicManager.Instance.PlayMusic(type, strength);
        }
        else
        {
            if (entered == true)
            {
                entered = false;
                MusicManager.Instance.StopMusic(type);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,triggerArea.x);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, triggerArea.y);

    }
}
