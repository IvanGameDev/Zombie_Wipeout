using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using ZombieDriveGame.Types;
using TMPro;
using System.Collections.Generic;

public class ZDGGameController : MonoBehaviour {

        public static ZDGGameController instance;
        public static RewardedAdmob admob;
        // The camera object and the camera holder that contains it and follows the player
        internal Camera cameraObject;
        internal Transform cameraHolder;

        [Tooltip("Death counter")]
        public int deathCounter;

        [Tooltip("The player object assigned from the scene")]
        public ZDGPlayer playerObject;
        public TurnOffMusic turnOffMusic;
        public TurnOffSound turnOffSound;

        // The turning direction of the player
        internal float turnDirection = 0;

        [Tooltip("The ground object that repeats under the player while he is moving")]
        public Transform groundObject;

        [Tooltip("How far should the player move before the ground object repeats")]
        public float groundRepeatDistance = 20;

        [Tooltip("The edge of the street where there is a railing that bounces the player back")]
        public float streetEdge = 2;

        [Tooltip("How much damage touching the railing causes to the player")]
        public float streetEdgeDamage = 1;

        [Tooltip("The effect that appears when hitting a rail, assigned from the player object")]
        public Transform streetEdgeEffect;

        [Tooltip("The sound that plays when hitting the rail")]
        public AudioClip zombieExplodeSound;

        [Tooltip("A list of all the obstacles that can be spawned, some good and some bad")]
        public Spawn[] spawnObstacles;
        internal Spawn[] spawnObstaclesList;

        [Tooltip("A list of all items you can pick up, which appear only at a certain rate")]
        public List<Transform> spawnPickups;

        [Tooltip("The rate at which a pickup appear. For example if set to 50, we will create 50 obstacles before showing an item pickup")]
        public int spawnPickupRate = 50;
        internal int spawnPickupRateCount;
        internal int spawnPickupIndex = 0;
        
        [Tooltip("How gap between each two obstacles")]
        public Vector2 spawnGap = new Vector2(1,2);
        internal float spawnGapCount = 0;
        
        internal bool isSpawning = true;

        [Tooltip("How long to wait before starting the game. Ready?GO! time")]
        public float startDelay = 1;

        [Tooltip("The effect displayed before starting the game")]
        public Transform readyGoEffect;

        [Tooltip("The turn button, click it or tap it to turn the player in the opposite direction")]
        public string turnButton = "Fire1";

        [Tooltip("A delay to prevent the player from too much health at once. If you lose health, you will not lose more health for some time")]
        public float loseHealthDelay = 1;
        internal float loseHealthDelayCount;

        [Tooltip("The score of the player")]
        public int score = 0;

        [Tooltip("How many score the player needs to collect before leveling up")]
        public int levelUpEveryScore = 500;
        internal int increaseCount = 0;

        [Tooltip("How much faster the game becomes when we level up")]
        public float levelUpSpeedIncrease = 0.1f;

        [Tooltip("How much fuel you have left.")]
        public float lowFuelIndicator;

        [Tooltip("The score text object which displays the current score of the player")]
        public Transform scoreText;
		internal int highScore = 0;
		internal int scoreMultiplier = 1;
        
        // Various canvases for the UI
        public Transform gameCanvas;
        public Transform healthCanvas;
        public Transform fuelCanvas;
        public Transform pauseCanvas;
		public Transform gameOverCanvas;

		// Is the game over?
		public bool isGameOver = false;
		
		// The level of the main menu that can be loaded after the game ends
		public string mainMenuLevelName = "StartMenu";
		
		// Various sounds and their source
		public AudioClip soundGameOver;
		public string soundSourceTag = "GameController";
		internal GameObject soundSource;
		
		// The button that will restart the game after game over
		public string confirmButton = "Submit";
		
		// The button that pauses the game. Clicking on the pause button in the UI also pauses the game
		public string pauseButton = "Cancel";
		internal bool  isPaused = false;

		// A general use index
		internal int index = 0;

        [SerializeField]
        private GameObject tutorialSpace;
        [SerializeField]
        public GameObject fuelLeft;
        [SerializeField]
        public GameObject infiniteFuelButton;
        /*[SerializeField]
        private GameObject infiniteDurabilityButton;*/

