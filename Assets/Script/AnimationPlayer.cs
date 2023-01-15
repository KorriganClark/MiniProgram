using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationPlayer : MonoBehaviour
{
    
    [SerializeField]
    private SpriteRenderer image;
    [SerializeField]
    public List<Sprite> animationSprites = new List<Sprite>();


    private int AnimationAmount { get { return animationSprites.Count; } }
    public void PlayAnimation()
    {
        if (image == null) image = GetComponent<SpriteRenderer>();
        StartCoroutine(PlayAnimationForwardIEnum());
    }

    private IEnumerator PlayAnimationForwardIEnum()
    {
        int index = 0;//可以用来控制起始播放的动画帧索引
        gameObject.SetActive(true);
        while (true)
        {
            //当我们需要在整个动画播放完之后  重复播放后面的部分 就可以展现我们纯代码播放的自由性
            if (index > AnimationAmount - 1)
            {
                index = 50;
            }
            image.sprite = animationSprites[index];
            index++;
            yield return new WaitForSeconds(0.03f);//等待间隔  控制动画播放速度
        }
    }
}
