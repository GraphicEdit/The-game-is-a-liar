using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AController : MonoBehaviour
{
	public int startingHealth = 100;          // The amount of health the enemy starts the game with.
	public int currentHealth;                    // The current health the enemy has.
	public float sinkSpeed = 2.5f;           // The speed at which the enemy sinks through the floor when dead.
	public int scoreValue = 10;               // The amount added to the player's score when the enemy dies.
	public AudioClip deathClip;              // The sound to play when the enemy dies.

	Animator anim;                              // Reference to the animator.
	public AudioSource audioSource;              // Reference to the audio source.
	public ParticleSystem hitParticles;             // Reference to the particle system that plays when the enemy is damaged.
	public CapsuleCollider capsuleCollider;     // Reference to the capsule collider.
	bool isDead;                                  // Whether the enemy is dead.
	bool isSinking;
	public RectTransform HealthBar;
	protected float healthBarWidth;
	public Animator animator;

	void Awake()
	{
		// Setting up the references.
		//anim = GetComponent<Animator>();
		audioSource = GetComponent<AudioSource>();
		hitParticles = GetComponentInChildren<ParticleSystem>();
		healthBarWidth = HealthBar.rect.width;
		// Setting the current health when the enemy first spawns.
		currentHealth = startingHealth;

		// If the enemy should be sinking...
		if (isSinking)
		{
			// ... move the enemy down by the sinkSpeed per second.
			transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
		}


		

	}


	public void TakeDamage(int amount, Vector3 hitPoint)
	{
		// If the enemy is dead...
		if (isDead)
			// ... no need to take damage so exit the function.
			return;

		// Play the hurt sound effect.
		audioSource.Play();

		// Reduce the current health by the amount of damage sustained.
		currentHealth -= amount;

		// Set the position of the particle system to where the hit was sustained.
		hitParticles.transform.position = hitPoint;

		// And play the particles.
		hitParticles.Play();

		var width = HealthBar.rect.width;

		HealthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width * currentHealth / startingHealth);

		// If the current health is less than or equal to zero...
		if (currentHealth <= 0)
		{
			// ... the enemy is dead.
			Death();
		}
	}

	public virtual void Death()
	{
		// The enemy is dead.
		isDead = true;

		// Turn the collider into a trigger so shots can pass through it.
		capsuleCollider.isTrigger = true;

		// Tell the animator that the enemy is dead.
		//anim.SetTrigger("Dead");

		// Change the audio clip of the audio source to the death clip and play it (this will stop the hurt clip playing).
		audioSource.clip = deathClip;
		audioSource.Play();

		StartSinking();
	}

	public void StartSinking()
	{
		// Find and disable the Nav Mesh Agent.
		GetComponent<NavMeshAgent>().enabled = false;

		// Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
		GetComponentInChildren<Rigidbody>().isKinematic = true;

		// The enemy should no sink.
		isSinking = true;

	}
}
