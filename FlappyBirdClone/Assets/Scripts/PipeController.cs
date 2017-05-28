using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pipe
{
    public Transform end;
    public Transform body;
}

public class PipeController : MonoBehaviour
{
    public float gapSize;
    public float gapCenter;

    private Pipe pipeUp;
    private Pipe pipeDown;

    private Bounds camBounds;

    private bool _enabled;
    public bool Enabled
    {
        get
        {
            return _enabled;
        }
        set
        {
            _enabled = value;

            GetComponent<AutoMovement>().enabled = value;
        }
    }
    public bool SpawnNext { get; set; }

    void Start()
    {
        Init();

        // Cano de cima
        CalculatePipeHeight(pipeUp, camBounds.max.y, gapCenter + (gapSize / 2));
        //Cano de baixo
        CalculatePipeHeight(pipeDown, gapCenter - (gapSize / 2), camBounds.min.y);

        // Posiciona o sprite do final do cano
        AdjustPipeEndPosition();
    }

    private void Init()
    {
        pipeUp.end = transform.Find("PipeUpEnd");
        pipeUp.body = transform.Find("PipeUpBody");
        
        pipeDown.end = transform.Find("PipeDownEnd");
        pipeDown.body = transform.Find("PipeDownBody");
        
        camBounds = Camera.main.OrthographicBounds();

        SpawnNext = true;
    }

    private void CalculatePipeHeight(Pipe pipe, float top, float bottom)
    {
        float height = top - bottom;
        float yPosition = top - height / 2;

        pipe.body.localScale = new Vector2(pipe.body.localScale.x, height);
        pipe.body.position = new Vector3(pipe.body.position.x, yPosition, pipe.body.position.z);
        
    }

    private void AdjustPipeEndPosition()
    {
        Bounds collBounds = pipeUp.body.GetComponent<BoxCollider2D>().bounds;
        Bounds endBounds = pipeUp.end.GetComponent<Renderer>().bounds;
        pipeUp.end.position = new Vector3(pipeUp.end.position.x, collBounds.min.y + endBounds.extents.y, pipeUp.end.position.z);

        collBounds = pipeDown.body.GetComponent<BoxCollider2D>().bounds;
        endBounds = pipeDown.end.GetComponent<Renderer>().bounds;
        pipeDown.end.position = new Vector3(pipeUp.end.position.x, collBounds.max.y - endBounds.extents.y, pipeUp.end.position.z);
    }
}
