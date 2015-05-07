using UnityEngine;
using System.Collections;
/* ----------------------------------------
 * class to demonstrate how to trigger and apply and Explosion force to 
 * a character featuring Ragdoll physics
 */ 
public class Landmine : MonoBehaviour {
	// Float variable for the explosion's radius
	public float range = 2f;
	public GameObject explosion;
	// Float variable for the explosion's force
	public float force = 2f;

	// Float variable for the explosion's Upwards modifier
	public float up = 4f;

	// Private bool for enabling/disabling the effects of a collision with the trigger
	private bool isActive = true;

	/* ----------------------------------------
	 * If a game object with the'Player' tag enters the Trigger collider, and 'active' is true, 
	 * activate character's ragdoll physics, apply explosion force to it, deactivate trigger and start
	 * coroutine to re-activate it.
	 */
	void  OnTriggerEnter ( Collider collision  ){
		if(collision.gameObject.tag == "Player"  && isActive){
			//IF gameobject that has collided with trigger has 'Player' tag and 'isActive' is true, THEN set 'isActive' as false.
			isActive = false;

			if(collision.gameObject.name=="Magritte")
			{
				// Start coroutine to reactivate trigger
				StartCoroutine(Reactivate());
				// Activate ragdoll physics on character through its RagdollCharacter component 
				collision.gameObject.GetComponent<RagdollCharacter>().ActivateRagdoll();
//				collision.gameObject.GetComponent<MouseAimLookAt>().enabled = false;
			}

			// Create Vector for the explosion's position
			Vector3 explosionPos = transform.position;

			// Get all colliders within explosion radius
       	 	Collider[] colliders = Physics.OverlapSphere(explosionPos, range);
        	
			// For each collider within explosion radius...
			foreach (Collider hit in colliders) {
				if (hit.GetComponent<Rigidbody>())
                	// IF collider object features a rigidbody component, add explosion force to it
					hit.GetComponent<Rigidbody>().AddExplosionForce(force, explosionPos, range, up);
           	}

			Instantiate(explosion,transform.position,Quaternion.identity);
			StartCoroutine(DestroyMine());
        }
		if(collision.gameObject.tag == "Bullet"  && isActive)
		{
			isActive = false;

			// Create Vector for the explosion's position
			Vector3 explosionPos = transform.position;

			// Get all colliders within explosion radius
			Collider[] colliders = Physics.OverlapSphere(explosionPos, range);
			
			// For each collider within explosion radius...
			foreach (Collider hit in colliders) {
				if (hit.GetComponent<Rigidbody>())
					// IF collider object features a rigidbody component, add explosion force to it
					hit.GetComponent<Rigidbody>().AddExplosionForce(force, explosionPos, range, up);
			}
			Instantiate(explosion,transform.position,Quaternion.identity);
			// Start coroutine to reactivate trigger
			StartCoroutine(DestroyMine());
		}
    }
	/* ----------------------------------------
	 * A function to destroy the landmine after two seconds
	 */
	IEnumerator DestroyMine(){
		// Wait for two seconds
		yield return new WaitForSeconds(2);

		Destroy (this.gameObject);
	}
	/* ----------------------------------------
	 * A function to reactivate the trigger after two seconds
	 */
	IEnumerator Reactivate(){
		// Wait for two seconds
		yield return new WaitForSeconds(2);

		// set 'active' as true
		isActive = true;
	}
}
