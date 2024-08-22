using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour, IData
{
 [SerializeField] private float _speed = 5f;
 [SerializeField] private Rigidbody2D _rigi = null;

 [SerializeField] private LayerMask JumpLayer = new();
 [SerializeField] private Transform groundCheck = null;
 private bool isGrounded = false;
 [SerializeField] private float JumpForce = 5f;
 private float jumpTimeCounter;
 [SerializeField] private float jumpTime; 
 [SerializeField] private Animator playerAnim = null;
 [SerializeField] private float DefaultGravity = 1f, SkatingGravity = 0.1f;
 [SerializeField] private GameObject skateBoard = null;
 private bool CanUseSkate = true;
 bool usedSkate = false;

 [SerializeField] private float MushroomForce = 10f;
 [SerializeField] private TMP_Text ScoreText = null;
 float _Score = 0f;
 [SerializeField] private BoxCollider2D playerCollider = null;
 [SerializeField] private ParticleSystem JumpParticles = null;
 [SerializeField] private Animation SkateBoardAnim = null;
 int high_score = 0;

 public void LoadData(GameData data)
 {
   high_score = data.highScore;
 }

 public void SaveData(ref GameData data)
 {
   if ((int)_Score > high_score)
   {
      high_score = (int)_Score;
      data.highScore = (int)_Score;
   }
 }

 private void Start()
 {
   InvokeRepeating(nameof(SpeedUpGame), 3f, 3f);
   ScoreText.text = "0";
 }

 void SpeedUpGame()
 {
   Time.timeScale = Math.Min(Time.timeScale + 0.01f, 1.8f);
 }

 private void Update()
 {
      Collider2D collider = Physics2D.OverlapCircle(groundCheck.position, 0.1f, JumpLayer);
      isGrounded = collider != null;

    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (_rigi.velocity.y <= 0f)
      {
         if (isGrounded)
         {
            if (_rigi.velocity.y <= JumpForce)
            {
               jumpTimeCounter = jumpTime;
               _rigi.velocity = new Vector2(_rigi.velocity.x, JumpForce);
               JumpParticles.Emit(12);
            }

            CanUseSkate = false;
         }
      }
    }

    if (_rigi.velocity.y <= 0 && isGrounded)
    {
      CanUseSkate = false;
    }
    if (!CanUseSkate && !isGrounded)
    {
      if (!Input.GetKey(KeyCode.Space))
      {
         CanUseSkate = true;
         usedSkate = false;
      }
    }

    if (isGrounded)
    {
      playerAnim.SetInteger("AnimState", 0);
      _rigi.gravityScale = DefaultGravity;
      skateBoard.SetActive(false);
    }
    else
    {
      if (Input.GetKey(KeyCode.Space))
      {
         if (CanUseSkate)
         {
            if (!usedSkate)
            {
                _rigi.velocity = new Vector2(_rigi.velocity.x, 0);
                
                usedSkate = true;
            }
         _rigi.gravityScale = SkatingGravity;
         playerAnim.SetInteger("AnimState", 2);
         if (!skateBoard.activeInHierarchy)
         {
            SkateBoardAnim.Stop();
            SkateBoardAnim.Play();
         }

         skateBoard.SetActive(true);
         }
         else if (jumpTimeCounter > 0 && _rigi.velocity.y > 0f && _rigi.velocity.y <= JumpForce)
         {
            _rigi.velocity = new Vector2(_rigi.velocity.x, JumpForce);
            jumpTimeCounter -= Time.deltaTime;
            playerAnim.SetInteger("AnimState", 1);
            _rigi.gravityScale = DefaultGravity;
         }
         else
         {
            _rigi.gravityScale = DefaultGravity;
            playerAnim.SetInteger("AnimState", 1);
            skateBoard.SetActive(false);
         }

      }
      else
      {
         _rigi.gravityScale = DefaultGravity;
         playerAnim.SetInteger("AnimState", 1);
         skateBoard.SetActive(false);
      }
    }


    transform.position += new Vector3(_speed * Time.deltaTime, 0, 0);
    _Score += _speed * Time.deltaTime * 10f;
    ScoreText.text = ((int)_Score).ToString();
 }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("mushroom"))
      {
         _rigi.velocity = new Vector2(_rigi.velocity.x, MushroomForce);
         jumpTimeCounter = 0f;
         CanUseSkate = false;
         usedSkate = false;
         collision.GetComponent<MushroomScript>().MushroomCollided();
      }
   }

   private void OnCollisionEnter2D(Collision2D collision)
   {
      if (collision.gameObject.CompareTag("platform"))
      {
         float PLayerFeetY = transform.position.y + (playerCollider.offset.y * 0.35f) - ((playerCollider.size.y / 2f) * 0.35f) - 0.1f;
         BoxCollider2D platformCollider = collision.gameObject.GetComponent<BoxCollider2D>();
         float SurfaceY = collision.gameObject.transform.position.y + platformCollider.offset.y + (platformCollider.size.y / 2f);
         if (PLayerFeetY < SurfaceY)
         {
            playerCollider.enabled = false;
            JumpForce = 0f;
            SkatingGravity = DefaultGravity;
         }
      }
   }

   public void Died()
   {
      this.enabled = false;
      _rigi.gravityScale = DefaultGravity;
      if (_rigi.velocity.y > 0f)
      {
         _rigi.velocity = new Vector2(_rigi.velocity.x, 0f);
      }
      skateBoard.SetActive(false);
   }
}
