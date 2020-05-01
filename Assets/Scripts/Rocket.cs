using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audiosourse;
    public float rotSpeed = 100f;
    public float flySpeed = 100f;
    //!!!
    [SerializeField] AudioClip boomSound; //!!!
    [SerializeField] AudioClip finishSound; //!!!
    [SerializeField] ParticleSystem boomP;
    [SerializeField] ParticleSystem finishP;

    enum State {Playing, Dead, NextLevel};
    State state = State.Playing;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Playing;
        rigidBody = GetComponent<Rigidbody>();
        audiosourse = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Playing)
        {
            RocketLaunch(); 
            RotateRoket();
        }

    }
    void OnCollisionEnter(Collision collision)
    {
        if (state == State.Dead || state == State.NextLevel)
        {
            return;
        }
        switch(collision.gameObject.tag)
        {
            case "Friendly":
            print("Ok");
            break;
            case "Finish":
            state = State.NextLevel;
            audiosourse.Stop();
            audiosourse.PlayOneShot(finishSound);///!!!
            finishP.Play();
            Invoke("LoadNextLevel",3.5f);
            break;
            case "Battery":
            print("Energy");
            break;
            default:
            state = State.Dead;
            audiosourse.Stop();
            audiosourse.PlayOneShot(boomSound);
            boomP.Play();
            print ("RocketBoom");
            Invoke("LoadFirstLevel",3f);
            break;
        }
    }
    void RocketLaunch()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * flySpeed);
            if (audiosourse.isPlaying == false)
            audiosourse.Play();///!!!
        }
        else
        {
            audiosourse.Pause();
        } 
    }
    void RotateRoket()
    {
        float rotationSpeed = rotSpeed * Time.deltaTime;
        rigidBody.freezeRotation = true;
        if(Input.GetKey(KeyCode.A))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        rigidBody.freezeRotation = false;
    }


    void LoadNextLevel ()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex+1; 
        SceneManager.LoadScene(nextLevelIndex);
    }
    void LoadFirstLevel ()
    {
        SceneManager.LoadScene(0);
    }
}