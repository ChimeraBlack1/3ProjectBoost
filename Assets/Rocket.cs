using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;


    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State {
        Alive,
        Transcending,
        Dying
    }

    State state = State.Alive;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        // todo somewhere stop sound on death
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
        
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                print("OK");
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); // paramaterize time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); // parameterize time
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // todo allow for more than 2 levels
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))  // can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        
        float rotationSpeed = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }

    

}
