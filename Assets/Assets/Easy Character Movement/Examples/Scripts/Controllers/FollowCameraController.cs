using System.Collections;
using UnityEngine;

namespace ECM.Examples
{
    public sealed class FollowCameraController : MonoBehaviour
    {
        #region PUBLIC FIELDS

        [SerializeField]
        private Transform _targetTransform;

        [SerializeField]
        private float _distanceToTarget = 15.0f;

        [SerializeField]
        private float _followSpeed = 3.0f;

        [SerializeField]
        private float _cameraXRotation = 70;

        #endregion

        private IEnumerator _enumerator;

        #region PROPERTIES

        public Transform targetTransform
        {
            get { return _targetTransform; }
            set { _targetTransform = value; }
        }

        public Vector3 cameraAngle
        {
            get { return new Vector3(_cameraXRotation, 0, 0); }
        }

        public float distanceToTarget
        {
            get { return _distanceToTarget; }
            set { _distanceToTarget = Mathf.Max(0.0f, value); }
        }

        public float followSpeed
        {
            get { return _followSpeed; }
            set { _followSpeed = Mathf.Max(0.0f, value); }
        }

        private Vector3 cameraRelativePosition
        {
            get { return targetTransform.position - transform.forward * distanceToTarget; }
        }

        #endregion


        public void ChangeRotation(float x)
        {
            if (_enumerator != null)
                StopCoroutine(_enumerator);
            _enumerator = ChangeRotationIE(x);
            StartCoroutine(_enumerator);
        }
        public IEnumerator ChangeRotationIE(float x)
        {
            float delta = x - transform.localEulerAngles.x;
            var endTime = new WaitForEndOfFrame();
            //Debug.Log(transform.localEulerAngles.x);
            while (Mathf.Abs(transform.localEulerAngles.x - x) > Mathf.Abs(0.5f))
            {
                //Debug.Log(Mathf.Abs(transform.localEulerAngles.x - x)+" "+ 1 / 30.0f+" "+ (Mathf.Abs(transform.localEulerAngles.x - x) > Mathf.Abs(1 / 30.0f)));
                _cameraXRotation += delta * Time.deltaTime;
                yield return endTime;
            }
            _cameraXRotation = x;
        }

        #region MONOBEHAVIOUR

        public void OnValidate()
        {
            distanceToTarget = _distanceToTarget;
            followSpeed = _followSpeed;
        }

        public void Awake()
        {
            transform.position = cameraRelativePosition;
        }

        public void LateUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cameraRelativePosition, followSpeed * Time.deltaTime);
            transform.localEulerAngles = cameraAngle;
        }

        #endregion
    }
}