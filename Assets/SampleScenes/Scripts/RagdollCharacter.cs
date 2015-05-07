using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.Cameras;
/* ----------------------------------------
 * class to demonstrate how to apply Ragdoll physics 
 * to a character previously set up with Ragdoll Wizard
 */ 
public class RagdollCharacter : MonoBehaviour {	

	public GameObject gunObj;
	public Transform leftHandT;
	public Transform rightHandT;
	public Transform leftFootT;
	public Transform rightFootT;
	public Transform motorcycleSitspot;

	private Vector3 gunStoredLocalPosition;
	private Quaternion gunStoredLocalRotation;

	/* ----------------------------------------
	 * At Start, deactivate all components that allow for ragdoll physics. 
	 * Also starting a coroutine that restores the character after 5 seconds
	 */
	void Start () {
		// Call DeactivateRagdoll function 
	    DeactivateRagdoll();
    }

	/* ----------------------------------------
	 * A function to activate all components that allow for ragdoll physics
	 */
    public void ActivateRagdoll(){

		// store local transform data
		gunStoredLocalPosition = gunObj.transform.localPosition;
		gunStoredLocalRotation = gunObj.transform.localRotation;
		// unparent gun
		gunObj.transform.parent = null;
		// add collider and rigidbody
		gunObj.AddComponent<Rigidbody>();
		gunObj.AddComponent<BoxCollider>();
		gunObj.GetComponent<Rigidbody>().drag = 0.02f;

		// Disable Character Controller component
		gameObject.GetComponent<AICharacterControl> ().enabled = false;

		//  Disable character's Basic Controller component (a C# script that controls Mecanim system)
		gameObject.GetComponent<ThirdPersonCharacter> ().enabled = false;

		//  Disable Animator component 
		//gameObject.GetComponent<Animator> ().enabled = false;
		gameObject.GetComponent<Animator> ().SetBool ("OnBike", true);
		gameObject.GetComponent<NavMeshAgent> ().enabled = false;
		// Find every Rigidbody in character's hierarchy
		foreach (Rigidbody bone in GetComponentsInChildren<Rigidbody>()) {
				// Set bone's rigidbody component as not kinematic
				bone.isKinematic = false;
				
				//Enable collision detection for rigidbody component 
				bone.detectCollisions = true;
		}
		GetComponent<Rigidbody>().isKinematic = true;
		GetComponent<Rigidbody>().detectCollisions = false;
		// Find every Collider in character's hierarchy
		foreach (Collider col in GetComponentsInChildren<Collider>()) {
				// Enable Collider
				col.enabled = true;
		}
		GetComponent<Collider>().enabled = false;


		transform.parent = motorcycleSitspot;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		// build appropriate physics links (arms, spine)


		//gameObject.AddComponent<FixedJoint>();
		//GetComponent<FixedJoint>().connectedBody = GetComponent<AICharacterControl>().target.GetComponent<Rigidbody>();

		tag = "Untagged";
		GetComponent<AICharacterControl> ().target.tag = "Player";
		if(Camera.main.name!="CCTV Camera")
			Camera.main.transform.parent.parent.SendMessage ("FindAndTargetPlayer");
		//Camera.main.GetComponentInParent<AutoCam> ().FindAndTargetPlayer ();
		// Start coroutine to restore character
		//StartCoroutine (Restore ());

    }

	/* ----------------------------------------
	 * A function to deactivate all components that allow for ragdoll physics
	 */
	public void DeactivateRagdoll(){
		// Enable Character Controller component
		gameObject.GetComponent<ThirdPersonCharacter>().enabled = true;

		//  Enable Animator component 
		//gameObject.GetComponent<Animator>().enabled = true;
		gameObject.GetComponent<Animator> ().SetBool ("OnBike", false);

		gameObject.GetComponent<NavMeshAgent> ().enabled = true;
		// Position character at Spawnpoint gameobject's position
//		transform.position = GameObject.Find("Spawnpoint").transform.position;

		// Rotate character according to Spawnpoint gameobject's rotation
//		transform.rotation = GameObject.Find("Spawnpoint").transform.rotation;

		// Find every Rigidbody in character's hierarchy
		foreach(Rigidbody bone in GetComponentsInChildren<Rigidbody>()){
			// Set bone's rigidbody component as  kinematic
			bone.isKinematic = true;
			// Disable collision detection for rigidbody component 
			bone.detectCollisions = false;
	    }
		GetComponent<Rigidbody>().isKinematic = false;
		GetComponent<Rigidbody>().detectCollisions = true;
		// Find every Collider in character's hierarchy
		foreach(Collider col in GetComponentsInChildren<Collider>()){
			// Disable Collider
			col.enabled = false;
		}
		GetComponent<Collider>().enabled = true;
		//  Enable character's Basic Controller component (a C# script that controls Mecanim system)
//		gameObject.GetComponent<CharacterController>().enabled = true;
//		gameObject.GetComponent<MouseAimLookAt>().enabled = true;

		transform.parent = null;
		tag = "Player";
		GetComponent<AICharacterControl> ().target.tag = "Untagged";
		
		Camera.main.GetComponentInParent<AutoCam> ().FindAndTargetPlayer ();

		if(gameObject.GetComponent<FixedJoint>())
			Destroy (gameObject.GetComponent<FixedJoint>() );
    }

	/* ----------------------------------------
	 * A function to restore the character after five seconds
	 */
	IEnumerator Restore(){
		// Wait for five seconds
		yield return new WaitForSeconds(5);

		// Deactivate Ragdoll 
		DeactivateRagdoll();
		Destroy(gunObj.GetComponent<Rigidbody>());
		Destroy(gunObj.GetComponent<Collider>());
		gunObj.transform.parent = leftHandT;
		gunObj.transform.localPosition = gunStoredLocalPosition;
		gunObj.transform.localRotation = gunStoredLocalRotation;

	}
}
