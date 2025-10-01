using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMouvement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;
    public Animator anim;
    public AnimatorOverrideController MoineController;
    public AnimatorOverrideController ArcherController;
    public SpriteRenderer sr;

    private RuntimeAnimatorController baseController;
    [SerializeField] Pnj Pnj;

    [SerializeField] PlayerStats PlayerStats;

    void Start()
    {
        baseController = anim.runtimeAnimatorController;
    }

    void FixedUpdate()
    {
        if (Pnj.isPlay == false)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            anim.SetFloat("horizontal", Mathf.Abs(horizontal));
            anim.SetFloat("vertical", Mathf.Abs(vertical));

            if (horizontal < 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }

            rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
        }
        else
        {
            rb.linearVelocity = new Vector2(0, 0);
            anim.SetBool("Dialog", true);
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
        }

        if (PlayerStats.PlayerClass == "Archer")
        {
            anim.runtimeAnimatorController = ArcherController;
        }
        else if (PlayerStats.PlayerClass == "Moine")
        {
            anim.runtimeAnimatorController = MoineController;
        }
        else if (PlayerStats.PlayerClass == "Warrior")
        {
            anim.runtimeAnimatorController = baseController;
        }



    }
}
