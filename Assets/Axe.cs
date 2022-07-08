using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Tool
{
    public int damageToTree;
    CharacterMovement playerCharacter;
    Animator animator;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        playerCharacter = Util.Player.GetComponent<CharacterMovement>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        print("Axe updated");
        animator.SetFloat("Speed", Mathf.Max(Mathf.Abs(Input.GetAxis("Horizontal")), Mathf.Abs(Input.GetAxis("Vertical"))));
        animator.SetBool("Using", onCooldown);
    }

    public override void Use()
    {
        //animator.SetBool("Use", true);
    }

    public void Check() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        foreach(var item in Physics.RaycastAll(ray, 1.7f)) {
            if(item.collider == null) continue;
            print(item.collider.name);
            if(item.collider.CompareTag("Tree")) {
                item.collider.GetComponentInParent<StopBeingAmbiguous.Tree>().TakeDamage(damageToTree);
                break;
            }
        }
        //GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Impact/treehit" + Random.Range(1, 10).ToString()));
        //animator.SetBool("Use", false);
    }
}
