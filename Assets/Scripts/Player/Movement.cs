using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Fields
    [SerializeField] Transform[] route;
    private int routeToGo = 0;
    private float tParam = 0;
    [SerializeField] private float speed;
    [SerializeField] private float autoTurnSpeed;
    [SerializeField] private float horizontalSpeed;
    private bool coroutineAllowed;
    #endregion

    private void Start()
    {
        coroutineAllowed = true;
    }

    void Update()
    {
        transform.Translate(HorizontalMovement());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (coroutineAllowed && other.GetComponent<TurnShard>())
        {
            StartCoroutine(AutoTurn());
        }
    }
    private IEnumerator AutoTurn()
    {
        coroutineAllowed = false;

        Vector3 p0 = route[0].GetChild(0).position;
        Vector3 p1 = route[0].GetChild(1).position;
        Vector3 p2 = route[0].GetChild(2).position;
        Vector3 p3 = route[0].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * autoTurnSpeed;

            transform.SetPositionAndRotation(Bezier.GetPoint(p0, p1, p2, p3, tParam), 
                Quaternion.LookRotation(Bezier.GetFirstDerivative(p0, p1, p2, p3, tParam)));

            yield return new WaitForEndOfFrame();
        }

        tParam = 0;
        routeToGo += 1;
        if (routeToGo > route.Length - 1)
        {
            routeToGo = 0;
        }
        coroutineAllowed = true;
    }
    private Vector3 HorizontalMovement()
    {
        Vector3 pos = new (Input.GetAxis("Horizontal") * horizontalSpeed * Time.deltaTime, 0, speed * Time.deltaTime);
        return pos;
    }
}