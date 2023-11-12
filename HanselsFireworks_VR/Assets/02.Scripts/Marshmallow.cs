using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marshmallow : MonoBehaviour
{
    public float loadTime;
    public float height;

    [SerializeField] private float timer;
    [SerializeField] private float realTimer;
    // [SerializeField] private float timer;
    // [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        realTimer = 0;
        StartCoroutine(Ascend());
    }

    // Update is called once per frame
    void Update()
    {
        // realTimer += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        realTimer += Time.fixedDeltaTime;
    }


    IEnumerator Ascend()
    {
        timer = 0;
        float fakeLoadingDuration = 1f / loadTime;
        // 높이는??
        float ratioAscend = 0;
        while (true)
        {
            timer += Time.deltaTime * fakeLoadingDuration;
            // transform.position += new Vector3(0, Time.deltaTime * fakeLoadingDuration, 0);
            // transform.localScale += new Vector3(0, Time.deltaTime * fakeLoadingDuration, 0);
            float value = Mathf.Lerp(0f, height, timer);
            transform.position = new Vector3(0, value, 0);
            transform.localScale = new Vector3(2, value, 2);

            if (value >= height)
            {

                yield break;
            }
            yield return null;
        }
    }
}
