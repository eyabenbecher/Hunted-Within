using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye_Blink_Script : MonoBehaviour
{
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private float blinkCount=0f;
    private float blinkSpeed = 7f;
    private float CountWait=0f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
     skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
     animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CountWait += 10f;



        //Blink
        if (CountWait > 400f)
        {
            CountWait = 0f;
            //　0～100の間のウエイト値を生成
            blinkCount = Mathf.Clamp01(Mathf.Sin(Time.time * blinkSpeed)) * 100f;
            skinnedMeshRenderer.SetBlendShapeWeight(16, blinkCount);
        }
        else
        {
            skinnedMeshRenderer.SetBlendShapeWeight(16, 0f);
        }

    }
}
