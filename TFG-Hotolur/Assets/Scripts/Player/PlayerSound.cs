﻿using UnityEngine;

/// <summary>
/// Script to manage the sounds of the player
/// </summary>
public class PlayerSound : MonoBehaviour {

    // Sounds of when the player does some actions
    //[HideInInspector]
    //public AudioClip audioJump;
    //[HideInInspector]
    //public AudioClip audioHitReaction;
    //[HideInInspector]
    //public AudioClip audioDeath;

    //private AudioSource audioSource;

    //private PlayerMovement playerMovement;
    //private PlayerHealth playerHealth;

    //private void Awake()
    //{
    //    audioSource = GetComponent<AudioSource>();

    //    playerMovement = GetComponent<PlayerMovement>();
    //    playerHealth = GetComponent<PlayerHealth>();
    //}

    //private void Update()
    //{
    //    // Control the sound when the player is jumping or not
    //    if (playerMovement.isJumping && !audioSource.isPlaying)
    //    {
    //        audioSource.clip = audioJump;

    //        playerMovement.isJumping = false;
    //    }

    //    // Control the sound when the player is hurt or not
    //    if (playerHealth.isHurt && !playerHealth.isDead && !audioSources[1].isPlaying)
    //    {
    //        audioSources[1].clip = audioHitReaction;
    //        audioSources[1].Play();

    //        playerHealth.isHurt = false;
    //    }

    //    // Control the sound when the player die
    //    if(playerHealth.isDead && !audioSources[1].isPlaying)
    //    {
    //        audioSources[1].clip = audioDeath;
    //        audioSources[1].Play();

    //        playerHealth.isDead = false;
    //    }
    //}

}
