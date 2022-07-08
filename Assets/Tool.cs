using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tool : Item
{
    public bool onCooldown;
    public float cooldownTime;
    public bool hasCooldown;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        print("Tool updated");
        if(Input.GetButtonDown("Fire1") && !onCooldown) {
            StartCoroutine(Cooldown());
        }
    }

    IEnumerator Cooldown() {
        Use();
        if(hasCooldown) {
            onCooldown = true;
            yield return new WaitForSeconds(cooldownTime);
            onCooldown = false;
        }
    }

    public abstract void Use();

    public void WalkSound() {
        var audioSource = GetComponent<AudioSource>();
        if(audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        //audioSource.clip = Resources.Load<AudioClip>("Sounds/Footstep/grassstep" + Random.Range(1, 7).ToString());
        List<AudioClip> grassSteps = new List<AudioClip>();
        foreach(var item in Resources.LoadAll<AudioClip>("Sounds/Footstep")) {
            if(item.name.Contains("grassstep1")) continue;
            if(item.name.Contains("grassstep")) grassSteps.Add(item);
        }
        if(grassSteps.Contains(audioSource.clip)) grassSteps.Remove(audioSource.clip);
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.volume = 0.75f;
        audioSource.clip = grassSteps[Random.Range(0, grassSteps.Count)];
        audioSource.PlayOneShot(audioSource.clip);
    }
}
