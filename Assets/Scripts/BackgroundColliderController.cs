using UnityEngine;
using System.Linq;


public class BackgroundColliderController : MonoBehaviour
{
    private int numberOfBgs;
    private float distanceBetweenBgs;

    private int numberOfGrounds;
    private float distanceBetweenGrounds;

    private int numberOfPipes;
    private float distanceBetweenPipes;

    private bool upperPipe; 

    public void Start()
    {
        var bgs = GameObject.FindGameObjectsWithTag("Background");
        var grounds = GameObject.FindGameObjectsWithTag("Ground");
        var pipes = GameObject.FindGameObjectsWithTag("Pipe");
        
        RandomizePipes(pipes);

        this.numberOfBgs = bgs.Length;
        this.numberOfGrounds = grounds.Length;
        this.numberOfPipes = pipes.Length;

        if (this.numberOfBgs < 2 || this.numberOfGrounds < 2 || this.numberOfPipes < 2)
        {
            throw new System.InvalidOperationException("You must have at least two backgrounds, grounds or pipes in your scene!");
        }

        this.distanceBetweenBgs = this.DistanceBetweenObjects(bgs);

        this.distanceBetweenGrounds = this.DistanceBetweenObjects(grounds);

        this.distanceBetweenPipes = this.DistanceBetweenObjects(pipes);
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Background") || collider.CompareTag("Ground") || collider.CompareTag("Pipe"))
        {
            var go = collider.gameObject;
            var originalPosition = go.transform.position;
            

            if (collider.CompareTag("Background"))
            {
                originalPosition.x +=
                this.numberOfBgs
                * this.distanceBetweenBgs;
            }
            else if (collider.CompareTag("Ground"))
            {
                originalPosition.x +=
                this.numberOfGrounds
                * this.distanceBetweenGrounds;
            }
            else if (collider.CompareTag("Pipe"))
            {
                originalPosition.x +=
                this.numberOfPipes
                * this.distanceBetweenPipes;

                float randomY;

                if (upperPipe)
                {
                    randomY = Random.Range(1, 2);
                }

                else
                {
                    randomY = Random.Range(-0.4f, 3);
                }

                originalPosition.y = randomY;
                this.upperPipe = !this.upperPipe;

            }

            go.transform.position = originalPosition;

        }
       
    }

    private float DistanceBetweenObjects(GameObject[] gameObjects)
    {
        float minDistance = float.MaxValue;

        for (int i = 1; i < gameObjects.Length; i++)
        {
           var current =  Mathf.Abs(gameObjects[i - 1].transform.position.x - gameObjects[i].transform.position.x);

            if(current < minDistance)
            {
                minDistance = current;
            }
        }
        return minDistance;
    }

    private void RandomizePipes(GameObject[] pipes)
    {

        int count = 0;

        for(int i = 1; i< pipes.Length; i++)
        {
            count++;
            var current = pipes[i];
            float randomY;
            if(count % 2 == 0)//upper
            {
                randomY = Random.Range(1, 2);
            }
            else//down
            {
                randomY = Random.Range(-0.4f, 3);
            }
            
            var pipesPosition = current.transform.position;

            pipesPosition.y = randomY;
            current.transform.position = pipesPosition;
        }
    }
}

