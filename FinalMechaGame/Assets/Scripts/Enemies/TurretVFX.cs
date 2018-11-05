using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class TurretVFX : MonoBehaviour{
    public Animator animator;
    public List<ParticleSystem> particles;
    public float movementMultiplyer;

    private void Start() {
        animator.SetTrigger("start");
        Quiet();
    }

    //Funciones
    public void OnMovement(float velocity) {
        animator.SetFloat("MOVING", velocity * movementMultiplyer);
        //Debug.Log("called"); 
    }
    public void OnDamage() {
        StartCoroutine(Damage());
    }
    public void Dying() {
        animator.SetTrigger("DYING");
        Instantiate(particles [ 2 ],transform.position,Quaternion.identity,transform);
        Instantiate(particles [ 1 ], transform.position, Quaternion.identity, transform);
        Instantiate(particles [ 3 ], transform.position, Quaternion.identity, transform);
        //print("die");

    }
    public void Rotate() {
        animator.SetTrigger("ITERATE");
    }
    public void Shooting() {

    }
    public void Quiet() {
        animator.SetBool("QUIET", true);
    }
    public void ClearAnimations() {
        animator.SetBool("QUIET", false);
        animator.SetBool("DYING", false);
    }
    IEnumerator Damage() {
        ParticleSystem pt = Instantiate(particles [ 0 ], transform.position + Vector3.up, Quaternion.identity, transform);
        yield return new WaitForSeconds(3);
        Destroy(pt.gameObject);
    }
}
