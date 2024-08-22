using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image Heart1 = null, Heart2 = null, Heart3 = null;
    private int _MaxHealth = 3, _Health = 3;

    [SerializeField] private PlayerMovement _movementScript = null;
    [SerializeField] private LoopingBackground _backBg, _frontBg;

    [SerializeField] private Animator playerAnim = null;
    [SerializeField] private BoxCollider2D playerCollider = null;
    [SerializeField] private Rigidbody2D _rigi = null;

    DataManager dataManagerScript;

    [SerializeField] private GameObject ExplosionPrefab = null;
    [SerializeField] private GameObject WoodShatterPrefab = null, RockShatterPrefab = null;
 
    void Start()
    {
        _Health = _MaxHealth;
        dataManagerScript = FindObjectOfType<DataManager>();
    }

    public int GetHealth()
    {
        return _Health;
    }

    public int GetMaxHealth()
    {
        return _MaxHealth;
    }

    private void OnBecameInvisible()
    {
       if (_Health > 0 && transform.position.y < -3)
       {
        _Health = 0;
        SetHearts();
        Invoke(nameof(GameOver), 2f);
       } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("obstacle"))
        {
            string obstacleName = collision.gameObject.name;
            if (obstacleName.Contains("Wood"))
            {
                Vector2 shatterPosition = collision.GetComponent<WoodScript>().GetShatterPosition();
                GameObject new_explosion = Instantiate(ExplosionPrefab, shatterPosition, Quaternion.identity, collision.transform.parent);
                GameObject new_shatter = Instantiate(WoodShatterPrefab, shatterPosition, Quaternion.identity, collision.transform.parent);
            }
            else if (obstacleName.Contains("Rock"))
            {
                GameObject new_explosion = Instantiate(ExplosionPrefab, collision.transform.position, Quaternion.identity, collision.transform.parent);
                GameObject new_shatter = Instantiate(RockShatterPrefab, collision.transform.position, Quaternion.identity, collision.transform.parent);
            }
            
            GetDamage(collision.gameObject);
        }
        else if (collision.CompareTag("heart"))
        {
            CollectedHeart(collision.gameObject);
        }
    }

    void CollectedHeart(GameObject _heart)
    {
        Destroy(_heart);

        _Health = Math.Min(_Health + 1, _MaxHealth);
        SetHearts();
    }

    void GetDamage(GameObject _obsctacle)
    {
        Destroy(_obsctacle);

        if(_Health > 0)
        {
            _Health--;
            SetHearts();
            if (_Health <= 0)
            {
                Die();
            }
        }
    }

    void SetHearts()
    {
        if (_Health == 3)
        {
            Heart1.enabled = true;
            Heart2.enabled = true;
            Heart3.enabled = true;
        }
        else if (_Health == 2)
        {
            Heart1.enabled = true;
            Heart2.enabled = true;
            Heart3.enabled = false;
        }
        else if (_Health == 1)
        {
            Heart1.enabled = true;
            Heart2.enabled = false;
            Heart3.enabled = false;
        }
        else if (_Health <= 0)
        {
            Heart1.enabled = false;
            Heart2.enabled = false;
            Heart3.enabled = false;
        }
    }

    void Die()
    {
       Time.timeScale = 1f;
       playerCollider.enabled = false;
       playerAnim.SetInteger("AnimState", 1);
       _rigi.velocity = new Vector2(0, 5f);
       _movementScript.Died();
       _backBg.enabled = false;
       _frontBg.enabled = false;
       Invoke(nameof(GameOver), 2f);
    }

    void GameOver()
    {
        dataManagerScript.SaveGame();
        Time.timeScale = 1f;
        _movementScript.enabled = false;
        _backBg.enabled = false;
        _frontBg.enabled = false;
        print("Return");
        SceneManager.LoadScene(0);
    }

}
