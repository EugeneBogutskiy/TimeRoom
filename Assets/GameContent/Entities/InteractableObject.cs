using GameContent.Entities.Abstract;
using UnityEngine;

namespace GameContent.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public abstract class InteractableObject : MonoBehaviour, IInteractable, IItemState
    {
        [SerializeField]
        private string _id;
        
        private InteractableData _initialData;

        protected Rigidbody _rigidbody;

        public string Id => _id;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _initialData = new InteractableData(transform);
        }

        public abstract void Interact();

        public void SetState(InteractableData data)
        {
            data.RestoreState(transform);
        }

        public InteractableData GetState()
        {
            return new InteractableData(transform);
        }

        public void RestoreDefaultState()
        {
            _initialData.RestoreState(transform);
        }
    }

    public struct InteractableData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public InteractableData(Transform transform)
        {
            Position = transform.position;
            Rotation = transform.rotation;
        }

        public void RestoreState(Transform transform)
        {
            transform.position = Position;
            transform.rotation = Rotation;
        }
    }
}