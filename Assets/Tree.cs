using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StopBeingAmbiguous {
    public class Tree : MonoBehaviour
    {
        public int damage;
        public int maxHealth;

        public void TakeDamage(int amount) {
            damage += amount;
            if(!GetComponent<AudioSource>()) {
                gameObject.AddComponent<AudioSource>();
                GetComponent<AudioSource>().spatialBlend = 1f;
            }
            //GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Sounds/treehit" + Random.Range(1, 10));
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Impact/treehit" + Random.Range(1, 10)));
            if(damage >= maxHealth) {
                Die();
            }
        }

        public void Die() {
            GetComponent<Animator>().SetTrigger("Fall");
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Impact/finalhit"));
            GetComponent<AudioSource>().pitch = 0.6f;
            GetComponent<AudioSource>().PlayOneShot(Resources.Load<AudioClip>("Sounds/Impact/woodcreak"));
            GetComponent<AudioSource>().pitch = 1f;
        }

        public void Destroy() {
            if(TreeSpawner.treePositions.Contains(transform.position)) TreeSpawner.treePositions.Remove(transform.position);
            Instantiate(Resources.Load<GameObject>("Sounds/GameObjects/TreeFall"), transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