      private bool isTutorial;

        [SerializeField]
        private float metersPassed;
        [SerializeField]
        private float metersPassedIncrease;

        [SerializeField]
        private TextMeshProUGUI metersPassedText;

		//public Transform slowMotionEffect;

		void Awake()
		{
            if(instance == null)
            {
                instance = this;
            }
            // Activate the pause canvas early on, so it can detect info about sound volume state
            if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(true);
        }

		void Start()
		{
            playerObject.fuelLeft = playerObject.fuel;
            playerObject.healthLeft = playerObject.health;

            // If the camera is not assigned yet, assign it and set the camera holder too
            if (cameraObject == null)
            {
                cameraObject = Camera.main;

                // Set the camera holder from the camera object
                if (cameraObject.transform.root) cameraHolder = cameraObject.transform.root;
            }

            // If we have a player defined, set its attributes in the gamecontroller
            if (playerObject)
            {
                // Set the turning speed of the player
                turnDirection = playerObject.turnRange;
                
                // Set the health and fuel of the player
                playerObject.healthMax = playerObject.health;
                playerObject.fuelMax = playerObject.fuel;
            }

            // Disable multitouch so that we don't tap two answers at the same time ( prevents multi-answer cheating, thanks to Miguel Paolino for catching this bug )
            Input.multiTouchEnabled = false;
            
            // Update the score at 0
            ChangeScore(0);

            // Update the number of lives we have
            ChangeHealth(0);
            UpdateFuel();

            loseHealthDelayCount = 0;
            
            //Hide the cavases
            if ( gameOverCanvas )    gameOverCanvas.gameObject.SetActive(false);
			if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);

            //Get the highscore for the player
            //highScore = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "HighScore", 0);
            highScore = PlayerPrefs.GetInt("HighScore", score);

            deathCounter = PlayerPrefs.GetInt("DeathCounter", deathCounter);

            // Calculate the chances for the objects to spawn
            int totalSpawns = 0;
            int totalSpawnsIndex = 0;

            // Calculate the total number of spawns with their chances
            for (index = 0; index < spawnObstacles.Length; index++)
            {
                totalSpawns += spawnObstacles[index].spawnChance;
            }

            // Create a new list of the objects that can be dropped
            spawnObstaclesList = new Spawn[totalSpawns];

            // Go through the list again and fill out each type of drop based on its drop chance
            for (index = 0; index < spawnObstacles.Length; index++)
            {
                int laneChanceCount = 0;

                while (laneChanceCount < spawnObstacles[index].spawnChance)
                {
                    spawnObstaclesList[totalSpawnsIndex] = spawnObstacles[index];

                    laneChanceCount++;

                    totalSpawnsIndex++;
                }
            }

            spawnPickupRateCount = spawnPickupRate;

            //Assign the sound source for easier access
            if ( GameObject.FindGameObjectWithTag(soundSourceTag) )    soundSource = GameObject.FindGameObjectWithTag(soundSourceTag);

			// Reset the spawn delay
			spawnGapCount = Random.Range( spawnGap.x, spawnGap.y);
			
