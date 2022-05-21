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
            _initialData = new InteractableData(transform, _id);
        }

        public abstract void Interact();

        public void SetState(InteractableData data)
        {
            data.RestoreState(transform);
        }

        public InteractableData GetState()
        {
            return new InteractableData(transform, _id);
        }

        public void RestoreDefaultState()
        {
            _initialData.RestoreState(transform);
        }
    }

    public struct InteractableData
    {
        public string Id { get; }
        public Vector3 Position { get;  }
        public Quaternion Rotation { get;  }

        public InteractableData(Transform transform, string id)
        {
            Id = id;
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