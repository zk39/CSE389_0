﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterLove.StateMachine;

public class Spikes : MonoBehaviour {
	
	[Header ("Damage")]
	public int Damage = 1;
	public bool DamageActive = true;
	public float DamageActiveTime = 0.15f;
	private float DamageActiveTimer = 0f;

	[Header ("Idle State")]
	public float IdleTime = 2f;
	private float IdleTimeTimer = 0f; 

	[Header ("Up State")]
	public float UpTime = 0.5f;
	private float UpTimeTimer = 0f;

	[Header ("Down State")]
	public float DownTime = 0.5f;
	private float DownTimeTimer = 0f;

	public List<Health> healthsDamaged = new List<Health>();

	public Animator animator;

	// States for the state machine
	public enum States {
		Idle, 
		Up,
		Down
	}

	// State Machine
	public StateMachine<States> fsm;

	void Awake () {
		fsm = StateMachine<States>.Initialize(this);
	}

	// Use this for initialization
	void Start () {
		fsm.ChangeState(States.Idle);
	}

	void Update () {
		UpdateSprite ();
	}

	void Idle_Enter () {
		IdleTimeTimer = IdleTime;
	}

	void Idle_Update () {
		if (IdleTimeTimer > 0f) {
			IdleTimeTimer -= Time.deltaTime;

			if (IdleTimeTimer <= 0f) {
				fsm.ChangeState (States.Up);
			}
		}	
	}

	void Up_Enter () {
		DamageActive = true;
		DamageActiveTimer = DamageActiveTime;
		UpTimeTimer = UpTime;
	}

	void Up_Update () {
		if (DamageActiveTimer > 0f) {
			DamageActiveTimer -= Time.deltaTime;

			if (DamageActiveTimer <= 0f) {
				DamageActive = false;
			}
		}

		if (UpTimeTimer > 0f) {
			UpTimeTimer -= Time.deltaTime;

			if (UpTimeTimer <= 0f) {
				fsm.ChangeState (States.Down);
			}
		}
	}

	void Up_Exit () {
		DamageActive = false;
		healthsDamaged.Clear ();
	}

	void Down_Enter () {
		DownTimeTimer = DownTime;
	}

	void Down_Update () {
		if (DownTimeTimer > 0f) {
			DownTimeTimer -= Time.deltaTime;

			if (DownTimeTimer <= 0f) {
				fsm.ChangeState (States.Idle);
			}
		}
	}

	protected void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Entity") && DamageActive) {
			var healthcomponent = other.GetComponent<Health> ();
			if (healthcomponent != null) {
				OnEntityEnter (healthcomponent);
			}
		}
	}

	protected void OnTriggerStay2D (Collider2D other) {
		if (other.CompareTag ("Entity") && DamageActive) {
			var healthcomponent = other.GetComponent<Health> ();
			if (healthcomponent != null) {
				OnEntityEnter (healthcomponent);
			}
		}
	}

	protected virtual void OnEntityEnter (Health health) {
		if (healthsDamaged.Contains (health))
			return;

		// camera shake
		if (CameraShaker.instance != null) {
			CameraShaker.instance.InitShake(0.2f, 1f);
		}
		
		var damaged = health.TakeDamage (Damage);
		if (damaged) {
			healthsDamaged.Add (health);
		}
		
	}

	void UpdateSprite () {
		if (fsm.State == States.Idle) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Idle")) {
				animator.Play ("Idle");
			}
		} else if (fsm.State == States.Up) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Up")) {
				animator.Play ("Up");
			}
		} else if (fsm.State == States.Down) {
			if (!animator.GetCurrentAnimatorStateInfo (0).IsName ("Down")) {
				animator.Play ("Down");
			}
		}
	}
}
