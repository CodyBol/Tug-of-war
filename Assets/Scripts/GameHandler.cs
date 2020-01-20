using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector2> linePositions;

    public float pullSpeed;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (linePositions[0].x > -8) {
                linePositions[0] = new Vector2(linePositions[0].x - this.pullSpeed, 0);
            }

            linePositions[1] = new Vector2(linePositions[1].x - this.pullSpeed, 0);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (linePositions[1].x < 8) {
                linePositions[1] = new Vector2(linePositions[1].x + this.pullSpeed, 0);
            }
            linePositions[0] = new Vector2(linePositions[0].x + this.pullSpeed, 0);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.drawLine();
    }

    /**
     * Draw line
     */
    public void drawLine()
    {
        for (int i = 0; i < this.linePositions.Count; i++)
        {
            this.lineRenderer.SetPosition(i, linePositions[i]);
        }
    }
}
