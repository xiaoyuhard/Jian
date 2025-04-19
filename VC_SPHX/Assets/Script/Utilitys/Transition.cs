using UnityEngine;

namespace RTS 
{
    public class Transition : MonoBehaviour, IDisplayStatusCommand
    {
        /// <summary>
        /// 过渡模式
        /// </summary>
        [SerializeField] private TransitionMode _transitionMode;
        /// <summary>
        /// 过渡类型
        /// </summary>
        [SerializeField] private LeanTweenType _tweenType;
        /// <summary>
        /// 过渡时间
        /// </summary>
        [SerializeField] private float _duration;
        /// <summary>
        /// 延迟时间
        /// </summary>
        [SerializeField] private float _delay;
        /// <summary>
        /// 距离
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