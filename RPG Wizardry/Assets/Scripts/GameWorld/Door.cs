using UnityEngine;

namespace nl.SWEG.RPGWizardry.GameWorld
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        #region Variables
        #region Public
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

        /// <summary>
        /// The place where the player spawns when they enter the room. (Or when the player is spawned/respawned)
        /// </summary>
        public Transform Spawn { get { return spawn; } }
        #endregion

        #region Editor
        /// <summary>
        /// The place where the player spawns when they enter the room.
        /// </summary>
        [SerializeField]
        [Tooltip("The place where the player spawns when they enter the room. (Or when the player is spawned/respawned)")]
        private Transform spawn;
        /// <summary>
        /// The other side of the door, in a different room.
        /// </summary>
        [SerializeField]
        [Tooltip("The other side of the door, in a different room.")]
        private Door destination;
        /// <summary>
        /// The opened door sprite.
        /// </summary>
        [Space]
        [SerializeField]
        [Tooltip("The opened door sprite.")]
        private GameObject openSprite;
        /// <summary>
        /// The closed door sprite.
        /// </summary>
        [SerializeField]
        [Tooltip("The closed door sprite.")]
        private GameObject closedSprite;
        #endregion

        #region Private
        /// <summary>
        /// The trigger which teleports the player to the other room.
        /// </summary>
        private Collider2D collider;
        /// <summary>
        /// Room this door is a part of
        /// </summary>
        private Room room;
        #endregion
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
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            FloorManager.Instance.SwitchTo(destination);
        }
        #endregion
        #endregion
    }
}