using UnityEngine;

namespace Core
{
    /// <summary> Hiện ứng chuyển cảnh </summary>
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] Animator _animator;

        private void Start()
        {
            _animator = GetComponentInChildren<Animator>();
            ScenesManager._OnLoadNextScene += LoadLevel;
        }

        private void LoadLevel()
        {
            if (_animator)
            {
                _animator.SetTrigger("Start");
            }
        }


    }
}
