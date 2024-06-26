using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum PatrolSide
{
    Left,
    Right
}

public class Drone : MonsterObject
{
    SpriteRenderer perceptionRangeRender;

    public bool recognizeComplete;

    Color originalColor;

    [SerializeField]
    Color recognizeColor;

    [SerializeField]
    float recognizeDuration = 1f;

    [SerializeField]
    PatrolSide side = PatrolSide.Right;

    [SerializeField]
    float patrolSpeed = 2f;

    Vector3 m_MoveDir = Vector3.zero;

    [SerializeField]
    private float PatrolTime = 1f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        perceptionRangeRender = transform.Find("PerceptionRange").gameObject.GetComponent<SpriteRenderer>();
        originalColor = perceptionRangeRender.color;

        StartCoroutine(Patrol());
    }

    protected override void Found()
    {
        base.Found();

        StartCoroutine(RecognizePlayer());
    }

    private void FixedUpdate()
    {
            transform.position += m_MoveDir * patrolSpeed * Time.deltaTime;
    }

    IEnumerator RecognizePlayer()
    {
        float time = 0f;
        while (perceptRange.GetPerception() && time < recognizeDuration)
        {
            Color lerpedColor = Color.Lerp(originalColor, recognizeColor, time/recognizeDuration);
            perceptionRangeRender.color = lerpedColor;
            yield return null;

            time += Time.deltaTime;
        }

        if (time >= recognizeDuration)
        {
            //targetObj.GetComponent<PlayerComponentCtrl>().StopAllComponent();
            targetObj.GetComponent<Player>().Die();
        }
        else
        {
            perceptionRangeRender.color = originalColor;
            StartCoroutine(WaitRecognize());
            StartCoroutine(Patrol());
        }
    }

    private IEnumerator Patrol()
    {
        while(true)
        {
            m_MoveDir = Vector3.right;
            transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(PatrolTime);

            m_MoveDir = -Vector3.right;
            transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);

            yield return new WaitForSeconds(PatrolTime);
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
