using UnityEngine;

public class PipeSpawner : MonoBehaviour {
    public float distanceBetweenPipes;
    public GameObject pipePrefab;

    public float maxY;
    public float minY;

    private Bounds camBounds;
    private BoxCollider2D coll;

    void Start()
    {
        camBounds = Camera.main.OrthographicBounds();

        coll = GetComponent<BoxCollider2D>();
        coll.size = camBounds.size;
    }

    private void OnEnable()
    {
        Bird.OnStartFlyging += SpawnFirst;
        Bird.OnFall += StopPipes;
    }

    private void OnDisable()
    {
        Bird.OnStartFlyging -= SpawnFirst;
        Bird.OnFall -= StopPipes;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            PipeController pipe = other.GetComponentInParent<PipeController>();

            if(pipe != null && pipe.SpawnNext)
            {
                pipe.SpawnNext = false;

                Vector2 newPosition = other.transform.position;
                newPosition.x += distanceBetweenPipes;

                AddPipe(newPosition);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Pipe"))
        {
            Destroy(other.transform.parent.gameObject);
        }
    }

    void AddPipe(Vector2 position)
    {
        GameObject newPipe = Instantiate(pipePrefab);
        newPipe.transform.position = position;
        newPipe.name = "Pipe";

        PipeController pipeCon = newPipe.GetComponent<PipeController>();
        pipeCon.gapCenter = Random.Range(minY, maxY);
    }

    public void SpawnFirst()
    {
        AddPipe(new Vector2(camBounds.max.x + 5, 0));
    }

    public void ClearPipes()
    {
        foreach (GameObject pipe in GameObject.FindGameObjectsWithTag("Pipe"))
        {
            Destroy(pipe.transform.parent.gameObject);
        }
    }
    
    public void StopPipes()
    {
        foreach (GameObject pipe in GameObject.FindGameObjectsWithTag("Pipe"))
        {
            pipe.transform.parent.GetComponent<PipeController>().Enabled = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        PipeController pipe = pipePrefab.transform.GetComponent<PipeController>();

        float min = minY - (pipe.gapSize / 2);
        float max = maxY + (pipe.gapSize / 2);

        Gizmos.DrawLine(
            new Vector2(-100, min),
            new Vector2(100, min)
        );

        Gizmos.DrawLine(
            new Vector2(-100, max),
            new Vector2(100, max)
        );
    }
}