			// Create the ready?GO! effect
			//if ( readyGoEffect )    Instantiate( readyGoEffect );

        }

		void Update()
		{
            if(isTutorial == false){
                MetersPassed();
            }

			// Delay the start of the game
			if ( startDelay > 0 )
			{
				startDelay -= Time.deltaTime;

                // Move the player forward even before we start playing
                if (playerObject) playerObject.transform.Translate(Vector3.forward * Time.deltaTime * playerObject.speed, Space.Self);
            }
			else
			{
				//If the game is over, listen for the Restart and MainMenu buttons
				if ( isGameOver == true )
				{
					//The jump button restarts the game
					if ( Input.GetButtonDown(confirmButton) )
					{
						Restart();
					}
					
					//The pause button goes to the main menu
					if ( Input.GetButtonDown(pauseButton) )
					{
						MainMenu();
					}
				}
				else
				{
                    // If there is a player object, move it forward and turn it in the correct direction
                        if (playerObject)
                        {
                        // Move the player forward
                        playerObject.transform.Translate(Vector3.forward * Time.deltaTime * playerObject.speed, Space.Self);

                        // Rotate the player to the correct angle
                        if ( playerObject.transform.eulerAngles.y != turnDirection ) 
                        playerObject.transform.eulerAngles = Vector3.up * Mathf.LerpAngle(playerObject.transform.eulerAngles.y, turnDirection, 
                        Time.deltaTime * playerObject.turnSpeed);
                        
                        //Vector3.RotateTowards(playerObject.transform.eulerAngles, Vector3.up * turnDirection, Time.deltaTime * playerObject.turnSpeed, 0.0F);

                        // If the player touches the edges of the street, bounce it back
                        if ( playerObject.transform.position.x > streetEdge || playerObject.transform.position.x < -streetEdge ) BounceOffRail();
                        
                        // Count down the time until game over
                        if (playerObject.fuel > 0)
                        {
                            // Reduce from the player's fuel
                            playerObject.fuel -= Time.deltaTime * playerObject.speed * 0.2f;
                            playerObject.fuelLeft -= Time.deltaTime * playerObject.speed * 0.2f;
                            if( playerObject.fuelLeft <= lowFuelIndicator)
                            {
                                fuelLeft.SetActive(true);
                            }

                            // Update the timer
                            UpdateFuel();
                        }
                    }

                    // Count the delay for losing health. If this is larger than 0, you can't lose health
                    if (loseHealthDelayCount > 0) loseHealthDelayCount -= Time.deltaTime;

                    // If we press the turn button, Turn!
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        if (Input.GetButtonDown(turnButton)) turnDirection *= -1;
                    }

                    // Count down to the next target spawn
                    if ( isSpawning == true )
                    {
                        if (spawnGapCount > 0) spawnGapCount -= playerObject.speed * Time.deltaTime;
                        else
                        {
                            // Reset the spawn delay count
                            spawnGapCount = Random.Range(spawnGap.x, spawnGap.y);

                            if ( spawnPickupRateCount > 0 )
                            {
                                // Choose a random spawn from the list of spawns
                                int randomSpawn = Mathf.FloorToInt(Random.Range(0, spawnObstaclesList.Length));

                                // Create a new random target from the target list
                                Transform newSpawn = Instantiate(spawnObstaclesList[randomSpawn].spawnObject) as Transform;

                                // Place the target at a random position along the height
                                newSpawn.position = new Vector3(Random.Range(-streetEdge, streetEdge), 0, playerObject.transform.position.z + groundRepeatDistance);

                                spawnPickupRateCount--;

                                // Add to the spawn gap based on the spawn object
                                spawnGapCount += spawnObstaclesList[randomSpawn].spawnGap;
                            }
                            else
                            {
                                // Create a new pickup spawn based on the index which loops in the list
                                Transform newSpawn = Instantiate(spawnPickups[spawnPickupIndex]) as Transform;

                                // Place the target at a random position along the height
                                newSpawn.position = new Vector3(Random.Range(-streetEdge, streetEdge), 0, playerObject.transform.position.z + groundRepeatDistance);

                                // Go to the next spawn pickup in the list
                                spawnPickupIndex++;

                                // Reset the index if we reach the end of the list
                                if (spawnPickupIndex > spawnPickups.Count - 1) spawnPickupIndex = 0;

                                // Reset the spawn pick up rate counter
                                spawnPickupRateCount = spawnPickupRate;
                            }
                        }
                    }

					//Toggle pause/unpause in the game
					if ( Input.GetButtonDown(pauseButton) )
					{
						if ( isPaused == true )    Unpause();
						else    Pause(true);
					}
				}
			}
		}

        void LateUpdate()
        {
            if (playerObject)
            {
                // Make the camera holder follow the position of the player along the street
                cameraHolder.position = new Vector3(cameraHolder.position.x, cameraHolder.position.x, playerObject.transform.position.z);
            }
        }

		void UpdateFuel()
        {
            if ( playerObject )
            {
                // Update the fuel bar
                fuelCanvas.GetComponent<Image>().fillAmount = playerObject.fuel / playerObject.fuelMax;

                // Time's up!
                if ( playerObject.fuel <= 0 )
                {
                    //playerObject.speed -= Time.deltaTime * 1.5f;
                    StartCoroutine(SlowDownTime());
                    StartCoroutine(GameOver(0));
                }
            }
            
        }
        /// <param name="changeValue"></param>
        public void ChangeFuel(float changeValue)
        {
            // Change the value of the fuel
            playerObject.fuel += changeValue;
            playerObject.fuelLeft += changeValue;

            // Limit the value of the fuel to the maximum allowed value
            if (playerObject.fuel > playerObject.fuelMax) playerObject.fuel = playerObject.fuelMax;
            if (playerObject.fuelLeft > playerObject.fuelMax) playerObject.fuelLeft = playerObject.fuelMax;
            fuelLeft.SetActive(false);

            // Play the animation of the fuel icon
            if (fuelCanvas && fuelCanvas.GetComponent<Animation>()) fuelCanvas.GetComponent<Animation>().Play();
        }
        public void BounceOffRail()
        {
            if ( playerObject )
            {
                // Damage the player
                ChangeHealth(-streetEdgeDamage);

                // If the player went too far to the right, bounce it back to the left
                if ( playerObject.transform.position.x > streetEdge )
                {
                    // Switch the turning direction of the player
                    turnDirection = -playerObject.turnRange;

                    // Move the player a little to the center so it doesn't get stuck in the railing
                    playerObject.transform.position = new Vector3( streetEdge - 0.2f, 0, playerObject.transform.position.z);

                    // Create the effect of hitting the rail
                    if (streetEdgeEffect)
                    {
                        // Set the scale to normal
                        streetEdgeEffect.localScale = new Vector3( 1, streetEdgeEffect.localScale.y, streetEdgeEffect.localScale.z);
                        
                        // Play the particle effect of hitting the rail
                        streetEdgeEffect.Find("Particle").GetComponent<ParticleSystem>().Emit(10);
                    }
                }
                else if (playerObject.transform.position.x < -streetEdge ) // Otherwise, if the player went too far to the left, bounce it back to the right
                {
                    // Switch the turning direction of the player
                    turnDirection = playerObject.turnRange;

                    // Move the player a little to the center so it doesn't get stuck in the railing
                    playerObject.transform.position = new Vector3( -streetEdge + 0.2f, 0, playerObject.transform.position.z);
                    
                    // Create the effect of hitting the rail
                    if (streetEdgeEffect)
                    {
                        // Flip the scale to the other side, so that the effect appers on the left side of the player
                        streetEdgeEffect.localScale = new Vector3( -1, streetEdgeEffect.localScale.y, streetEdgeEffect.localScale.z);

                        // Play the particle effect of hitting the rail
                        streetEdgeEffect.Find("Particle").GetComponent<ParticleSystem>().Emit(10);
                    }
                }

                // If there is a sound and a source, play it
                //if (soundSource && streetEdgeSound) soundSource.GetComponent<AudioSource>().PlayOneShot(streetEdgeSound);

                // Reset the rotation of the player
                playerObject.transform.eulerAngles = Vector3.up;
            }
        }
        /// <param name="changeValue"></param>
        public void ChangeHealth(float changeValue)
        {
            // Change the health value
            playerObject.health += changeValue;
            playerObject.healthLeft += changeValue;
            
            // Limit the value of the health to the maximum allowed value
            if (playerObject.health > playerObject.healthMax) playerObject.health = playerObject.healthMax;
            if (playerObject.healthLeft > playerObject.healthMax) playerObject.healthLeft = playerObject.healthMax;

        // If we are recieving damage, check if we should die
        if ( loseHealthDelayCount <= 0 && changeValue < 0 )
            {
                if (playerObject.health <= 0)
                {
                    playerObject.Die();

                // Health reached 0, so the target should die
                    StartCoroutine(SlowDownTime());
                    StartCoroutine(GameOver(1));
                }

                loseHealthDelayCount = loseHealthDelay;
            }

            // Update the health bar 
            if (healthCanvas)
            {
                // Update the health bar based on the health we have
                healthCanvas.GetComponent<Image>().fillAmount = playerObject.health / playerObject.healthMax;

                // Play the animation of the health icon
                if (healthCanvas.GetComponent<Animation>()) healthCanvas.GetComponent<Animation>().Play();
            }
        }
        /// <param name="changeValue">Change value</param>
        public void  ChangeScore( int changeValue )
		{
            // Change the score value
			score += changeValue;

            //Update the score text
            if (scoreText)
            {
                scoreText.GetComponent<Text>().text = score.ToString();

                // Play the score object animation
                if (scoreText.GetComponent<Animation>()) scoreText.GetComponent<Animation>().Play();
            }

            //Increase the counter to the next level
            increaseCount += changeValue;

            //If we reached the required score, level up!
            if (increaseCount >= levelUpEveryScore)
            {
                increaseCount -= levelUpEveryScore;

                LevelUp();
            }
        }

        private void MetersPassed(){

            metersPassed += Time.deltaTime * 3.5f;
            metersPassedText.text = ((int)metersPassed).ToString() + " m";
        }

		void LevelUp()
        {
            //Increase game speed
            playerObject.speed += levelUpSpeedIncrease;
        }

        /// <param name="showMenu">If set to <c>true</c> show menu.</param>
        public void Pause(bool showMenu)
        {
            isPaused = true;

            //Set timescale to 0, preventing anything from moving
            Time.timeScale = 0;

            //Show the pause screen and hide the game screen
            if (showMenu == true)
            {
                if (pauseCanvas) pauseCanvas.gameObject.SetActive(true);
                if (gameCanvas) gameCanvas.gameObject.SetActive(false);
            }
        }

        public void Unpause()
        {
            isPaused = false;

            //Hide the pause screen and show the game screen
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);
            if (gameCanvas) gameCanvas.gameObject.SetActive(true);
        }

        IEnumerator GameOver(float delay)
		{
			isGameOver = true;
            deathCounter += 1;

            //Remove the pause and game screens
            if ( pauseCanvas )    pauseCanvas.gameObject.SetActive(false);
            if ( gameCanvas )    gameCanvas.gameObject.SetActive(false);

            //Show the game over screen
            if ( gameOverCanvas )    
			{
                yield return new WaitForSeconds(1f);
				//Show the game over screen
				gameOverCanvas.gameObject.SetActive(true);
				
				//Write the score text
				gameOverCanvas.Find("Base/ZombiesKilledPanel/TextScore").GetComponent<TextMeshProUGUI>().text = " " + score.ToString();
				
				//Check if we got a high score
				if ( score > highScore )    
				{
					highScore = score;

                //Register the new high score
                //PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "HighScore", score);
                PlayerPrefs.SetInt("HighScore", score);
                }

                PlayerPrefs.SetInt("DeathCounter", deathCounter);
                //Write the high sscore text
                gameOverCanvas.Find("Base/HighestKillstreakPanel/TextHighScore").GetComponent<TextMeshProUGUI>().text = " " + highScore.ToString();

				//If there is a source and a sound, play it from the source
				if ( soundSource && soundGameOver )    
				{
					soundSource.GetComponent<AudioSource>().pitch = 1;
					
					soundSource.GetComponent<AudioSource>().PlayOneShot(soundGameOver);
				}
			}
        }

        IEnumerator SlowDownTime()
        {
            while (Time.timeScale > 0.2f)
            {
                Time.timeScale -= Time.deltaTime * 1.75f;

                yield return null;
            }

            yield return new WaitForSeconds(0.4f);

            while (Time.timeScale <= 1.0f)
            {
                Time.timeScale += Time.deltaTime;

                yield return null;
            }

                Time.timeScale = 1.0f;
        }

        public void  Restart()
		{
		    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            isPaused = false;
            Time.timeScale = 1.0f;
            if (pauseCanvas) pauseCanvas.gameObject.SetActive(false);
            if (gameCanvas) gameCanvas.gameObject.SetActive(true);
            playerObject.playerCar.SetActive(true);
        }

        public void  MainMenu()
		{
			//SceneManager.LoadScene(mainMenuLevelName);
            Time.timeScale = 1.0f;
            isPaused = false;
            turnOffMusic.TurnMusicOff();
            turnOffSound.TurnSoundOff();
		}

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            // Draw two lines to show the edges of the street
            Gizmos.DrawLine(new Vector3(streetEdge, 1, 0), new Vector3(streetEdge, 0,10));
            Gizmos.DrawLine(new Vector3(-streetEdge, 1, 0), new Vector3(-streetEdge, 0, 10));
        }
    }
