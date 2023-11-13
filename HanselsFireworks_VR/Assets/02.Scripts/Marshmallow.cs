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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            timer -= 0.01f;
        }
    }

    private void FixedUpdate()
    {
        realTimer += Time.fixedDeltaTime;
    }

    public void Descend()
    {
        // PlaySound(audioClipHurt);
        Debug.Log("Marshmallow Damaged");
        if (timer >= 0) timer -= 0.01f;
        else timer = 0;
    }
    

    IEnumerator Ascend()
    {
        timer = 0;
        float fakeLoadingDuration = 1f / loadTime;
        // 높이는??
        while (true)
        {
            timer += Time.deltaTime * fakeLoadingDuration;
            // transform.position += new Vector3(0, Time.deltaTime * fakeLoadingDuration, 0);
            // transform.localScale += new Vector3(0, Time.deltaTime * fakeLoadingDuration, 0);
            float value = Mathf.Lerp(0f, height, timer);
            transform.position = new Vector3(0, value, 0);
            transform.localScale = new Vector3(2, value, 2);

            if (timer >= 1)
            {

                yield break;
            }
            yield return null;
        }
    }
}
