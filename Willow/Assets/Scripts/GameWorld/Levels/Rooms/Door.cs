using nl.SWEG.Willow.Utils.Attributes;
using UnityEngine;

namespace nl.SWEG.Willow.GameWorld.Levels.Rooms
{
    /// <summary>
    /// A Door functions as a gateway between Rooms
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        #region Variables
        #region Public
        /// <summary>
        /// The room this Door is a part of
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
        /// The place where the player spawns when they enter the room through this Door
        /// </summary>
        public Transform Spawn { get { return spawn; } }
        #endregion

        #region Editor
        /// <summary>
        /// Tag for Player
        /// </summary>
        [TagSelector]
        [SerializeField]
        [Tooltip("Tag for Player")]
        protected string playerTag;
        /// <summary>
        /// The place where the player spawns when they enter the room through this Door
        /// </summary>
        [SerializeField]
        [Tooltip("The place where the player spawns when they enter the room through this Door")]
        private Transform spawn;
        /// <summary>
        /// The other side of the door, in a different room
        /// </summary>
        [SerializeField]
        [Tooltip("The other side of the door, in a different room")]
        private Door destination;
        /// <summary>
        /// Sprite displayed when Door is Opened
        /// </summary>
        [Space]
        [SerializeField]
        [Tooltip("Sprite displayed when Door is Opened")]
        private GameObject openSprite;
        /// <summary>
        /// Sprite displayed when Door is Closed
        /// </summary>
        [SerializeField]
        [Tooltip("Sprite displayed when Door is Closed")]
        private GameObject closedSprite;
        #endregion

        #region Private
        /// <summary>
        /// The trigger which teleports the player to the other room.
        /// </summary>
        private Collider2D coll;
        /// <summary>
        /// Room this door is a part of
        /// </summary>
        private Room room;
        #endregion
        #endregion

        #region Methods
        #region Public
        /// <summary>.
        /// Opens this Door
        /// </summary>
        public void Open()
        {
            closedSprite.SetActive(false);
            openSprite.SetActive(true);
        }

        /// <summary>
        /// Closes this Door
        /// </summary>
        public void Close()
        {
            openSprite.SetActive(false);
            closedSprite.SetActive(true);
        }
        #endregion

        #region Unity
        /// <summary>
        /// Gets Room this Door is a part of
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
        /// Switches to different Room if Player hits collider
        /// </summary>
        /// <param name="collision">Collider with which collision occurred</param>
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(playerTag)) // Make sure it's a player
                FloorManager.Instance.SwitchTo(destination);
        }
        #endregion
        #endregion
    }
}