using System.Collections;
using System.Collections.Generic;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class CustomerPooler : ObjectPooler
    {
        public static CustomerPooler Instance { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }



    }
}