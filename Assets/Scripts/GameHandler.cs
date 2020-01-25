using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector2> linePositions;
    public List<GameObject> Players;

    public GameObject menu;

    public Text timerHolder;

    public GameObject countDownText;
    public Text countDownHolder;
    public float countDown = 3;

    public float pullSpeed;

    public float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.displayCountDown();
    }

    private void displayCountDown() {
        this.countDown -= Time.deltaTime;

        if (this.countDown <= -1)
        {
            countDownText.SetActive(false);
            this.countDownHolder.text = "";

            this.countDown = -1;
        }
        else if (Math.Ceiling(this.countDown) <= 0)
        {
            this.countDownHolder.text = "GO";
        }
        else
        {
            this.countDownHolder.text = Math.Ceiling(this.countDown).ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Math.Ceiling(this.countDown) <= 0)
        {
            if (this.countDown >= -1)
            {
                this.displayCountDown();
            }

            if ((linePositions[0].x < 0 && linePositions[1].x > 0) && this.timer < 100)
            {
                this.timer += Time.deltaTime;

                /**
                 * Mobile control
                 */
                if (Input.touchCount > 0)
                {
                    foreach (Touch touch in Input.touches)
                    {
                        Vector2 location = new Vector2(touch.position.x, touch.position.y);

                        if (location.x < (Screen.width / 2))
                        {
                            if (linePositions[0].x > -8)
                            {
                                linePositions[0] = new Vector2(linePositions[0].x - this.pullSpeed, 0);
                            }

                            linePositions[1] = new Vector2(linePositions[1].x - this.pullSpeed, 0);
                        }
                        else if (location.x > (Screen.width / 2))
                        {
                            if (linePositions[1].x < 8)
                            {
                                linePositions[1] = new Vector2(linePositions[1].x + this.pullSpeed, 0);
                            }
                            linePositions[0] = new Vector2(linePositions[0].x + this.pullSpeed, 0);
                        }
                    }
                }
                /**
                 * end Mobile control
                 */


                /**
                 * Desktop control
                 */
                if (Input.GetMouseButtonUp(0))
                {
                    if (linePositions[0].x > -8)
                    {
                        linePositions[0] = new Vector2(linePositions[0].x - this.pullSpeed, 0);
                    }

                    linePositions[1] = new Vector2(linePositions[1].x - this.pullSpeed, 0);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    if (linePositions[1].x < 8)
                    {
                        linePositions[1] = new Vector2(linePositions[1].x + this.pullSpeed, 0);
                    }
                    linePositions[0] = new Vector2(linePositions[0].x + this.pullSpeed, 0);
                }
                /**
                 * end Desktop control
                 */
            }
            else 
            {
                Text gameOverInfo = null;
                for (int i = 0; i < this.menu.transform.childCount; i++) {
                    if (this.menu.transform.GetChild(i).name == "GameOverInfo") {
                        gameOverInfo = this.menu.transform.GetChild(i).GetComponent<Text>();
                    }
                }

                if (this.timer >= 100)
                {
                    this.timerHolder.text = String.Format("{0:0.0}", 100);
                    gameOverInfo.text = "Ran out of time";
                }
                else if (linePositions[0].x >= 0)
                {
                    gameOverInfo.text = "Blue wins";
                }
                else
                {
                    gameOverInfo.text = "Red wins";
                }

                this.menu.SetActive(true);
            }
        } else {
            this.displayCountDown();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.timerHolder.text = String.Format("{0:0.00}", this.timer);

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
            Players[i].transform.position = new Vector2(linePositions[i].x, Players[i].transform.position.y);
        }
    }
}
