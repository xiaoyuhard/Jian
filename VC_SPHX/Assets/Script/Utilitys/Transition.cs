using UnityEngine;

namespace RTS 
{
    public class Transition : MonoBehaviour, IDisplayStatusCommand
    {
        /// <summary>
        /// ����ģʽ
        /// </summary>
        [SerializeField] private TransitionMode _transitionMode;
        /// <summary>
        /// ��������
        /// </summary>
        [SerializeField] private LeanTweenType _tweenType;
        /// <summary>
        /// ����ʱ��
        /// </summary>
        [SerializeField] private float _duration;
        /// <summary>
        /// �ӳ�ʱ��
        /// </summary>
        [SerializeField] private float _delay;
        /// <summary>
        /// ����
        /// </summary>
        [SerializeField] private float _distance;

        private Vector3 _defaultPosition;

        private void Start()
        {
            Init();
        }

        public void Close()
        {
            LeanTween.cancel(this.gameObject);
            LeanTween.moveLocal(this.gameObject, _defaultPosition, _duration).setEase(_tweenType);
        }

        public void Open()
        {
            LeanTween.cancel(this.gameObject);
            switch (_transitionMode)
            {
                case TransitionMode.ToTOP:
                    LeanTween.moveLocalY(this.gameObject, -_distance, _duration).setEase(_tweenType).setDelay(_delay);
                    break;
                case TransitionMode.ToRight:
                    LeanTween.moveLocalX(this.gameObject, _distance, _duration).setEase(_tweenType).setDelay(_delay);
                    break;
                case TransitionMode.ToLeft:
                    LeanTween.moveLocalX(this.gameObject, -_distance, _duration).setEase(_tweenType).setDelay(_delay);
                    break;
                case TransitionMode.ToBottom:
                    LeanTween.moveLocalY(this.gameObject, _distance, _duration).setEase(_tweenType).setDelay(_delay);
                    break;
            }
        }

        public void Init()
        {
            _defaultPosition = transform.localPosition;
        }
    }
}