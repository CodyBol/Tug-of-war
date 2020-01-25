using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * @var LineRenderer lineRenderer is the line it self
 * @var List<Vector2> linePositions contains the anchor points of the line
 * @var List<GameObject> Players contains the player GameObjects (that contains the player sprite)
 * 
 * @var GameObject menu used to access the ui elements in the menu
 * 
 * @var float timer contains the time after start
 * @var Text timerHolder this is the place where the variable timer is displayed
 * 
 * @var float countDown this is the countdown time
 * @var GameObject countDownText contains the background image of the start countdown
 * @var Text countDownHolder place where the start countdown is displayed
 * 
 * @var float pullSpeed how far a tap/click moves the players
 */
public class GameHandler : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public List<Vector2> linePositions;
    public List<GameObject> Players;

    public GameObject menu;

    public float timer = 0;
    public Text timerHolder;

    public float countDown = 3;
    public GameObject countDownText;
    public Text countDownHolder;

    public float pullSpeed;

    /**
     * Does this every frame
     */
    void Update()
    {
        if (Math.Ceiling(this.countDown) <= 0)
        {
            /**
             * does this when countdown says go
             */

            if (this.countDown >= -1)
            {
                /**
                 * hide countdown
                 */
                this.countDownHolder.text = this.displayCountDown();
            }

            if ((this.linePositions[0].x < 0 && this.linePositions[1].x > 0) && this.timer < 100)
            {
                /**
                 * No one has won or ran out of time
                 */

                /**
                 * add to current time
                 */
                this.timer += Time.deltaTime;

                /**
                 * Mobile control
                 */
                if (Input.touchCount > 0)
                {
                    /**
                     * when a player taps on screen
                     */


                    /**
                     * loops through all the taps and see on what side it's on
                     */
                    foreach (Touch touch in Input.touches)
                    {
                        /**
                         * @var Vector2 location get tap position
                         */
                        Vector2 location = new Vector2(touch.position.x, touch.position.y);

                        if (location.x < (Screen.width / 2))
                        {
                            /**
                             * tap on left side
                             */

                            if (this.linePositions[0].x > -8)
                            {
                                /**
                                 * moves player when it's not on the map border
                                 */
                                this.linePositions[0] = new Vector2(this.linePositions[0].x - this.pullSpeed, 0);
                            }

                            /**
                             * moves opponent
                             */
                            this.linePositions[1] = new Vector2(this.linePositions[1].x - this.pullSpeed, 0);
                        }
                        else if (location.x > (Screen.width / 2))
                        {
                            /**
                             * tap on right side
                             */

                            if (this.linePositions[1].x < 8)
                            {
                                /**
                                 * moves player when it's not on the map border
                                 */
                                this.linePositions[1] = new Vector2(this.linePositions[1].x + this.pullSpeed, 0);
                            }

                            /**
                             * moves opponent
                             */
                            this.linePositions[0] = new Vector2(this.linePositions[0].x + this.pullSpeed, 0);
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
                    /**
                     * on left click
                     */

                    if (linePositions[0].x > -8)
                    {
                        /**
                         * moves player when it's not on the map border
                         */

                        linePositions[0] = new Vector2(linePositions[0].x - this.pullSpeed, 0);
                    }

                    /**
                     * moves opponent
                     */
                    linePositions[1] = new Vector2(linePositions[1].x - this.pullSpeed, 0);
                }

                if (Input.GetMouseButtonUp(1))
                {
                    /**
                     * on right click
                     */

                    if (linePositions[1].x < 8)
                    {
                        /**
                         * moves player when it's not on the map border
                         */

                        linePositions[1] = new Vector2(linePositions[1].x + this.pullSpeed, 0);
                    }

                    /**
                     * moves opponent
                     */
                    linePositions[0] = new Vector2(linePositions[0].x + this.pullSpeed, 0);
                }
                /**
                 * end Desktop control
                 */
            }
            else 
            {
                /**
                 * when a player reached the center of the map (when someone lost) 
                 * or when the timer hit 100 sec (ran out of time)
                 */

                /**
                 * @var Text gameOverInfo get text box with the winner information
                 */
                Text gameOverInfo = null;
                for (int i = 0; i < this.menu.transform.childCount; i++) {
                    if (this.menu.transform.GetChild(i).name == "GameOverInfo") {
                        gameOverInfo = this.menu.transform.GetChild(i).GetComponent<Text>();
                    }
                }

                /**
                 * sets the gameover text
                 */
                gameOverInfo.text = this.gameOverText();

                /**
                 * shows the menu
                 */
                this.menu.SetActive(true);
            }
        } else {
            /**
             * shows remaining time before start
             */
            this.countDownHolder.text = this.displayCountDown();
        }
    }

    /**
     * edits UI elements after frame
     */
    void FixedUpdate()
    {
        /**
         * sets the current time
         */
        this.timerHolder.text = String.Format("{0:0.00}", this.timer);

        this.drawLine();
    }

    /**
     * gets the gameover text
     */
    public string gameOverText()
    {
        if (this.timer >= 100)
        {
            /**
             * when the players ran out of time
             */

            this.timerHolder.text = String.Format("{0:0.0}", 100);
            return "Tijd is om";
        }
        else if (linePositions[0].x >= 0)
        {
            /**
             * when the right player wins
             */

            return "Blauw wint";
        }
        else
        {
            /**
             * when the left player wins
             */

            return "Rood wint";
        }
    }

    /**
     * Draws line and moves players
     */
    public void drawLine()
    {
        for (int i = 0; i < this.linePositions.Count; i++)
        {
            /**
             * set new line anchor points
             */
            this.lineRenderer.SetPosition(i, this.linePositions[i]);

            /**
             * set new position of player sprites
             */
            this.Players[i].transform.position = new Vector2(this.linePositions[i].x, this.Players[i].transform.position.y);
        }
    }

    /**
 * used for displaying the countdown at the start of the game
 * 
 * @returns string this shows the time remaining
 */
    private string displayCountDown()
    {
        /**
         * adds the time between the frames
         */
        this.countDown -= Time.deltaTime;

        if (this.countDown <= -1)
        {
            /**
            * disable when countdown is over
            */
            this.countDownText.SetActive(false);
            this.countDown = -1;
            return "";
        }
        else if (Math.Ceiling(this.countDown) <= 0)
        {
            /**
            * tells the player when to start
            */
            return "GO";
        }
        else
        {
            /**
            * shows time remaining
            */
            return Math.Ceiling(this.countDown).ToString();
        }
    }
}
