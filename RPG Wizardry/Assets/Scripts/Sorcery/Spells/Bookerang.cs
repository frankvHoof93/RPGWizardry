using nl.SWEG.RPGWizardry.Entities.Stats;
using nl.SWEG.RPGWizardry.Player;
using nl.SWEG.RPGWizardry.Utils.Functions;
using UnityEngine;

namespace nl.SWEG.RPGWizardry.Sorcery.Spells
{
    public class Bookerang : Projectile
    {
        #region Variables
        /// <summary>
        /// Speed at which the book rotates in flight
        /// </summary>
        [SerializeField]
        private float SpinSpeed;
        /// <summary>
        /// Duration for which the book pauses before returning
        /// </summary>
        [SerializeField]
        private float hangTime = 0.3f;
        /// <summary>
        /// Bool for whether the book is moving forwards (false) or returning (true)
        /// </summary>
        private bool back;
        /// <summary>
        /// Bool to make the book pause in mid-air
        /// </summary>
        private bool pause;
        /// <summary>
        /// Float to keep track of how far we've moved
        /// </summary>
        private float movedSpace = 0;
        /// <summary>
        /// Sprite of the "crosshair" book
        /// </summary>
        private SpriteRenderer bookRenderer;
        /// <summary>
        /// Transform of the object that contains the sprite
        /// (in a separate object so we can spin it independently)
        /// </summary>
        [SerializeField]
        private Transform spriteTransform;
        /// <summary>
        /// Current location of the player, so we can return to it
        /// </summary>
        private Transform playerLocation;
        /// <summary>
        /// Position where we ended the forward movement
        /// </summary>
        private Vector3 savedPosition;
        #endregion

        #region Methods
        #region Unity
        /// <summary>
        /// Get references for the player position and renderer of the "crosshair" book
        /// </summary>
        protected override void Start()
        {
            base.Start();
            if (PlayerManager.Exists)
            {
                PlayerManager player = PlayerManager.Instance;
                //Get the location of the player (to return to later)
                playerLocation = player.transform;
                //Turn the "crosshair" book invisible so it looks like this projectile IS the book
                bookRenderer = player.BookRenderer;
                bookRenderer.enabled = false;
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Move in a straight line until we hit something or max out our range
        /// Wait for a short time, then return to the player
        /// </summary>
        protected override void Move()
        {
            //SPIN TO WIN
            spriteTransform.Rotate(Vector3.forward, SpinSpeed * Time.deltaTime,Space.World);

            //If not paused in mid-air
            if (!pause)
            {
                //If not returning to player
                if (!back)
                {
                    //If we haven't flown our full range yet
                    if (movedSpace < data.LifeTime)
                    {
                        //Fly in a straight line
                        base.Move();
                        movedSpace += Time.deltaTime * data.ProjectileSpeed;
                        
                    }
                    else
                    {
                        //Start returning to the player
                        Return();
                    }
                }
                //If returning to the player
                else
                {
                    //Use a lerp so we always target the player's current position
                    transform.position = Vector3.Lerp(savedPosition, playerLocation.position, movedSpace);
                    //Set speed so we always make it in time
                    movedSpace += Time.deltaTime * 5;

                    //If we've made it
                    if (movedSpace >= 1)
                    {
                        //Make the "crosshair" book reappear and delete this projectile
                        bookRenderer.enabled = true;
                        Destroy(gameObject);
                    }
                }
            }
        }

        /// <summary>
        /// Instead of deleting itself, return the projectile to the player
        /// </summary>
        /// <param name="collision">Object the projectile collided with</param>
        protected override void Effect(Collider2D collision)
        {
            //Bool check so it doesnt get stuck on anything on the way back
            if (!back)
            {
                //apply knockback
                Rigidbody2D body = collision.gameObject.GetComponent<Rigidbody2D>();
                body.AddForce(transform.up * data.Knockback);

                //apply damage
                collision.gameObject.GetComponent<IHealth>()?.Damage(data.Damage);

                Return();
            }
        }
        #endregion

        #region Private
        /// <summary>
        /// Returns the book to the player, after hanging in the air for a short time
        /// </summary>
        private void Return()
        {
            back = true;
            savedPosition = transform.position;
            movedSpace = 0;
            pause = true;
            StartCoroutine(CoroutineMethods.RunDelayed(() => { pause = false; }, hangTime));
        }
        #endregion
        #endregion
    }
}