using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary> Hiện ứng chuyển cảnh </summary>
    public class LevelLoader : MonoBehaviour
    {
        public Animator _anim;

        private void Start()
        {
            _anim = GetComponentInChildren<Animator>();
            ScenesManager._OnLoadNextScene += LoadLevel;
        }

        void LoadLevel()
        {
            if (_anim)
                _anim.SetTrigger("Start");
        }


    }
}
