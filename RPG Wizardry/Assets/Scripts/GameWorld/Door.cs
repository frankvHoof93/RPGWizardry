using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        #region Variables
        /// <summary>
        /// The room the door is leading to.
        /// </summary>
        public Room Room
        {
            get
            {
                if (room == null)
                    Awake();
                return room;
            }
        }

        public Transform Spawn { get { return spawn; } }

        /// <summary>
        /// The place where the player spawns when they enter the room.
        /// </summary>
        [SerializeField]
        private Transform spawn;


        /// <summary>
        /// The other side of the door, in a different room.
        /// </summary>
        [SerializeField]
        private Door destination;

        /// <summary>
        /// The trigger which teleports the player to the other room.
        /// </summary>
        private Collider2D collider;

        /// <summary>
        /// The opened door sprite.
        /// </summary>
        [Space]
        [SerializeField]
        private GameObject openSprite;

        /// <summary>
        /// The closed door sprite.
        /// </summary>
        [SerializeField]
        private GameObject closedSprite;

        private Room room;
        #endregion

        #region Methods
        #region Public
        /// <summary>.
        /// Opens the door.
        /// </summary>
        public void Open()
        {
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
        }

        /// <summary>
        /// Closes the door.
        /// </summary>
        public void Close()
        {
            openSprite.SetActive(false);
            closedSprite.SetActive(true);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets TargetRoom
        /// </summary>
        private void Awake()
        {
            Transform tf = transform;
            while (room == null)
            {
                room = tf.GetComponent<Room>();
                if (room == null && tf != tf.root)
                    tf = tf.parent;
                else // Found root (or Room)
                    return;
            }
        }

        /// <summary>
        /// Checks if the player is hitting the room switch trigger.
        /// </summary>
        /// <param name="collision"></param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            FloorManager.Instance.SwitchTo(destination);
        }
        #endregion
        #endregion
    }
}