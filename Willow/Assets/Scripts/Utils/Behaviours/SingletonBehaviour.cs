using UnityEngine;

namespace nl.SWEG.Willow.Utils.Behaviours
{
    /// <summary>
    /// Turns any MonoBehaviour into a Singleton (Static Instance-Reference)
    /// <para>
    ///     Usage: <c>public class MyClass : SingletonBehaviour&lt;MyClass&gt; {  }</c>
    /// </para>
    /// </summary>
    /// <typeparam name="T">Class to create Singleton for (must inherit MonoBehaviour)</typeparam>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        #region Variables
        #region Static
        #region Public
        /// <summary>
        /// Whether an instance exists
        /// </summary>
        public static bool Exists => instance != null;

        /// <summary>
        /// Singleton-Reference
        /// Auto-Creates GameObject if it does not exist
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance != null)
                    return instance;
                Debug.LogWarning("Creating Instance");
                GameObject obj = new GameObject
                {
                    name = typeof(T).Name + "-Singleton"
                };
                instance = obj.AddComponent<T>();
                return instance;
            }
        }
        #endregion

        #region Protected
        /// <summary>
        /// Internal Singleton-Reference
        /// </summary>
        protected static T instance;
        #endregion
        #endregion

        #region Instance
        #pragma warning disable 0649 // Hide Null-Warning for Editor-Variables
        /// <summary>
        /// Whether this Singleton has a Root-Object. If true, root-Object will be added to DontDestroyOnLoad instead
        /// </summary>
        [SerializeField]
        [Tooltip("Whether this Singleton has a Root-Object. If true, root-Object will be added to DontDestroyOnLoad instead")]
        protected bool hasRootObject;
        #pragma warning restore 0649 // Restore Null-Warning after Editor-Variables
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// Singleton-Setup
        /// </summary>
        protected virtual void Awake()
        {
            if (instance != null && !ReferenceEquals(instance, this))
            {
                Debug.LogError($"Singleton<{typeof(T).Name}> already exists! Existing Object: {instance.gameObject.name}. Destroying new object {gameObject.name}", gameObject);
                Destroy(gameObject);
                return;
            }
            if (!hasRootObject && transform.parent != null)
                Debug.LogError($"Singleton<{typeof(T).Name}> on {gameObject.name} is not a root-object. Did you mean to set HasRootObject?");
            DontDestroyOnLoad(!hasRootObject ? gameObject : transform.root.gameObject);
            instance = this as T;
        }

        /// <summary>
        /// Singleton-Destruction
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Exists && ReferenceEquals(instance, this))
                instance = null;
        }
        #endregion
    }
}